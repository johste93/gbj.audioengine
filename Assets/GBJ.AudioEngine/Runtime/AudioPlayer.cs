using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Audio;
using GBJ.AudioEngine.Settings;
using GBJ.AudioEngine.Effects;

namespace GBJ.AudioEngine
{
    public class AudioPlayer : MonoBehaviour
    {
        public AudioSource Source;

        [HideInInspector] public AudioChorusFilter ChorusFilter;
        [HideInInspector] public AudioDistortionFilter DistortionFilter;
        [HideInInspector] public AudioEchoFilter EchoFilter;
        [HideInInspector] public AudioHighPassFilter HighPassFilter;
        [HideInInspector] public AudioLowPassFilter LowPassFilter;
        [HideInInspector] public AudioReverbFilter ReverbFilter;

        private Coroutine playRoutine;
        private AssetReference assetReference;
        private AudioDelaySettings audioDelaySettings;

        public void Play()
        {
            if(assetReference.RuntimeKeyIsValid() && Source.clip == null)
            {
                LoadAssetReference(assetReference, (success, clip)=>
                {
                    if(!success)
                    {
                        UnityEngine.Debug.LogError($"Failed loading asset referance: {assetReference}");
                        return;
                    }

                    SetAudioClip(clip);
                    
                    if(Application.isPlaying)
                        playRoutine = StartCoroutine(PlayRoutine());
                    else
                        playRoutine = StartCoroutine(EditorPlayRoutine());
                });
            }
            else
            {
                if(Application.isPlaying)
                    playRoutine = StartCoroutine(PlayRoutine());
                else
                    playRoutine = StartCoroutine(EditorPlayRoutine());
            }
        }

        public void Stop()
        {
            if(playRoutine != null)
                StopCoroutine(playRoutine);

            playRoutine = null;

            Source.Stop();
        }

        public void StopAndKill()
        {
            Stop();
            Destroy();
        }

        private IEnumerator PlayRoutine()
        {
            float delay = 0;

            if(audioDelaySettings != null && audioDelaySettings.RandomDelay)
                delay = Random.Range(audioDelaySettings.MinDelay, audioDelaySettings.MaxDelay);
            else
                delay = audioDelaySettings.Delay;

            if(delay > 0f)
                yield return new WaitForSeconds(delay);
            else
                yield return new WaitForEndOfFrame();
                
            if(Source.clip == null)
                UnityEngine.Debug.LogError("AudioClip is null!");
            else
                Source.Play();
            
            while (Source.loop || Source.isPlaying || Source.IsPaused())
            {
                yield return 0;
            }
            
            Destroy();
        }

        private IEnumerator EditorPlayRoutine()
        {
            yield return new WaitForEndOfFrame();

            if(Source.clip == null)
                UnityEngine.Debug.LogError("AudioClip is null!");
            else
                Source.Play();

            while(Source.isPlaying)
                yield return 0;

            Destroy();
        }

        public void LoadAudioEvent(AudioEvent audioEvent)
        {
            assetReference = audioEvent.GetAssetReferance();

            if(audioEvent.SurviveSceneChanges)
                SurviveSceneChanges();

            SetDelay(
                audioEvent.AudioSourceSettings.Delay,
                audioEvent.AudioSourceSettings.RandomDelay,
                audioEvent.AudioSourceSettings.MinDelay,
                audioEvent.AudioSourceSettings.MaxDelay
                );

            SetAudioMixerGroup(
                audioEvent.AudioSourceSettings.Output
            );

            SetBypassEffects(
                audioEvent.AudioSourceSettings.BypassEffects
            );

            SetBypassListenerEffects(
                audioEvent.AudioSourceSettings.BypassListenerEffects
                );

            SetBypassReverbZones(
                audioEvent.AudioSourceSettings.BypassReverbZones
            );

            SetLoop(
                Source.loop = audioEvent.AudioSourceSettings.Loop && Application.isPlaying
                );
            if(audioEvent.AudioSourceSettings.VolumeOverLifetimeType == AudioOverLifetimeType.Curve)
                SetVolumeOverLifetime(
                    audioEvent.AudioSourceSettings.VolumeOverLifetime, 
                    audioEvent.AudioSourceSettings.VolumeOverLifetimeCurve
                );

            if(audioEvent.AudioSourceSettings.VolumeOverLifetimeType == AudioOverLifetimeType.Precise)
                SetVolumeOverLifetime(
                    audioEvent.AudioSourceSettings.VolumeOverLifetime, 
                    audioEvent.AudioSourceSettings.VolumeFadeInPosition,
                    audioEvent.AudioSourceSettings.VolumeFadeOutPosition
                );

            if(audioEvent.AudioSourceSettings.PitchOverLifetimeType == AudioOverLifetimeType.Curve)
                SetPitchOverLifetime(
                    audioEvent.AudioSourceSettings.PitchOverLifetime, 
                    audioEvent.AudioSourceSettings.PitchOverLifetimeCurve
                );

            if(audioEvent.AudioSourceSettings.PitchOverLifetimeType == AudioOverLifetimeType.Precise)
                SetPitchOverLifetime(
                    audioEvent.AudioSourceSettings.PitchOverLifetime, 
                    audioEvent.AudioSourceSettings.PitchFadeInPosition,
                    audioEvent.AudioSourceSettings.PitchFadeOutPosition
                );
            if (audioEvent.AudioSourceSettings.RandomVolume)
                SetVolume(audioEvent.AudioSourceSettings.MinVolume, audioEvent.AudioSourceSettings.MaxVolume);
            else
                SetVolume(audioEvent.AudioSourceSettings.Volume);
            
            if(audioEvent.AudioSourceSettings.RandomPitch)
                SetPitch(audioEvent.AudioSourceSettings.MinPitch, audioEvent.AudioSourceSettings.MaxPitch);
            else
                SetPitch(audioEvent.AudioSourceSettings.Pitch);

            SetStereoPan(
                audioEvent.AudioSourceSettings.StereoPan,
                audioEvent.AudioSourceSettings.RandomStereoPan,
                audioEvent.AudioSourceSettings.MinStereoPan,
                audioEvent.AudioSourceSettings.MaxStereoPan
            );

            if(audioEvent.AudioSourceSettings.PitchOverLifetimeType == AudioOverLifetimeType.Curve)
                SetStereoPanOverLifetime(
                    audioEvent.AudioSourceSettings.StereoPanOverLifetime, 
                    audioEvent.AudioSourceSettings.StereoPanOverLifetimeCurve
                );
            
            if(audioEvent.AudioSourceSettings.PitchOverLifetimeType == AudioOverLifetimeType.Precise)
                SetStereoPanOverLifetime(
                    audioEvent.AudioSourceSettings.StereoPanOverLifetime, 
                    audioEvent.AudioSourceSettings.StereoPanFadeInPosition,
                    audioEvent.AudioSourceSettings.StereoPanFadeOutPosition
                );

            Set3DSoundSettings(
                audioEvent.AudioSourceSettings.SpatialBlend,
                audioEvent.AudioSourceSettings.DopplerLevel,
                audioEvent.AudioSourceSettings.Spread,
                audioEvent.AudioSourceSettings.MinDistance,
                audioEvent.AudioSourceSettings.MaxDistance,
                audioEvent.AudioSourceSettings.AudioRolloffMode,
                audioEvent.AudioSourceSettings.CustomRolloff
            );

            SetPriority(
                audioEvent.AudioSourceSettings.Priority
            );

            SetAudioChorusFilterSettings(
                audioEvent.AudioChorusFilterSettings.Enabled,
                audioEvent.AudioChorusFilterSettings.DryMix,
                audioEvent.AudioChorusFilterSettings.WetMix1,
                audioEvent.AudioChorusFilterSettings.WetMix2,
                audioEvent.AudioChorusFilterSettings.WetMix3,
                audioEvent.AudioChorusFilterSettings.Delay,
                audioEvent.AudioChorusFilterSettings.Rate,
                audioEvent.AudioChorusFilterSettings.Depth
                );

            SetAudioDistortionFilterSettings(
                audioEvent.AudioDistortionFilterSettings.Enabled,
                audioEvent.AudioDistortionFilterSettings.DistortionLevel
            );

            SetAudioEchoFilterSettings(
                audioEvent.AudioEchoFilterSettings.Enabled,
                audioEvent.AudioEchoFilterSettings.Delay,
                audioEvent.AudioEchoFilterSettings.DecayRatio,
                audioEvent.AudioEchoFilterSettings.DryMix,
                audioEvent.AudioEchoFilterSettings.WetMix
            );

            SetAudioHighPassFilterSettings(
                audioEvent.AudioHighPassFilterSettings.Enabled,
                audioEvent.AudioHighPassFilterSettings.CutoffFrequency,
                audioEvent.AudioHighPassFilterSettings.HighpassResonanceQ
            );

            SetAudioLowPassFilterSettings(
                audioEvent.AudioLowPassFilterSettings.Enabled,
                audioEvent.AudioLowPassFilterSettings.CutoffFrequency,
                audioEvent.AudioLowPassFilterSettings.LowpassResonanceQ
            );

            SetAudioReverbFilterSettings(
                audioEvent.AudioReverbFilterSettings.ReverbPreset != AudioReverbPreset.Off,
                audioEvent.AudioReverbFilterSettings.ReverbPreset,
                audioEvent.AudioReverbFilterSettings.DryLevel,
                audioEvent.AudioReverbFilterSettings.Room,
                audioEvent.AudioReverbFilterSettings.RoomHF,
                audioEvent.AudioReverbFilterSettings.RoomLF,
                audioEvent.AudioReverbFilterSettings.DecayTime,
                audioEvent.AudioReverbFilterSettings.DecayHFRatio,
                audioEvent.AudioReverbFilterSettings.ReflectionsLevel,
                audioEvent.AudioReverbFilterSettings.ReflectionsDelay,
                audioEvent.AudioReverbFilterSettings.ReverbLevel,
                audioEvent.AudioReverbFilterSettings.ReverbDelay,
                audioEvent.AudioReverbFilterSettings.HfReferance,
                audioEvent.AudioReverbFilterSettings.LfReferance,
                audioEvent.AudioReverbFilterSettings.Diffusion,
                audioEvent.AudioReverbFilterSettings.Density
            );
        }
        
        public AudioPlayer SetAudioClip(AudioClip clip)
        {
            Source.clip = clip;
            gameObject.name = $"AudioPlayer ({Source.clip.name})";
            return this;
        }

        public AudioPlayer SetDelay(float constant, bool random, float min, float max)
        {
            audioDelaySettings = new AudioDelaySettings(){
                Delay = constant,
                RandomDelay = random,
                MinDelay = min,
                MaxDelay = max
            };
            return this;
        }
        public AudioPlayer SetVolume(float constant)
        {
            Source.volume = constant;
            return this;
        }

        public AudioPlayer SetVolume(float min, float max)
        {
            Source.volume = Random.Range(min, max);
            return this;
        }
        
        public AudioPlayer SetVolumeOverLifetime(bool enabled, float fadeInPosition, float fadeOutPosition)
        {
            var effect = gameObject.GetComponent<VolumeOverLifetimeEffect>();
            if(effect == null && enabled)
                effect = gameObject.AddComponent<VolumeOverLifetimeEffect>();

            if(!effect)
                return this;

            effect.enabled = enabled;
            effect.Type = AudioOverLifetimeType.Precise;
            effect.InPosition = fadeInPosition;
            effect.OutPosition = fadeOutPosition;

            return this;
        }
        
        public AudioPlayer SetVolumeOverLifetime(bool enabled, AnimationCurve volumeOverLifetimeCurve)
        {
            var effect = gameObject.GetComponent<VolumeOverLifetimeEffect>();
            if(effect == null && enabled)
                effect = gameObject.AddComponent<VolumeOverLifetimeEffect>();

            if(!effect)
                return this;

            effect.enabled = enabled;
            effect.Type = AudioOverLifetimeType.Curve;
            effect.Curve = volumeOverLifetimeCurve;
            return this;
        }

        public AudioPlayer SetPitch(float constant)
        {
            Source.pitch = constant;
            return this;
        }
        
        public AudioPlayer SetPitch(float min, float max)
        {
            Source.pitch = Random.Range(min, max);
            return this;
        }

        public AudioPlayer SetPitchOverLifetime(bool enabled, float fadeInPosition, float fadeOutPosition)
        {
            var effect = gameObject.GetComponent<PitchOverLifetimeEffect>();
            if(effect == null && enabled)
                effect = gameObject.AddComponent<PitchOverLifetimeEffect>();

            if(!effect)
                return this;

            effect.enabled = enabled;
            effect.Type = AudioOverLifetimeType.Precise;
            effect.InPosition = fadeInPosition;
            effect.OutPosition = fadeOutPosition;

            return this;
        }

        public AudioPlayer SetPitchOverLifetime(bool enabled, AnimationCurve pitchOverLifetimeCurve)
        {
            var effect = gameObject.GetComponent<PitchOverLifetimeEffect>();
            if(effect == null && enabled)
                effect = gameObject.AddComponent<PitchOverLifetimeEffect>();
            
            if(!effect)
                return this;

            effect.enabled = enabled;
            effect.Type = AudioOverLifetimeType.Curve;
            effect.Curve = pitchOverLifetimeCurve;

            return this;
        }

        public AudioPlayer SetStereoPan(float constant, bool random, float min, float max)
        {
            if(random)
                Source.panStereo = Random.Range(min, max);
            else
                Source.panStereo = constant;

            return this;
        }

        public AudioPlayer SetStereoPanOverLifetime(bool enabled, float fadeInPosition, float fadeOutPosition)
        {
            var effect = gameObject.GetComponent<StereoPanOverLifetimeEffect>();
            if(effect == null && enabled)
                effect = gameObject.AddComponent<StereoPanOverLifetimeEffect>();

            if(!effect)
                return this;

            effect.enabled = enabled;
            effect.Type = AudioOverLifetimeType.Precise;
            effect.InPosition = fadeInPosition;
            effect.OutPosition = fadeOutPosition;

            return this;
        }

        public AudioPlayer SetStereoPanOverLifetime(bool enabled, AnimationCurve stereoPanOverLifetimeCurve)
        {
            var effect = gameObject.GetComponent<StereoPanOverLifetimeEffect>();
            if(effect == null && enabled)
                effect = gameObject.AddComponent<StereoPanOverLifetimeEffect>();
            
            if(!effect)
                return this;

            effect.enabled = enabled;
            effect.Type = AudioOverLifetimeType.Curve;
            effect.Curve = stereoPanOverLifetimeCurve;

            return this;
        }

        public AudioPlayer Set3DSoundSettings(float spatialBlend, float dopplerLevel, float spread, float minDistance, float maxDistance, AudioRolloffMode mode, AnimationCurve customCurve)
        {
            Source.spatialBlend = spatialBlend;
            Source.dopplerLevel = dopplerLevel;
            Source.spread = spread;
            Source.minDistance = minDistance;
            Source.maxDistance = maxDistance;
            Source.rolloffMode = mode;
            Source.SetCustomCurve(AudioSourceCurveType.CustomRolloff, customCurve);

            return this;
        } 

        public AudioPlayer SetPriority(int priority)
        {
            Source.priority = priority;
            return this;
        }

        public AudioPlayer SetAudioMixerGroup(AudioMixerGroup group)
        {
            Source.outputAudioMixerGroup = group;
            return this;
        }

        public AudioPlayer SetBypassEffects(bool bypass)
        {
            Source.bypassEffects = bypass;
            return this;
        }

        public AudioPlayer SetBypassListenerEffects(bool bypass)
        {
            Source.bypassListenerEffects = bypass;
            return this;
        }

        public AudioPlayer SetBypassReverbZones(bool bypass)
        {
            Source.bypassReverbZones = bypass;
            return this;
        }

        public AudioPlayer SetLoop(bool loop)
        {
            Source.loop = loop;
            return this;
        }

        public AudioPlayer SetAudioChorusFilterSettings(bool enabled, float dryMix, float wetMix1, float wetMix2, float wetMix3, float delay, float rate, float depth)
        {
            if(ChorusFilter == null)
            {
                if(!enabled)
                    return this;

                ChorusFilter = gameObject.AddComponent<AudioChorusFilter>();
            }

            ChorusFilter.enabled = enabled;
            ChorusFilter.dryMix = dryMix;
            ChorusFilter.wetMix1 = wetMix1;
            ChorusFilter.wetMix2 = wetMix2;
            ChorusFilter.wetMix3 = wetMix3;
            ChorusFilter.delay = delay;
            ChorusFilter.rate = rate;
            ChorusFilter.depth = depth;
            return this;
        }

        public AudioPlayer SetAudioDistortionFilterSettings(bool enabled, float distortionLevel)
        {
            if(DistortionFilter == null)
            {
                if(!enabled)
                    return this;

                DistortionFilter = gameObject.AddComponent<AudioDistortionFilter>();
            }
                
            DistortionFilter.enabled = enabled;
            DistortionFilter.distortionLevel = distortionLevel;
            return this;
        }

        public AudioPlayer SetAudioEchoFilterSettings(bool enabled, float delay, float decayRatio, float dryMix, float wetMix)
        {
            if(EchoFilter == null)
            {
                if(!enabled)
                    return this;
                
                EchoFilter = gameObject.AddComponent<AudioEchoFilter>();
            }    

            EchoFilter.enabled = enabled;
            EchoFilter.delay = delay;
            EchoFilter.decayRatio = decayRatio;
            EchoFilter.dryMix = dryMix;
            EchoFilter.wetMix = wetMix;
            return this;
        }

        public AudioPlayer SetAudioHighPassFilterSettings(bool enabled, float cutoffFrequency, float highpassResonanceQ)
        {
            if(HighPassFilter == null)
            {
                if(!enabled)
                    return this;

                HighPassFilter = gameObject.AddComponent<AudioHighPassFilter>();
            }
                
            HighPassFilter.enabled = enabled;
            HighPassFilter.cutoffFrequency = cutoffFrequency;
            HighPassFilter.highpassResonanceQ = highpassResonanceQ;
            return this;
        }

        public AudioPlayer SetAudioLowPassFilterSettings(bool enabled, float cutoffFrequency, float lowpassResonanceQ)
        {
            if(LowPassFilter == null)
            {
                if(!enabled)
                    return this;

                LowPassFilter = gameObject.AddComponent<AudioLowPassFilter>();
            }
                
            LowPassFilter.enabled = enabled;
            LowPassFilter.cutoffFrequency = cutoffFrequency;
            LowPassFilter.lowpassResonanceQ = lowpassResonanceQ;
            return this;
        }

        public AudioPlayer SetAudioReverbFilterSettings(bool enabled, AudioReverbPreset reverbPreset, float dryLevel, float room, float roomHF, float roomLF, float decayTime, float decayHFRatio, float reflectionsLevel, float reflectionsDelay, float reverbLevel, float reverbDelay, float hfReferance, float lfReferance, float diffusion, float density)
        {
            if(ReverbFilter == null)
            {
                if(!enabled)
                    return this;

                ReverbFilter = gameObject.AddComponent<AudioReverbFilter>();
            }
               
            ReverbFilter.enabled = enabled;
            ReverbFilter.reverbPreset = reverbPreset;
            if(ReverbFilter.reverbPreset == AudioReverbPreset.Off)
                return this;
            
            ReverbFilter.dryLevel = dryLevel;
            ReverbFilter.room = room;
            ReverbFilter.roomHF = roomHF;
            ReverbFilter.roomLF = roomLF;
            ReverbFilter.decayTime = decayTime;
            ReverbFilter.decayHFRatio = decayHFRatio;
            ReverbFilter.reflectionsLevel = reflectionsLevel;
            ReverbFilter.reflectionsDelay = reflectionsDelay;
            ReverbFilter.reverbLevel = reverbLevel;
            ReverbFilter.reverbDelay = reverbDelay;
            ReverbFilter.hfReference = hfReferance;
            ReverbFilter.lfReference = lfReferance;
            ReverbFilter.diffusion = diffusion;
            ReverbFilter.density = density;
            return this;
        }
        
        public AudioPlayer SurviveSceneChanges()
        {
            DontDestroyOnLoad(gameObject);
            return this;
        }

        public void Destroy()
        {
            if(assetReference.RuntimeKeyIsValid())
                assetReference.ReleaseAsset();
            
#if UNITY_EDITOR
            Unsubscribe();
            
            if(Application.isPlaying)
                Destroy(gameObject);
            else
                DestroyImmediate(gameObject);
            
            return;
#endif
            Destroy(gameObject);
        }
        
#if UNITY_EDITOR
        private AudioEvent subscribedToEvent;
        public void Subscribe(AudioEvent audioEvent)
        {
            Unsubscribe();
            subscribedToEvent = audioEvent;
            subscribedToEvent.OnEventChanged += OnEventChanged;
        }

        public void Unsubscribe()
        {
            if (subscribedToEvent == null)
                return;
            
            subscribedToEvent.OnEventChanged -= OnEventChanged;
            subscribedToEvent = null;
        }
        
        private void OnEventChanged(AudioEvent audioEvent)
        {
            LoadAudioEvent(audioEvent);
        }
#endif
        
        private void LoadAssetReference(AssetReference referance, System.Action<bool, AudioClip> onLoaded)
        {
#if UNITY_EDITOR
            var path = UnityEditor.AssetDatabase.GUIDToAssetPath(referance.AssetGUID);
            AudioClip clip = UnityEditor.AssetDatabase.LoadAssetAtPath<AudioClip>(path);
            onLoaded.Invoke(clip != null, clip);
#else
            if (referance.OperationHandle.IsValid())
            {
                Addressables.ResourceManager.Acquire(referance.OperationHandle);
                onLoaded.Invoke(referance.OperationHandle.Status == AsyncOperationStatus.Succeeded, (AudioClip) referance.OperationHandle.Result);
                return;
            }
            
            referance.LoadAssetAsync<AudioClip>().Completed += handle => 
            {
                onLoaded.Invoke(handle.Status == AsyncOperationStatus.Succeeded, handle.Result);
            };
#endif
        }
    }
}
using UnityEngine;
using UnityEngine.Audio;

namespace GBJ.AudioEngine.Settings
{
    [System.Serializable]
    public class AudioSourceSettings
    {
        public bool Foldout = false;

        public AudioMixerGroup Output = null;

        public bool BypassEffects = false;
        public bool BypassListenerEffects = false;
        public bool BypassReverbZones = false;
        public bool Loop = false;

        public float Volume = 1f;
        public bool RandomVolume = false;
        public float MinVolume = 0f;
        public float MaxVolume = 1f;
        public bool VolumeOverLifetime = false;
        public AudioOverLifetimeType VolumeOverLifetimeType;
        public float VolumeFadeInPosition;
        public float VolumeFadeOutPosition;
        public AnimationCurve VolumeOverLifetimeCurve;

        public float Pitch = 1f;
        public bool RandomPitch = false;
        public float MinPitch = -3f;
        public float MaxPitch = 3f;
        public bool PitchOverLifetime = false;
        public AudioOverLifetimeType PitchOverLifetimeType;
        public float PitchFadeInPosition;
        public float PitchFadeOutPosition;
        public AnimationCurve PitchOverLifetimeCurve;

        public float StereoPan = 0;
        public bool RandomStereoPan = false;
        public float MinStereoPan = -1f;
        public float MaxStereoPan = 1f;
        public bool StereoPanOverLifetime = false;
        public AudioOverLifetimeType stereoPanOverLifetimeType;
        public float StereoPanFadeInPosition;
        public float StereoPanFadeOutPosition;
        public AnimationCurve StereoPanOverLifetimeCurve;

        public float Delay = 0f;
        public bool RandomDelay = false;
        public float MinDelay = 0f;
        public float MaxDelay = 1f;

        public int Priority = 128;
        
        public bool Foldout3DSoundSettings = false;
        public float SpatialBlend = 0;
        public float DopplerLevel = 1;
        public float Spread = 0f;
        public float MinDistance = 1;
        public float MaxDistance = 500;
        public AudioRolloffMode AudioRolloffMode = AudioRolloffMode.Logarithmic;
        public AnimationCurve CustomRolloff = new AnimationCurve();
    }
}
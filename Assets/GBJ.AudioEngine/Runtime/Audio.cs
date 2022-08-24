using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GBJ.AudioEngine
{
    public class Audio
    {
        internal const string MainVolumeTag = "Main";
        private const int DefaultVolume = 1;
        private static bool _initalized;
        private static bool _isMuted;
        private static Dictionary<string, float> _tagVolumes = new Dictionary<string, float>();

        public delegate void VolumeTagEvent(string tag, float volume);
        public static VolumeTagEvent OnVolumeChangedEvent;
        
        public delegate void MuteEvent(bool isMuted);
        public static MuteEvent OnMuteEvent;

        
        
        private static Dictionary<string, AudioEvent> audioEvents;
        private static GameObject _audioPlayerPrefab;
        private static readonly List<AudioPlayer> livingAudioPlayers = new List<AudioPlayer>();
        private static GameObject AudioPlayerPrefab
        {
            get{
                if(_audioPlayerPrefab == null)
                    _audioPlayerPrefab = Resources.Load<GameObject>("AudioPlayer");
                
                return _audioPlayerPrefab;
            }
        }

        public static AudioPlayer Play(string eventName)
        {
            if(!audioEvents.ContainsKey(eventName))
            {
                Debug.LogError($"AudioEvent \"{eventName}\" not found!");
                return null;
            }
            return Play(audioEvents[eventName]);
        }

        public static AudioPlayer Play(AudioEvent audioEvent)
        {
            GameObject instantiatedPrefab = GameObject.Instantiate(AudioPlayerPrefab);
            instantiatedPrefab.hideFlags = HideFlags.DontSaveInEditor;
            AudioPlayer player = instantiatedPrefab.GetComponent<AudioPlayer>();

#if UNITY_EDITOR
            player.SubscribeToAudioEventChange(audioEvent);
#endif
            
            player.LoadAudioEvent(audioEvent);
            player.Play();

            return player;
        }

        public static List<AudioPlayer> GetLivingAudioPlayers() => livingAudioPlayers;
        internal static void RegisterAudioPlayer(AudioPlayer audioPlayer) => livingAudioPlayers.Add(audioPlayer);
        internal static void UnregisterAudioPlayer(AudioPlayer audioPlayer) => livingAudioPlayers.Remove(audioPlayer);

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnBeforeSceneLoadRuntimeMethod() => Initalize();

        public static void Initalize()
        {
            if (_initalized)
                return;

            _initalized = true;
            LoadEvents();
            _tagVolumes = new Dictionary<string, float>() { {"Main", DefaultVolume} };
            foreach (var tag in audioEvents.SelectMany(x => x.Value.Tags))
                AddTag(tag);
        }

        private static void LoadEvents()
        {
            //Debug.Log("Loading Audio Events");
            audioEvents = new Dictionary<string, AudioEvent>();
            AudioEvent[] loadedAudioEvents = Resources.LoadAll<AudioEvent>("AudioEvents");
            foreach(AudioEvent audioEvent in loadedAudioEvents)
            {
                if(audioEvents.ContainsKey(audioEvent.name))
                {
#if UNITY_EDITOR
                    UnityEditor.EditorUtility.DisplayDialog("AudioManagment Error!", $"Found duplicate audio event name:\n\n\"{audioEvent.Name}\".\n\nAudio event names must be unique!", "Please forgive me!");
#endif
                    Debug.LogError($"Found duplicate audio event name: \"{audioEvent.Name}\". Audio event names must be unique!");
                    continue;
                }
                else
                    audioEvents.Add(audioEvent.Name, audioEvent);
            }
        }
        
        public static bool IsMuted() => _isMuted;

        public static void Mute()
        {
            _isMuted = true;
            OnMuteEvent?.Invoke(_isMuted);
        }

        public static void Unmute()
        {
            _isMuted = false;
            OnMuteEvent?.Invoke(_isMuted);
        }
        
        public static float GetMainVolume()
        {
            return _tagVolumes[MainVolumeTag];
        }

        public static void AddTag(string tag)
        {
            if (_tagVolumes.ContainsKey(tag))
                return;

            _tagVolumes.Add(tag, DefaultVolume);
        }

        public static float GetVolumeByTag(string tag)
        {
            if (_tagVolumes.ContainsKey(tag))
                return _tagVolumes[tag];

            return DefaultVolume;
        }

        public static void SetVolumeByTag(string tag, float value)
        {
            _tagVolumes[tag] = Mathf.Clamp01(value);
            OnVolumeChangedEvent?.Invoke(tag, _tagVolumes[tag]);
        }
    }
}
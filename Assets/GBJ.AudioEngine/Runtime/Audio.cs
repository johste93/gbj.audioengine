using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GBJ.AudioEngine
{
    public class Audio
    {
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
            player.Subscribe(audioEvent);
#endif
            
            player.LoadAudioEvent(audioEvent);
            player.Play();

            return player;
        }

        public static List<AudioPlayer> GetLivingAudioPlayers() => livingAudioPlayers;
        internal static void RegisterAudioPlayer(AudioPlayer audioPlayer) => livingAudioPlayers.Add(audioPlayer);
        internal static void UnregisterAudioPlayer(AudioPlayer audioPlayer) => livingAudioPlayers.Remove(audioPlayer);
        
        [RuntimeInitializeOnLoadMethod]
        private static void OnRuntimeMethodLoad() => LoadEvents();

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
    }
}
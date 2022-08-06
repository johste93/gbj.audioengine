using System.Collections.Generic;
using UnityEngine;
using GBJ.AudioEngine.Settings;

namespace GBJ.AudioEngine
{
    
    [CreateAssetMenu(fileName = "New Audio Event", menuName = "AudioEngine/New Audio Event")]
    public class AudioEvent : ScriptableObject
    {
        public string Name;
        
        public AssetReferenceAudioClip[] AssetReferances;
        
        public PlayOrder PlayOrder;
        public bool SurviveSceneChanges;
        public List<string> Tags;

        public AudioSourceSettings                  AudioSourceSettings;
        public AudioChorusFilterSettings            AudioChorusFilterSettings;
        public AudioDistortionFilterSettings        AudioDistortionFilterSettings;
        public AudioEchoFilterSettings              AudioEchoFilterSettings;
        public AudioHighPassFilterSettings          AudioHighPassFilterSettings;
        public AudioLowPassFilterSettings           AudioLowPassFilterSettings;
        public AudioReverbFilterSettings            AudioReverbFilterSettings;

        private int previousIndex = -1;
        private AssetReferenceAudioClip previousAssetReferance;
        private List<AssetReferenceAudioClip> _audioClips;
        
#if UNITY_EDITOR
        public System.Action<AudioEvent> OnEventChanged;        
#endif
        
        public AssetReferenceAudioClip GetAssetReferance()
        {
            switch(PlayOrder)
            {
                default:
                case PlayOrder.InOrder:
                    return LoadNextAudioClip();
                case PlayOrder.RandomNotTwice:
                    return LoadAudioClipRandomNotTwice();
                case PlayOrder.RandomNoRepeat:
                    return LoadAudioClipRandomNoRepeat();
            }
        }

        private AssetReferenceAudioClip LoadNextAudioClip()
        {
            previousIndex++;
            if(previousIndex >= AssetReferances.Length)
                previousIndex = 0;
            
            previousAssetReferance = AssetReferances[previousIndex];
            return previousAssetReferance;
        }

        private AssetReferenceAudioClip LoadAudioClipRandomNotTwice()
        {
            int randomIndex = Random.Range(0, AssetReferances.Length);

            while(randomIndex == previousIndex && AssetReferances.Length > 1)
                randomIndex = Random.Range(0, AssetReferances.Length);

            previousAssetReferance = AssetReferances[randomIndex];
            return previousAssetReferance;
        }

        private AssetReferenceAudioClip LoadAudioClipRandomNoRepeat()
        {
            if(_audioClips == null || _audioClips.Count == 0)
                _audioClips = new List<AssetReferenceAudioClip>(AssetReferances);

            int randomIndex = Random.Range(0, _audioClips.Count);
            while(_audioClips.Count > 1 && previousAssetReferance == _audioClips[randomIndex])
                randomIndex = Random.Range(0, AssetReferances.Length);

            previousAssetReferance = _audioClips[randomIndex];
            _audioClips.RemoveAt(randomIndex);

            return previousAssetReferance;
        }
    }
}

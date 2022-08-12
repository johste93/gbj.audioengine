using System.Collections;
using UnityEngine;

namespace GBJ.AudioEngine.Effects
{
    public abstract class AudioOverLifetimeEffect : MonoBehaviour
    {
        public AudioOverLifetimeType Type;
        public AnimationCurve Curve;
        public float InPosition = -1;
        public float OutPosition = -1;
    
        protected float time = 0;
        protected float peek = 1f;
        protected AudioPlayer audioPlayer;
        private Coroutine playListenerRoutine;

        private void Awake()
        {
            audioPlayer = GetComponent<AudioPlayer>();
            playListenerRoutine = StartCoroutine(PlayListener());
        }

        private IEnumerator PlayListener()
        {
            while(!audioPlayer.IsPlaying())
                yield return 0;

            time = 0f;

            if(Type == AudioOverLifetimeType.Precise)
            {
                Curve = new AnimationCurve(new Keyframe[]{ new Keyframe(0f, 0f), new Keyframe(InPosition/audioPlayer.GetClipLength(), 1f), new Keyframe(OutPosition/audioPlayer.GetClipLength(), 1f), new Keyframe(1f, 0f)});
                Debug.Log("Curve Generated");
            }

            OnStartedPlaying();
        }

        protected abstract void OnStartedPlaying();

        protected virtual void Update()
        {
            if(!audioPlayer.HasClip())
                return;

            if(!audioPlayer.IsPlaying())
                return;

            time = audioPlayer.GetTime() / audioPlayer.GetClipLength();
        }

        private void OnDestroy()
        {
            if(playListenerRoutine != null)
                StopCoroutine(playListenerRoutine);

            playListenerRoutine = null;
        }
    }
}
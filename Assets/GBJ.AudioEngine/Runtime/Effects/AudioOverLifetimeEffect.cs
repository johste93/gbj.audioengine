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
        protected AudioSource source;
        private Coroutine playListenerRoutine;

        private void Awake()
        {
            source = GetComponent<AudioSource>();
            playListenerRoutine = StartCoroutine(PlayListener());
        }

        private IEnumerator PlayListener()
        {
            while(!source.isPlaying)
                yield return 0;

            time = 0f;

            if(Type == AudioOverLifetimeType.Precise)
            {
                Curve = new AnimationCurve(new Keyframe[]{ new Keyframe(0f, 0f), new Keyframe(InPosition/source.clip.length, 1f), new Keyframe(OutPosition/source.clip.length, 1f), new Keyframe(1f, 0f)});
                Debug.Log("Curve Generated");
            }

            OnStartedPlaying();
        }

        protected abstract void OnStartedPlaying();

        protected virtual void Update()
        {
            if(source.clip == null)
                return;

            if(!source.isPlaying)
                return;

            time = source.time / source.clip.length;
        }

        private void OnDestroy()
        {
            if(playListenerRoutine != null)
                StopCoroutine(playListenerRoutine);

            playListenerRoutine = null;
        }
    }
}
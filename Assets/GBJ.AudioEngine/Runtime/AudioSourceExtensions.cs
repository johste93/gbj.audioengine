using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GBJ.AudioEngine
{
    public static class AudioSourceExtensions
    {
        public static bool IsPaused(this AudioSource audioSource)
        {
            return !audioSource.isPlaying && audioSource.time > 0;
        }

        public static bool IsStopped(this AudioSource audioSource)
        {
            return !audioSource.isPlaying && audioSource.time == 0;
        }
    }
}
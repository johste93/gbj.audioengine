using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using GBJ.AudioEngine;

namespace GBJ.AudioEngine.DOTween
{
    public static class AudioPlayerExtensions
    {
        public static void Crossfade(this AudioPlayer outPlayer, AudioPlayer inPlayer, float volume, float duration)
        {
            outPlayer?.DoFade(0, duration).SetEase(Ease.Linear);
            inPlayer?.DoFade(volume, duration).SetEase(Ease.Linear);
        }
        
        public static void Crossfade(this AudioPlayer outPlayer, AudioPlayer inPlayer, float volume, float duration, AnimationCurve outCurve, AnimationCurve inCurve)
        {
            outPlayer?.DoFade(0, duration).SetEase(outCurve);
            inPlayer?.DoFade(volume, duration).SetEase(inCurve);
        }
    }
}
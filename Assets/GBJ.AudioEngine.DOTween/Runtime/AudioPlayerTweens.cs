using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;

namespace GBJ.AudioEngine.DOTween
{
    public static class AudioPlayerTweens
    {
        public static TweenerCore<float,float,FloatOptions> DoFade(this AudioPlayer player, float endValue, float duration)
        {
            return player.Source.DOFade(endValue, duration);
        }
        
        public static TweenerCore<float,float,FloatOptions> DOPitch(this AudioPlayer player, float endValue, float duration)
        {
            return player.Source.DOPitch(endValue, duration);
        }
    }
}
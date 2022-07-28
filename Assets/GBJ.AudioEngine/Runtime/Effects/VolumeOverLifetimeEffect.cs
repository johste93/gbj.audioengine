
namespace GBJ.AudioEngine.Effects
{
    public class VolumeOverLifetimeEffect : AudioOverLifetimeEffect
    {
        protected override void OnStartedPlaying()
        {
            peek = source.volume;
        }

        protected override void Update()
        {
            base.Update();
            if(Curve != null)
                source.volume = Curve.Evaluate(time) * peek;
        }
    }
}
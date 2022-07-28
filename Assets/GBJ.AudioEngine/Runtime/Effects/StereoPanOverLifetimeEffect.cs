
namespace GBJ.AudioEngine.Effects
{
    public class StereoPanOverLifetimeEffect : AudioOverLifetimeEffect
    {
        protected override void OnStartedPlaying()
        {
            peek = source.panStereo;
        }

        protected override void Update()
        {
            base.Update();
            if(Curve != null)
                source.panStereo = Curve.Evaluate(time) * peek;
        }
    }
}

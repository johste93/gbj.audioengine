
namespace GBJ.AudioEngine.Effects
{
    public class PitchOverLifetimeEffect : AudioOverLifetimeEffect
    {
        protected override void OnStartedPlaying()
        {
            peek = source.pitch;
        }

        protected override void Update()
        {
            base.Update();
            if(Curve != null)
                source.pitch = Curve.Evaluate(time) * peek;
        }
    }
}
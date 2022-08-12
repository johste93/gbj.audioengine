
namespace GBJ.AudioEngine.Effects
{
    public class PitchOverLifetimeEffect : AudioOverLifetimeEffect
    {
        protected override void OnStartedPlaying()
        {
            peek = audioPlayer.GetPitch();
        }

        protected override void Update()
        {
            base.Update();
            if(Curve != null)
                audioPlayer.SetPitch(Curve.Evaluate(time) * peek);
        }
    }
}
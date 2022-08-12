
namespace GBJ.AudioEngine.Effects
{
    public class StereoPanOverLifetimeEffect : AudioOverLifetimeEffect
    {
        protected override void OnStartedPlaying()
        {
            peek = audioPlayer.GetPanStereo();
        }

        protected override void Update()
        {
            base.Update();
            if (Curve != null)
                audioPlayer.SetStereoPan(Curve.Evaluate(time) * peek, false, 0, 0);
        }
    }
}

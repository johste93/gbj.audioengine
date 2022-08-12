
namespace GBJ.AudioEngine.Effects
{
    public class VolumeOverLifetimeEffect : AudioOverLifetimeEffect
    {
        protected override void OnStartedPlaying()
        {
            peek = audioPlayer.GetVolume();
        }

        protected override void Update()
        {
            base.Update();
            if(Curve != null)
                audioPlayer.SetVolume(Curve.Evaluate(time) * peek);
        }
    }
}
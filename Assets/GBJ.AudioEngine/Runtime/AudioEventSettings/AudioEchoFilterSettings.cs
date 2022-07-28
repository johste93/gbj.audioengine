
namespace GBJ.AudioEngine.Settings
{
    [System.Serializable]
    public class AudioEchoFilterSettings
    {
        public bool Foldout = false;
        public bool Enabled = false;
        public float Delay = 500;
        public float DecayRatio = 0.5f;
        public float DryMix = 1f;
        public float WetMix = 1f;
    }
}
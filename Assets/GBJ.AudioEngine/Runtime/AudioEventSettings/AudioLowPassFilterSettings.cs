
namespace GBJ.AudioEngine.Settings
{
    [System.Serializable]
    public class AudioLowPassFilterSettings
    {
        public bool Foldout = false;
        public bool Enabled = false;
        public float CutoffFrequency = 5007.7f;
        public float LowpassResonanceQ = 1f; 
    }
}
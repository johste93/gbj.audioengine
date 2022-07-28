
namespace GBJ.AudioEngine.Settings
{
    [System.Serializable]
    public class AudioHighPassFilterSettings
    {
        public bool Foldout = false;
        public bool Enabled = false;
        public float CutoffFrequency = 5000f;
        public float HighpassResonanceQ = 1f; 
    }
}

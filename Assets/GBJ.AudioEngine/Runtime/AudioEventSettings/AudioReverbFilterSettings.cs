using UnityEngine;

namespace GBJ.AudioEngine.Settings
{
    [System.Serializable]
    public class AudioReverbFilterSettings
    {
        public bool Foldout = false;
        public AudioReverbPreset ReverbPreset = AudioReverbPreset.Off;
        public float DryLevel = 0f;
        public float Room = 0f;
        public float RoomHF = 0f;
        public float RoomLF = 0f;
        public float DecayTime = 1f;
        public float DecayHFRatio = 0.5f;
        public float ReflectionsLevel = -10000f;
        public float ReflectionsDelay = 0f;
        public float ReverbLevel = 0f;
        public float ReverbDelay = 0.04f;
        public float HfReferance = 5000f;
        public float LfReferance = 250f;
        public float Diffusion = 100f;
        public float Density = 100f;
    }
}

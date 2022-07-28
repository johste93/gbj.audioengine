using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GBJ.AudioEngine.Settings
{
    [System.Serializable]
    public class AudioChorusFilterSettings
    {
        public bool Foldout = false;
        public bool Enabled = false;
        public float DryMix = 0.5f;
        public float WetMix1 = 0.5f;
        public float WetMix2 = 0.5f;
        public float WetMix3 = 0.5f;
        public float Delay = 40;
        public float Rate = 0.8f;
        public float Depth = 0.03f;
    }
}

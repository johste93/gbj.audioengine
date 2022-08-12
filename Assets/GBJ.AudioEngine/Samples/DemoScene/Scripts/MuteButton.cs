using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GBJ.AudioEngine.Samples
{
    public class MuteButton : MonoBehaviour
    {
        [SerializeField] private Text LabelText;
        [SerializeField] private Button Button;
        
        private void Awake()
        {
            LabelText.text = "Mute";
            Button.onClick.AddListener(OnClick);
        }

        private void OnDestroy()
        {
            Button.onClick.RemoveListener(OnClick);
        }

        private void OnClick()
        {
            if (Audio.IsMuted())
            {
                Audio.Unmute();
                LabelText.text = "Mute";
            }
            else
            {
                Audio.Mute();
                LabelText.text = "Unmute";
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using GBJ.AudioEngine;
using UnityEngine;
using UnityEngine.UI;

namespace GBJ.AudioEngine.Samples
{
    public class MusicPlayer : MonoBehaviour
    {
        [SerializeField] private AudioEvent AudioEvent;
        [SerializeField] private Text LabelText;
        [SerializeField] private Button PlayButton;
        [SerializeField] private Text ButtonLabelText;

        private AudioPlayer player;
        
        private void Start()
        {
            player = Audio.Play(AudioEvent);
            LabelText.text = $"{AudioEvent.Name}";
            PlayButton.onClick.AddListener(OnClick);
        }

        private void OnDestroy()
        {
            PlayButton.onClick.RemoveListener(OnClick);
        }

        private void OnClick()
        {
            if (player.IsPlaying())
            {
                player.Pause();
                ButtonLabelText.text = "Resume";
            }
            else
            {
                player.Resume();
                ButtonLabelText.text = "Pause";
            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GBJ.AudioEngine.Samples
{
    public class VolumeSlider : MonoBehaviour
    {
        [SerializeField] private string Tag;
        [SerializeField] private Text LabelText;
        [SerializeField] private Text ValueText;
        [SerializeField] private Slider Slider;

        private void Awake()
        {
            LabelText.text = $"{Tag} Volume:";
            Slider.onValueChanged.AddListener(OnValueChanged);
        }

        private void OnDestroy()
        {
            Slider.onValueChanged.RemoveListener(OnValueChanged);
        }

        private void OnValueChanged(float volume)
        {
            ValueText.text = $"{(volume * 100).ToString("0")}%";
            Audio.SetVolumeByTag(Tag, volume);
        }
    }
}
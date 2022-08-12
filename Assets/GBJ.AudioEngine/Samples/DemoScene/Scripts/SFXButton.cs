using System.Collections;
using System.Collections.Generic;
using GBJ.AudioEngine;
using UnityEngine;
using UnityEngine.UI;

namespace GBJ.AudioEngine.Samples
{
	public class SFXButton : MonoBehaviour
	{
		[SerializeField] private AudioEvent AudioEvent;
		[SerializeField] private Text LabelText;
		[SerializeField] private Button Button;

		private void Awake()
		{
			LabelText.text = $"Test {AudioEvent.Name}";
			Button.onClick.AddListener(OnClick);
		}

		private void OnDestroy()
		{
			Button.onClick.RemoveListener(OnClick);
		}

		private void OnClick()
		{
			Audio.Play(AudioEvent);
		}
	}
}
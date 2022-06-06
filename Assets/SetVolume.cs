using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

namespace IR
{
    public class SetVolume : MonoBehaviour
    {
        public Slider slider;
        public AudioMixer mixer;
        public string volumeControl;

        public void Start()
        {
            slider.onValueChanged.AddListener(delegate { SetLevel(); });
        }
        public void SetLevel()
        {
            mixer.SetFloat(volumeControl, Mathf.Log10(slider.value) * 20);
        }
    }
}
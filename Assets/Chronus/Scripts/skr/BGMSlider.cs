using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BGMSlider : MonoBehaviour
{
    public static int Changebool = 0;
    [SerializeField] Slider BGMVolumer;
    public static float volume;
    [SerializeField] AudioSource audioSource;
    // Start is called before the first frame update
    public void Start()
    {
        audioSource = BGMSingleton.instance.GetAudioSource();
        Changebool = GetBool();
        if(Changebool == 1)
        {
            BGMVolumer.value = GetSliderVolume();
        }
        audioSource.volume = BGMVolumer.value;
        volume = audioSource.volume;
    }

    public void SoundSliderOnValueChange(float newSliderValue)
    {
        Changebool = 1;
        audioSource.volume = newSliderValue;
        volume = audioSource.volume;
    }

    public static float GetSliderVolume()
    {
        return volume;
    }

    public static int GetBool()
    {
        return Changebool;
    }
}

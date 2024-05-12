using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SESlider : MonoBehaviour
{
    public static int Changebool = 0;
    [SerializeField] Slider SEVolumer;
    public static float volume;
    [SerializeField] AudioSource audioSource;
    // Start is called before the first frame update
    public void Start()
    {
        audioSource = SESingleton.instance.GetAudioSource();
        Changebool = GetBool();
        if (Changebool == 1)
        {
            SEVolumer.value = GetSliderVolume();
        }
        audioSource.volume = SEVolumer.value;
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

using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPSliderMove : MonoBehaviour
{
    [SerializeField] Slider slider;

    [SerializeField] int maxHP;
    [SerializeField] float currentHP;
    // Start is called before the first frame update
    void Start()
    {
        slider.maxValue = maxHP;
        currentHP = maxHP;
        slider.value = currentHP;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public enum DamageType
    {
        Sting,
        Fire,
        Ice,
        Elect,
        Other
    }

    public void DamageChecker()
    {
        currentHP -= 10;

        slider.DOValue(currentHP, 0.5f).SetEase(Ease.OutSine);       
    }
}

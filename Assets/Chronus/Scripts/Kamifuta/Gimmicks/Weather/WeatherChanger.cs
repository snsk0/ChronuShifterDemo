using Chronus.ChronusGimmick.Battery;
using Chronus.ChronuShift;
using Chronus.UI.InGame.Interact;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chronus.ChronusGimmick.Weather
{
    public class WeatherChanger : MonoBehaviour, IChronusTarget
    {
        [SerializeField] private BatteryPedestal batteryPedestal;
        [SerializeField] private ParticleSystem generateCloudEfffect;
        [SerializeField] private AudioSource audioSource;

        [SerializeField] private WeatherType nonActiveWeather;
        [SerializeField] private WeatherType activeWeather;

        public WeatherType currentWeather { get; private set; }

        [SerializeField] private bool IsActive;

        private bool IsChangableWeather = true;

        public Action<WeatherType> weatherChangeAction { get; set; }

        private void Awake()
        {
            if (IsActive)
            {
                ChangeWeather(activeWeather);
            }
            else
            {
                ChangeWeather(nonActiveWeather);
            }
        }

        private void Start()
        {
            var height = transform.GetChild(0);
            var main = generateCloudEfffect.main;
            main.startLifetime = height.transform.position.y / 10;
        }

        private void Update()
        {
            if (!IsChangableWeather)
                return;

            if (batteryPedestal.IsSet)
            {
                if (IsActive)
                    return;

                IsActive = true;
                ChangeWeather(activeWeather);
            }
            else
            {
                if (!IsActive)
                    return;

                IsActive = false;
                ChangeWeather(nonActiveWeather);
            }
        }

        public void OnShift(ChronusState state)
        {
            switch (state)
            {
                case ChronusState.Backward:
                    IsChangableWeather = true;
                    if (IsActive)
                    {
                        ChangeWeather(activeWeather);
                    }
                    else
                    {
                        ChangeWeather(nonActiveWeather);
                    }
                    break;
                case ChronusState.Forward:
                    if (currentWeather == activeWeather)
                    {
                        IsChangableWeather = false;
                    }
                    break;
                default:
                    break;
            }
        }

        private void ChangeWeather(WeatherType weatherType)
        {
            currentWeather = weatherType;
            weatherChangeAction?.Invoke(weatherType);

            var emmision = generateCloudEfffect.emission;

            switch (weatherType)
            {
                case WeatherType.Rainy:
                case WeatherType.Snowy:
                    audioSource.Play();
                    emmision.rateOverTime = 10;
                    break;
                case WeatherType.Sunny:
                    audioSource.Stop();
                    emmision.rateOverTime = 1;
                    break;
            }
        }
    }
}


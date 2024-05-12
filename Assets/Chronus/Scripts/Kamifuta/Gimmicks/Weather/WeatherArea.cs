using Kamifuta.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chronus.ChronusGimmick.Weather
{
    public class WeatherArea : MonoBehaviour
    {
        [SerializeField] private WeatherChanger weatherChanger;

        [SerializeField] private ParticleSystem rainEffect;
        [SerializeField] private ParticleSystem rainAroundEffect;
        [SerializeField] private ParticleSystem snowEffect;
        [SerializeField] private ParticleSystem snowAroundEffect;
        [SerializeField] private ParticleSystem cloudEffect;

        [SerializeField] private float areaRadius = 1;
        [SerializeField] private Mesh cilynderMesh;

        private Transform cameraTransform;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            var position = new Vector3(transform.position.x, transform.position.y / 2, transform.position.z);
            GizmosExtensions.DrawWireCylinder(position, areaRadius, transform.position.y);
        }

        private void Awake()
        {
            weatherChanger.weatherChangeAction = ChangeEffect;
        }

        private void Start()
        {
            cameraTransform = Camera.main.transform;

            //エフェクト範囲を調整
            var cloudEffectShape = cloudEffect.shape;
            cloudEffectShape.radius = areaRadius;

            var rainAroundEffectShape = rainAroundEffect.shape;
            var snowAroundEffectShape = snowAroundEffect.shape;
            rainAroundEffectShape.radius = areaRadius;
            snowAroundEffectShape.radius = areaRadius;

            //範囲に合わせてエフェクトの量を調整
            var rainAroundEmmision = rainAroundEffect.emission;
            var snowAroundEmmision = snowAroundEffect.emission;
            rainAroundEmmision.rateOverTime = areaRadius * 20;
            snowAroundEmmision.rateOverTime = areaRadius * 5;
        }

        private void Update()
        {
            if (weatherChanger.currentWeather == WeatherType.Sunny)
                return;

            //カメラとエフェクトの中心の距離を計算
            var groundPoint = new Vector3(transform.position.x, weatherChanger.transform.position.y, transform.position.z);
            var distance = Vector3.Distance(groundPoint, cameraTransform.position);

            if (distance < areaRadius)
            {
                //範囲内にカメラがあればカメラにエフェクトを追従
                var effectPoint = new Vector3(cameraTransform.position.x, transform.position.y, cameraTransform.position.z);
                rainEffect.transform.position = effectPoint;
                snowEffect.transform.position = effectPoint;

                rainAroundEffect.gameObject.SetActive(false);
                snowAroundEffect.gameObject.SetActive(false);

                if(!rainEffect.gameObject.activeSelf && !snowEffect.gameObject.activeSelf)
                {
                    StartCoroutine(SetInitialSimulationSpeed());
                }

                switch (weatherChanger.currentWeather)
                {
                    case WeatherType.Rainy:
                        rainEffect.gameObject.SetActive(true);
                        break;
                    case WeatherType.Snowy:
                        snowEffect.gameObject.SetActive(true);
                        break;
                }
            }
            else
            {
                //範囲外にカメラがあるときエリアの周囲だけエフェクトを実行
                if(!rainAroundEffect.gameObject.activeSelf && !snowAroundEffect.gameObject.activeSelf)
                {
                    StartCoroutine(SetInitialSimulationSpeed());
                }

                switch (weatherChanger.currentWeather)
                {
                    case WeatherType.Rainy:
                        rainAroundEffect.gameObject.SetActive(true);
                        break;
                    case WeatherType.Snowy:
                        snowAroundEffect.gameObject.SetActive(true);
                        break;
                }

                rainEffect.gameObject.SetActive(false);
                snowEffect.gameObject.SetActive(false);
            }
        }

        private void ChangeEffect(WeatherType weatherType)
        {
            switch (weatherChanger.currentWeather)
            {
                case WeatherType.Sunny:
                    rainEffect.gameObject.SetActive(false);
                    snowEffect.gameObject.SetActive(false);
                    cloudEffect.gameObject.SetActive(false);
                    rainAroundEffect.gameObject.SetActive(false);
                    snowAroundEffect.gameObject.SetActive(false);
                    break;
                case WeatherType.Rainy:
                    rainEffect.gameObject.SetActive(true);
                    snowEffect.gameObject.SetActive(false);
                    cloudEffect.gameObject.SetActive(true);
                    break;
                case WeatherType.Snowy:
                    rainEffect.gameObject.SetActive(false);
                    snowEffect.gameObject.SetActive(true);
                    cloudEffect.gameObject.SetActive(true);
                    break;
            }

            StartCoroutine(SetInitialSimulationSpeed());
        }

        private IEnumerator SetInitialSimulationSpeed()
        {
            var rainEffectMain = rainEffect.main;
            var rainAroundEffectMain = rainAroundEffect.main;
            var snowEffectMain = snowEffect.main;
            var snowAroundEffectMain = snowAroundEffect.main;

            rainEffectMain.simulationSpeed = 10f;
            rainAroundEffectMain.simulationSpeed = 10f;
            snowEffectMain.simulationSpeed = 50f;
            snowAroundEffectMain.simulationSpeed = 50f;

            yield return new WaitForSeconds(1f);

            rainEffectMain.simulationSpeed = 1f;
            rainAroundEffectMain.simulationSpeed = 1f;
            snowEffectMain.simulationSpeed = 1f;
            snowAroundEffectMain.simulationSpeed = 1f;
        }
    }
}


using Chronus.ChronusGimmick.Battery;
using Chronus.ChronuShift;
using Chronus.Direction;
using Chronus.UI.InGame.Interact;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Chronus.ChronusGimmick.Weather {
    public class LightningRod : MonoBehaviour, IChronusTarget {

        [SerializeField] private WeatherChanger weatherChanger_obj = null;

        [SerializeField] private BatteryPedestal myAria;

        [SerializeField] GameObject body;
        [SerializeField] Color chargedColor, emptyChargeColor;

        [SerializeField] private PlayableDirector playableDirector;
        [SerializeField] TimelineAsset forward_broken, forward_unBroken, backward_broken, backward_unBroken;

        Renderer myRender = null;
        WeatherType myWeatherType = WeatherType.Rainy, canLightningStrikeWeather = WeatherType.Rainy;

        bool isOnBattery = false;
        Color colorToDisplay = Color.white;

        [SerializeField] bool isLightningStrike = true;
        [SerializeField] GameObject brokenObj;
        [SerializeField] GameObject unbrokenObj;

        void Start() {

            myRender = body.GetComponent<Renderer>();
            if (myRender == null) { Debug.LogError("material is not setted"); return; }
            colorToDisplay = myRender.material.GetColor("_EmissionColor");
            ColorChanger(emptyChargeColor);

            if (playableDirector == null) {
                playableDirector = GetComponent<PlayableDirector>();
                if (playableDirector == null) Debug.LogError("PlaybleDirector is not setted lightning_rod");
            }

            brokenObj.SetActive(isLightningStrike);    
        }

        void Update() {
            if (isOnBattery != myAria.IsSet) {
                if (myAria.IsSet) {
                    ColorChanger(chargedColor);
                }
                else {
                    ColorChanger(emptyChargeColor);
                }
                isOnBattery = myAria.IsSet;
            }
        }

        public void OnShift(ChronusState state) {
            Debug.Log("Shift");
            switch (state) {
                case ChronusState.Forward:
                    CurrentFanc();
                    break;
                case ChronusState.Backward:
                    PastFanc();
                    break;
                case ChronusState.Past:
                    brokenObj.SetActive(false);
                    unbrokenObj.SetActive(false);
                    break;
                case ChronusState.Current:
                    brokenObj.SetActive(isLightningStrike);
                    unbrokenObj.SetActive(!isLightningStrike!);
                    break;
                default:
                    break;
            }
        }

        //過去に行った
        private void PastFanc() {

            if (isLightningStrike) playableDirector.playableAsset = backward_broken;
            else playableDirector.playableAsset = backward_unBroken;

            TimelinePlayer.SetTimeline(playableDirector);
        }


        //現在に行った
        public void CurrentFanc() {

            if (weatherChanger_obj != null) myWeatherType = weatherChanger_obj.currentWeather;
            else Debug.LogAssertion("weatherChanger_obj is not setted");

            //天候が雨でバッテリーが置かれていないときに雷が落ちる
            isLightningStrike = (myWeatherType == canLightningStrikeWeather) && !isOnBattery;

            if (isLightningStrike) playableDirector.playableAsset = forward_broken;
            else playableDirector.playableAsset = forward_unBroken;

            TimelinePlayer.SetTimeline(playableDirector);
        }


        void ColorChanger(Color c) {
            colorToDisplay = c;
            myRender.material.SetColor("_EmissionColor", colorToDisplay);
        }
    }
}
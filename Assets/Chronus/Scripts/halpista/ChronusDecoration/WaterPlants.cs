using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Chronus.ChronuShift;
using System;

namespace Chronus.ChronusDecoration
{
    public class WaterPlants : MonoBehaviour, IChronusTarget
    {
        [SerializeField] private List<SpriteRenderer> plantList;

        private float shiftDuration;

        private void Awake()
        {
            foreach (var plant in plantList)
            {
                plant.color = Color.clear;
                plant.transform.DOMove(Vector3.left * 0.04f, 1f).SetRelative().SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
            }

            shiftDuration = ChronusStateManager.Instance.shiftDuration;
        }

        public void OnShift(ChronusState state)
        {
            if (state == ChronusState.Backward)
            {
                SetPlantsActive(false);
            }
        }

        public void SetPlantsActive(bool enabled)
        {
            foreach(var plant in plantList)
            {
                DOVirtual.Float(plant.color.a, (float)Convert.ToDouble(enabled), shiftDuration, value => plant.color = Color.white * value);
            }
        }
    }
}
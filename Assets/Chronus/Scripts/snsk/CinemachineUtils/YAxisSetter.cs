using UnityEngine;
using Cinemachine;

namespace Chronus.CinemachineUtils
{
    public class YAxisSetter : MonoBehaviour
    {
        [SerializeField] private CinemachineFreeLook _freeLookCamera;
        [SerializeField] private float _yAxisMax;
        [SerializeField] private float _yAxisMin;
        [SerializeField] private float _yAxisDefault;

        private void Awake()
        {
            _freeLookCamera.m_YAxis.m_MaxValue = _yAxisMax;
            _freeLookCamera.m_YAxis.m_MinValue = _yAxisMin;
            _freeLookCamera.m_YAxis.Value = _yAxisDefault;
        }
    }
}

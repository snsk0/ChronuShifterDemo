using UnityEngine;
using Cinemachine;
using Chronus.UI.InGame.ToOut;
using UnityEngine.InputSystem;

namespace Chronus.CinemachineUtils
{
    public class CameraConfigSetter : MonoBehaviour
    {
        private const float BaseMouseMultiply = 0.05f;
        private const float BaseControllerMultiply = 1f;

        [SerializeField] private CinemachineFreeLook _cinemachineFreeLook;
        [SerializeField] private PlayerInput _input;

        private bool _xInverseDefault;
        private bool _yInverseDefault;

        private void Awake()
        {
            _xInverseDefault = _cinemachineFreeLook.m_XAxis.m_InvertInput;
            _yInverseDefault = _cinemachineFreeLook.m_YAxis.m_InvertInput;

            SetSensitivityMultiply(UIStateRecorder.Instance);
        }

        private void Update()
        {
            SetSensitivityMultiply(UIStateRecorder.Instance);
        }

        private void SetSensitivityMultiply(UIStateRecorder instance)
        {
            float baseMultiply = _input.currentControlScheme == "GamePad" ? BaseControllerMultiply : BaseMouseMultiply;

            _cinemachineFreeLook.m_XAxis.m_MaxSpeed = baseMultiply * instance.sensitivity;
            _cinemachineFreeLook.m_YAxis.m_MaxSpeed = baseMultiply * instance.sensitivity;

            _cinemachineFreeLook.m_XAxis.m_InvertInput = instance.isRevers_x ? !_xInverseDefault : _xInverseDefault;
            _cinemachineFreeLook.m_YAxis.m_InvertInput = instance.isRevers_y ? !_yInverseDefault : _yInverseDefault;
        }
    }
}

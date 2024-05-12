using Chronus.ChronuShift;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

public class SkyboxRotate : MonoBehaviour
{
    [Header("回転スピード")]
    [SerializeField] float RotateSpeed = 0.5f;
    private Material SkyboxMaterial;

    private float _transform = 0;
    private float _currentSpeed = 0;
    [SerializeField] private float _acceleration;
    // Start is called before the first frame update
    void Start()
    {
        SkyboxMaterial = RenderSettings.skybox;

        _currentSpeed = RotateSpeed;

        ChronusStateManager.Instance.chronusState.Subscribe(async state =>
        {
            if (state == ChronusState.Forward)
            {
                while(_currentSpeed < RotateSpeed)
                {
                    _currentSpeed += _acceleration * Time.deltaTime;
                    await UniTask.Delay(1);
                }
                _currentSpeed = RotateSpeed;
            }
            else if (state == ChronusState.Backward)
            {
                while (_currentSpeed > -RotateSpeed)
                {
                    _currentSpeed -= _acceleration * Time.deltaTime;
                    await UniTask.Delay(1);
                }
                _currentSpeed = -RotateSpeed;
            }
        }).AddTo(this);
    }

    private void Update()
    {
        _transform += _currentSpeed * Time.deltaTime;

        SkyboxMaterial.SetFloat("_CloudSpeed", _transform);
    }
}

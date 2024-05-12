using UnityEngine;
using Chronus.ChronusItem;
using UnityView.Player.Utils;
using Cysharp.Threading.Tasks;

public class HandableObject : MonoBehaviour, IChronusItemController
{
    private const float _scaleOffset = 0.1f; 

    [SerializeField] private float _carriedScaleMagnitude;
    [SerializeField] private float _dropYOffset;
    [SerializeField] private Vector3 _rotationOffSet;

    private Collider _collider;
    private IChronusItem _chronusItem;

    private Vector3 _defaultScale;

    public float DropYOffset => _dropYOffset;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _chronusItem = GetComponent<IChronusItem>();

        _defaultScale = transform.localScale;
    }

    //通知関数
    public void OnHandle(bool isPickUp)
    {
        _chronusItem.ToggleCarriedState(isPickUp);

        if(isPickUp)
        {
            _collider.enabled = false;
        }
        else
        {
            _collider.enabled = true;
        }
    }

    public async UniTask CallItemAnimation(bool isPickUp)
    {
        if (isPickUp)
        {
            while (transform.localScale.magnitude > _carriedScaleMagnitude)
            {
                transform.localScale *= (1 - _scaleOffset);
                await UniTask.Delay(1);
            }

            transform.localScale = _defaultScale * _carriedScaleMagnitude;
        }
        else
        {
            transform.Rotate(_rotationOffSet);

            while (transform.localScale.magnitude < _defaultScale.magnitude)
            {
                transform.localScale *= (1 + _scaleOffset);
                await UniTask.Delay(1);
            }

            transform.localScale = _defaultScale;
        }
    }
}

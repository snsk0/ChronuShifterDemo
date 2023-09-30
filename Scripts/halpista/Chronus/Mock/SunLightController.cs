using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class SunLightController : MonoBehaviour
{
    [SerializeField] Vector3 minRotation;
    [SerializeField] Vector3 maxRotation;
    [SerializeField] float speed;

    void Start()
    {
        transform.rotation = Quaternion.Euler(minRotation);
        transform.DORotate(maxRotation - minRotation, speed, RotateMode.LocalAxisAdd).SetLoops(-1, LoopType.Yoyo);
    }
}

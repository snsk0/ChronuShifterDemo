using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using static UnityEngine.Rendering.DebugUI;
using Chronus.ChronuShift;

public class TitleImage : MonoBehaviour
{
    [SerializeField] GameObject TImage;
    [SerializeField] private ChronusStateManager _chrousManager;
    [SerializeField] private float _rotateSpan;
    [SerializeField] private float _rotateDuration;
    //float spant;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 eulerangle = transform.eulerAngles;
        eulerangle.z = 0;
        transform.eulerAngles = eulerangle;
        TImage.transform.Rotate(0, 0, 0);
        //spant = 0;
        StartCoroutine("Anim");
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator Anim()
    {
        while (true)
        {
            yield return new WaitForSeconds(_rotateSpan);
            TImage.transform.DOLocalRotate(new Vector3(0, 0, 180f), _rotateDuration).SetEase(Ease.OutQuad);

            _chrousManager.ChronuShift();

            yield return new WaitForSeconds(_rotateSpan);
            TImage.transform.DOLocalRotate(new Vector3(0, 0, 360f), _rotateDuration).SetEase(Ease.OutQuad);

            _chrousManager.ChronuShift();
        }
    }
}

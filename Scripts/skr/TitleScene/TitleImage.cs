using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using static UnityEngine.Rendering.DebugUI;

public class TitleImage : MonoBehaviour
{
    [SerializeField] GameObject TImage;
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
            yield return new WaitForSeconds(5f);
            TImage.transform.DOLocalRotate(new Vector3(0, 0, 180f), 1f).SetEase(Ease.OutQuad);
            yield return new WaitForSeconds(5f);
            TImage.transform.DOLocalRotate(new Vector3(0, 0, 360f), 1f).SetEase(Ease.OutQuad);
        }
    }
}

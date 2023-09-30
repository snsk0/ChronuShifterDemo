using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class IntractCheck : MonoBehaviour
{
    [SerializeField] Image pict;
    [SerializeField] Sprite pict1;//現在の箱
    [SerializeField] Sprite pict2;//過去の箱
    [SerializeField] Sprite pict3;//鍵？
    [SerializeField] Sprite pict4;//なんか
    [SerializeField] Sprite pict5;//あれやこれや
    // Start is called before the first frame update
    void Start()
    {
        pict.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public enum ItemType
    {
        Current,
        Past,
        Key,
        NONE //アイテム持ってない
    }

    public void IntractChecker(ItemType Type)//何のアイテム持ってる？
    {
        switch (Type)
        {
            case ItemType.Current:
                pict.sprite = pict1;
                pict.enabled = true;
                break;
            case ItemType.Past:
                pict.sprite = pict2;
                pict.enabled = true;
                break;
            case ItemType.Key:
                pict.sprite = pict3;
                pict.enabled = true;
                break;
            case ItemType.NONE:
                pict.enabled = false;
                break;
        }
    }
}

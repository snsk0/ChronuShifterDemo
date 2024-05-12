using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class IntractCheck : MonoBehaviour
{
    [SerializeField] Image pict;
    [SerializeField] Sprite pict1;//���݂̔�
    [SerializeField] Sprite pict2;//�ߋ��̔�
    [SerializeField] Sprite pict3;//���H
    [SerializeField] Sprite pict4;//�Ȃ�
    [SerializeField] Sprite pict5;//����₱���
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
        NONE //�A�C�e�������ĂȂ�
    }

    public void IntractChecker(ItemType Type)//���̃A�C�e�������Ă�H
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

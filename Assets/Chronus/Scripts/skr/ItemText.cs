using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class ItemText : MonoBehaviour
{
    [SerializeField] GameObject itemtext;
    // Start is called before the first frame update
    void Start()
    {
        
        itemtext.GetComponent<Text>().text = "";
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}

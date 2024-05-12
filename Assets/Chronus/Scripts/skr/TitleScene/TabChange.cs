using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabChange : MonoBehaviour
{
    [SerializeField] GameObject tab1;
    [SerializeField] GameObject tab2;
    private SESingleton SE;
    // Start is called before the first frame update
    void Start()
    {
        tab1.SetActive(true);
        tab2.SetActive(false);
        SE = GameObject.Find("SEManager").GetComponent<SESingleton>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClickTab1()
    {
        SE.ClickSE();
        tab1.SetActive(true);
        tab2.SetActive(false);
    }

    public void ClickTab2()
    {
        SE.ClickSE();
        tab1.SetActive(false);
        tab2.SetActive(true);
    }
}

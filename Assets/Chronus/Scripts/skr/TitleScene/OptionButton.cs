using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionButton : MonoBehaviour
{
    private SESingleton SE;
    // Start is called before the first frame update
    void Start()
    {
        SE = GameObject.Find("SEManager").GetComponent<SESingleton>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BackTitle()
    {
        SE.ClickSE();
        FadeManager.Instance.LoadScene("SKR_TestTitleUI", 0.5f);
    }
}

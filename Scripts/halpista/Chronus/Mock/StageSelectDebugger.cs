using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectDebugger : MonoBehaviour
{
    [SerializeField] GameObject debugButton;
    bool isActive = false;

    void Start()
    {
        if(debugButton == null) Destroy(this);
        debugButton.SetActive(isActive);
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.S))
        {
            isActive = !isActive;
            debugButton.SetActive(isActive);
        }        
    }
}
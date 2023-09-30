using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class StageSelecter : MonoBehaviour
{
    [SerializeField] EventSystem eventSystem;
    [SerializeField] GameObject defaultSelected;

    private SESingleton SE;

    void Start()
    {
        GameObject seManager = GameObject.Find("SEManager");
        if(seManager != null)
        {
            SE = seManager.GetComponent<SESingleton>();
        }
    }

    void Update()
    {
        /*if(Input.GetMouseButtonDown(0))
        {
            eventSystem.SetSelectedGameObject(defaultSelected);
        }*/
    }

    public void SelectStage(string text)
    {
        if(SE != null)
        {
            SE.ClickSE();
        }

        if(FadeManager.Instance == null)
        {
            SceneManager.LoadScene(text);
        }
        else
        {
            FadeManager.Instance.LoadScene(text, 0.5f);
        }
    }
}
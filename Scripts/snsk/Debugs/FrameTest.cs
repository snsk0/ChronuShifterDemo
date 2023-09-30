using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameTest : MonoBehaviour
{
    private void Awake()
    {
        Debug.Log("Awake: " + Time.frameCount);
    }
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start: " + Time.frameCount);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Update: " + Time.frameCount);
    }
}

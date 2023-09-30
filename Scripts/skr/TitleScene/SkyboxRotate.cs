using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxRotate : MonoBehaviour
{
    [Header("回転スピード")]
    [SerializeField] float RotateSpeed = 0.5f;
    private Material SkyboxMaterial;
    // Start is called before the first frame update
    void Start()
    {
        SkyboxMaterial = RenderSettings.skybox;
    }

    // Update is called once per frame
    void Update()
    {
        SkyboxMaterial.SetFloat("_Rotation", Mathf.Repeat(SkyboxMaterial.GetFloat("_Rotation") + RotateSpeed * Time.deltaTime, 360f));
    }
}

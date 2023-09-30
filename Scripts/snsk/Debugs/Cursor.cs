using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Cursor : MonoBehaviour
{
    [SerializeField] private bool cursol;
    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.Cursor.visible = cursol;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

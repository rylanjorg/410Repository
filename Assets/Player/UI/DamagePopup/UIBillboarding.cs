using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBillboarding : MonoBehaviour
{
    private Camera cam;
    void Awake()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        transform.forward = cam.transform.forward;
    }
}

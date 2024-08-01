using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondOrderDemoPosition : MonoBehaviour
{
    public UpdateMode updateMode;
    public float f = 5;
    public float z = 1.0f;
    public float r = 0.0f;

    public Transform target;

    private SecondOrderDynamics secondOrderDynamics;

    // Start is called before the first frame update
    void Awake()
    {
        secondOrderDynamics = new SecondOrderDynamics(f, z, r, transform.position);
    }

    public void RecalculateConstants()
    {
        secondOrderDynamics.SetConstants(f, z, r);
    }

    // Update is called once per frame
    void Update()
    {
        if(updateMode == UpdateMode.Update)
        {
            transform.position = secondOrderDynamics.Update(Time.fixedDeltaTime, target.position, transform.position);
        }
    }

    void FixedUpdate()
    {
        if(updateMode == UpdateMode.FixedUpdate)
        {
            transform.position = secondOrderDynamics.Update(Time.fixedDeltaTime, target.position, transform.position);
        }
    }
}

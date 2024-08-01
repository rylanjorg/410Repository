using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondOrderDemoVector1 : MonoBehaviour
{
    public UpdateMode updateMode;
    public float f = 5;
    public float z = 1.0f;
    public float r = 0.0f;

    public Transform target;

    private SecondOrderDynamicsFloat secondOrderDynamics;

    // Start is called before the first frame update
    void Awake()
    {
        secondOrderDynamics = new  SecondOrderDynamicsFloat(f, z, r, transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        if(updateMode == UpdateMode.Update)
        {
            Vector3 pos = transform.position;
            pos.y = secondOrderDynamics.Update(Time.fixedDeltaTime, target.position.y, transform.position.y);
            transform.position = pos;
        }
    }

    void FixedUpdate()
    {
        if(updateMode == UpdateMode.FixedUpdate)
        {
            Vector3 pos = transform.position;
            pos.y = secondOrderDynamics.Update(Time.fixedDeltaTime, target.position.y, transform.position.y);
            transform.position = pos;
        }
    }
}

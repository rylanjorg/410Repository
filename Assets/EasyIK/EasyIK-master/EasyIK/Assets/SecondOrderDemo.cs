using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;

public enum UpdateMode
{
    Update,
    FixedUpdate,
    CustomUpdate
}

public class SecondOrderDemo : MonoBehaviour
{

    public UpdateMode updateMode;
    public float f = 5;
    public float z = 1.0f;
    public float r = 0.0f;

    public Transform target;

    private SecondOrderDynamics secondOrderDynamics;
    public List<LeanTowardsTarget> leanComponent = new List<LeanTowardsTarget>();

    // Start is called before the first frame update
    void Awake()
    {
        secondOrderDynamics = new SecondOrderDynamics(f, z, r, transform.position);
        
    }

    // Update is called once per frame
    void Update()
    {
        if(updateMode == UpdateMode.Update)
        {
      
           
            //LeanAndReset();
            foreach (var component in leanComponent)
            {
                if(component.currentState == LeanTowardsTarget.State.Active)
                {
                    transform.position = secondOrderDynamics.Update(Time.deltaTime, target.position, transform.position);
                    component.LeanAndReset(target);
                }
                else if(component.currentState == LeanTowardsTarget.State.Reset)
                {
                    transform.position = secondOrderDynamics.Update(Time.deltaTime, component.resetTarget.position, transform.position);
                    component.LeanAndReset(component.resetTarget);
                }
                
            }
        }
    }

    void FixedUpdate()
    {
        if(updateMode == UpdateMode.FixedUpdate)
        {
            //LeanAndReset();
            foreach (var component in leanComponent)
            {
                if(component.currentState == LeanTowardsTarget.State.Active)
                {

                    transform.position = secondOrderDynamics.Update(Time.fixedDeltaTime, target.position, transform.position);
                    component.LeanAndReset(target);
                }
                else if(component.currentState == LeanTowardsTarget.State.Reset)
                {
                    transform.position = secondOrderDynamics.Update(Time.fixedDeltaTime, component.resetTarget.position, transform.position);
                    component.LeanAndReset(component.resetTarget);
                }
                
            }
        }
    }
}

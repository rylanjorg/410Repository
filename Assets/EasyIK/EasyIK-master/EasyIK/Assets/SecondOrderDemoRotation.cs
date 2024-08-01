using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;

public class SecondOrderDemoRotation : MonoBehaviour
{
    public enum axis{
        x,
        y,
        z
    }

     public enum RelativeRotation{
        initalrotation,
        anothertransformrotation,
    }


    public Quaternion rootTransformRotation;
    public Transform rootTransform;

    public UpdateMode updateMode;
    public float f = 5;
    public float z = 1.0f;
    public float r = 0.0f;
    public bool doRotate = true;
    public Transform target;
    private SecondOrderDynamicsFloat secondOrderDynamics;
    private Quaternion targetRotation;
    public float angleOffset = 0f; // Add this line to declare the angle offset
    public float minAngle = -180f;
    public float maxAngle = 180f;
    public axis axisToRotate = axis.y;
    public RelativeRotation relativeRotationMode = RelativeRotation.initalrotation;
    public bool flipRotationDirection = false;
    [SerializeField] [ReadOnly] public float angle;

    
    [SerializeField] [ReadOnly] float newYRotation;

    [Title("last Rotation:")]
    [SerializeField] [ReadOnly] private float lastRotation = -1f;
    
    [Title("this Rotation:")]

    [SerializeField] [ReadOnly] float thisCurrentRotation = -1f;
    [SerializeField] [ReadOnly] private float thisTargetRotation = -1f;
    private Quaternion initialRotation;



    // Start is called before the first frame update
    void Awake()
    {
        initialRotation = transform.localRotation;

        if(axisToRotate == axis.x)
        {
            lastRotation = transform.eulerAngles.x;
            thisTargetRotation = transform.eulerAngles.x;
            thisCurrentRotation = transform.eulerAngles.x;
            secondOrderDynamics = new SecondOrderDynamicsFloat(f, z, r, thisCurrentRotation);
        }
        else if(axisToRotate == axis.y)
        {
            lastRotation = transform.eulerAngles.y;
            thisTargetRotation = transform.eulerAngles.y;
            thisCurrentRotation = transform.eulerAngles.y;
            secondOrderDynamics = new SecondOrderDynamicsFloat(f, z, r, thisCurrentRotation);
        }
        else if(axisToRotate == axis.z)
        {
            lastRotation = transform.eulerAngles.z;
            thisTargetRotation = transform.eulerAngles.z;
            thisCurrentRotation = transform.eulerAngles.z;
            secondOrderDynamics = new SecondOrderDynamicsFloat(f, z, r, thisCurrentRotation);
        }
    }

    // Update is called once per frame
    // Update is called once per frame
    void Update()
    {


        if(!doRotate) return;

        rootTransformRotation = rootTransform.localRotation;
        if (updateMode == UpdateMode.Update)
        {
            GetCurrentTargetRotation();
            Rotate();
            DrawRay();
        }

       
    }

    void FixedUpdate()
    {
        if(!doRotate) return;

        rootTransformRotation = rootTransform.localRotation;
        if (updateMode == UpdateMode.FixedUpdate)
        {
            GetCurrentTargetRotation();
            
            Rotate();
            DrawRay();
        }

        
    }


    void GetCurrentTargetRotation()
    {
        if(axisToRotate == axis.x)
        {
            Vector3 relative = (target.position-this.transform.position); //transform.InverseTransformPoint(target.position);
            angle = Mathf.Atan2(relative.y, relative.z) * Mathf.Rad2Deg;//  - rootTransformRotation.eulerAngles.x;
        }
        else if(axisToRotate == axis.y)
        {
            Vector3 relative = (target.position-this.transform.position); //transform.InverseTransformPoint(target.position);
            angle = Mathf.Atan2(relative.x, relative.z) * Mathf.Rad2Deg;//  - rootTransformRotation.eulerAngles.y;
        }
        else if(axisToRotate == axis.z)
        {
            Vector3 relative = (target.position-this.transform.position); //transform.InverseTransformPoint(target.position);
            angle = Mathf.Atan2(relative.x, relative.y) * Mathf.Rad2Deg;// - rootTransformRotation.eulerAngles.z;
        }
   
    }

    void Rotate()
    {
        thisTargetRotation = Mathf.Clamp(angle, minAngle ,maxAngle);
       
        if(axisToRotate == axis.x)
        {
            thisCurrentRotation = transform.eulerAngles.x;
        }
        else if(axisToRotate == axis.y)
        {
            thisCurrentRotation = transform.eulerAngles.y;
        }
        else if(axisToRotate == axis.z)
        {
            thisCurrentRotation = transform.eulerAngles.z;
        }

        while(thisTargetRotation < lastRotation - 180f)
        {
            thisTargetRotation += 360f;
        }
        while(thisTargetRotation > lastRotation + 180f)
        {
            thisTargetRotation -= 360f;
        }

        while(thisCurrentRotation < lastRotation - 180f)
        {
            thisCurrentRotation += 360f;
        }
        while(thisCurrentRotation > lastRotation + 180f)
        {
            thisCurrentRotation -= 360f;
        }


        if(updateMode == UpdateMode.FixedUpdate) newYRotation = secondOrderDynamics.Update(Time.fixedDeltaTime, thisTargetRotation,  thisCurrentRotation);
        else if (updateMode == UpdateMode.Update) newYRotation = secondOrderDynamics.Update(Time.deltaTime, thisTargetRotation,  thisCurrentRotation);
    

       
        lastRotation = newYRotation;
        

    
        CalculateRelativeRotation();
    }       

    // Helper method to normalize angles to 0-360 range
    float NormalizeAngle(float angle)
    {
        return (angle + 360) % 360;
    }


    void CalculateRelativeRotation()
    {
        Quaternion targetRotation = Quaternion.identity;

        if(axisToRotate == axis.x)
        {
            if(flipRotationDirection)
            {
                targetRotation = Quaternion.AngleAxis(newYRotation + angleOffset, Vector3.left);
            }
            else
            {
                targetRotation = Quaternion.AngleAxis(newYRotation + angleOffset, Vector3.right);
            }  
        }
        else if(axisToRotate == axis.y)
        {
            if(flipRotationDirection)
            {
                targetRotation = Quaternion.AngleAxis(newYRotation + angleOffset, Vector3.down);
            }
            else
            {
                targetRotation = Quaternion.AngleAxis(newYRotation + angleOffset, Vector3.up);
            }

          
        }
        else if(axisToRotate == axis.z)
        {
            if(flipRotationDirection)
            {
                targetRotation = Quaternion.AngleAxis(newYRotation + angleOffset, Vector3.back);
            }
            else
            {
                targetRotation = Quaternion.AngleAxis(newYRotation + angleOffset, Vector3.forward);
            }
        }  

        SetRelativeRotation(targetRotation);
    }
    
    void SetRelativeRotation(Quaternion targetRotation)
    {
        if(relativeRotationMode  == RelativeRotation.initalrotation)
        {
            Quaternion relativeRotation = Quaternion.Inverse(initialRotation) * targetRotation;
            transform.localRotation = initialRotation * relativeRotation;
        }
        else if(relativeRotationMode  == RelativeRotation.anothertransformrotation)
        {
            Quaternion relativeRotation = Quaternion.Inverse(rootTransformRotation) * targetRotation;
            transform.localRotation = rootTransformRotation * relativeRotation;
        }
    }


    void DrawRay()
    {
        //Debug.DrawRay(transform.position, transform.forward * 5.0f, Color.white);
        //Debug.DrawRay(transform.position, transform.forward * 5.0f, Color.white);
        //Debug.DrawRay(transform.position, target.position-this.transform.position, Color.red);
    }

}

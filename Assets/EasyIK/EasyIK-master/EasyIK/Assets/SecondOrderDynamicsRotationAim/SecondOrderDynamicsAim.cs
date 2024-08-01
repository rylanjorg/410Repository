using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;

public class SecondOrderDemoAim : MonoBehaviour
{
    public enum axis{
        x,
        y,
        z
    }

     public enum RelativeRotation{
        InitalRotation,
        AnotherTransformRotation,
    }

    [TabGroup("fsm", "Inscribed", TextColor = "green")] public UpdateMode updateMode;
    [TabGroup("fsm", "Inscribed")] public RelativeRotation relativeRotationMode = RelativeRotation.InitalRotation;
    [TabGroup("fsm", "Inscribed")] public axis axisToRotate = axis.y;
    [TabGroup("fsm", "Inscribed")] public float minAngle = -180f;
    [TabGroup("fsm", "Inscribed")] public float maxAngle = 180f;
    [TabGroup("fsm", "Inscribed")] public float angleOffset = 0f;
    [TabGroup("fsm", "Inscribed")] public bool flipRotationDirection = false;
    [TabGroup("fsm", "Inscribed")] public PlayerWeaponContainer playerWeaponContainer;  


    [TabGroup("fsm", "SecondOrderDynamics", TextColor = "orange")] public float f = 5;
    [TabGroup("fsm", "SecondOrderDynamics")] public float z = 1.0f;
    [TabGroup("fsm", "SecondOrderDynamics")] public float r = 0.0f;
    [TabGroup("fsm", "SecondOrderDynamics")] private SecondOrderDynamicsFloat thisSecondOrderDynamics;
    [TabGroup("fsm", "SecondOrderDynamics")] public SecondOrderDemoRotation playerForwardSecondOrderDynamics;
    public Quaternion rootTransformRotation;
    public Transform playerForwardVector;

    


    public Transform target;
    
    private Quaternion targetRotation;
     // Add this line to declare the angle offset

   
   
    [TabGroup("fsm", "Dynamic", TextColor = "blue")] [SerializeField] [ReadOnly] public float angle;
    [TabGroup("fsm", "Dynamic")] [SerializeField] [ReadOnly] float relativeAngle;
    [TabGroup("fsm", "Dynamic")] [SerializeField] [ReadOnly] float currentPForwardAngle;
    [TabGroup("fsm", "Dynamic")] [SerializeField] [ReadOnly] public float leftAngleMax;
    [TabGroup("fsm", "Dynamic")] [SerializeField] [ReadOnly] public float rightAngleMax;




    
    [TabGroup("fsm", "Dynamic")] [SerializeField] [ReadOnly] float newYRotation;


    [Title("last Rotation:")]
    [TabGroup("fsm", "Dynamic")] [SerializeField] [ReadOnly] private float lastRotation = -1f;
    
    [Title("this Rotation:")]

    [TabGroup("fsm", "Dynamic")] [SerializeField] [ReadOnly] float thisCurrentRotation = -1f;
    [TabGroup("fsm", "Dynamic")] [SerializeField] [ReadOnly] private float thisTargetRotation = -1f;
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
            thisSecondOrderDynamics = new SecondOrderDynamicsFloat(f, z, r, thisCurrentRotation);
        }
        else if(axisToRotate == axis.y)
        {
            lastRotation = transform.eulerAngles.y;
            thisTargetRotation = transform.eulerAngles.y;
            thisCurrentRotation = transform.eulerAngles.y;
            thisSecondOrderDynamics = new SecondOrderDynamicsFloat(f, z, r, thisCurrentRotation);
        }
        else if(axisToRotate == axis.z)
        {
            lastRotation = transform.eulerAngles.z;
            thisTargetRotation = transform.eulerAngles.z;
            thisCurrentRotation = transform.eulerAngles.z;
            thisSecondOrderDynamics = new SecondOrderDynamicsFloat(f, z, r, thisCurrentRotation);
        }




    }

    // Update is called once per frame
    // Update is called once per frame
    void Update()
    {
        if(!playerWeaponContainer.isWeaponEquipped) return;
    

        rootTransformRotation = playerForwardVector.localRotation;
        if (updateMode == UpdateMode.Update)
        {
            GetCurrentTargetRotation();
            RotateGun();
            CalculateRelativeRotation();
            //DrawRay();
        }

       
    }

    void FixedUpdate()
    {
         if(!playerWeaponContainer.isWeaponEquipped) return;
        rootTransformRotation = playerForwardVector.localRotation;
        if (updateMode == UpdateMode.FixedUpdate)
        {
            
            GetCurrentTargetRotation();
            RotateGun();
            CalculateRelativeRotation();
            //DrawRay();
        }

        
    }


    void GetCurrentTargetRotation()
    {
        // Angle relative from the current position of the gun to the target position
        Vector3 relative = (target.position-this.transform.position); 
        angle = Mathf.Atan2(relative.x, relative.z) * Mathf.Rad2Deg;

        // Get the current forward angle of the player
        currentPForwardAngle = playerForwardVector.eulerAngles.y;
        leftAngleMax = currentPForwardAngle + minAngle;
        rightAngleMax = currentPForwardAngle + maxAngle;

        // The current angle of the gun then is the gun - the player forward angle
        relativeAngle = angle - currentPForwardAngle;

        // might need to normalize it for transition between 0 and 180 and -180 and 0
        relativeAngle = NormalizeAngle_(relativeAngle, angle);
    }
    
    float NormalizeAngle_(float a, float b)
    {
        while(a < b - 180f)
        {
            a += 360f;
        }
        while(a> b + 180f)
        {
            a -= 360f;
        }

        while(a < b - 180f)
        {
            a += 360f;
        }
        while(a > b + 180f)
        {
            a -= 360f;
        }

        return a;
    }


    void RotateGun()
    {
        thisTargetRotation = angle;

        if(relativeAngle < minAngle)
        {
           playerForwardVector.rotation = Quaternion.AngleAxis(currentPForwardAngle + (-minAngle + maxAngle), Vector3.up);
            //currentPForwardAngle = playerForwardVector.eulerAngles.y;
        }

        if( relativeAngle > maxAngle)
        {
            playerForwardVector.rotation = Quaternion.AngleAxis(currentPForwardAngle + (-minAngle + maxAngle), Vector3.up);
             //currentPForwardAngle = playerForwardVector.eulerAngles.y;
        }
    
        thisCurrentRotation =  NormalizeAngle_(transform.eulerAngles.y, lastRotation);
        thisTargetRotation =  NormalizeAngle_(relativeAngle, lastRotation);

        if(updateMode == UpdateMode.FixedUpdate) newYRotation = thisSecondOrderDynamics.Update(Time.fixedDeltaTime, thisTargetRotation,  thisCurrentRotation);
        else if (updateMode == UpdateMode.Update) newYRotation = thisSecondOrderDynamics.Update(Time.deltaTime, thisTargetRotation,  thisCurrentRotation);

        lastRotation = newYRotation;    
    }



    void CalculateRelativeRotation()
    {
        Quaternion targetRotation = Quaternion.identity;

        if(flipRotationDirection)
        {
            targetRotation = Quaternion.AngleAxis(newYRotation + angleOffset, Vector3.down);
        }
        else
        {
            targetRotation = Quaternion.AngleAxis(newYRotation + angleOffset, Vector3.up);
        }

        SetRelativeRotation(targetRotation);
    }
    
    void SetRelativeRotation(Quaternion targetRotation)
    {
        if(relativeRotationMode  == RelativeRotation.InitalRotation)
        {
            Quaternion relativeRotation = Quaternion.Inverse(initialRotation) * targetRotation;
            transform.localRotation = initialRotation * relativeRotation;
        }
        else if(relativeRotationMode  == RelativeRotation.AnotherTransformRotation)
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

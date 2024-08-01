using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBasedOnSOD : MonoBehaviour
{
    public SecondOrderDemoRotation sod;
    public SecondOrderDemoRotation bodyRootSOD;
    public PlayerWeaponContainer playerWeaponContainer;
    public PlayerDataManagement playerDataManagement;
    public bool doTryRotate = false;
    public float rotateTrueTime = 0.5f;
    private bool coroutineRunning = false;
    public void Update()
    {
        doTryRotate = playerWeaponContainer.isWeaponEquipped ? true : false;


        if(!doTryRotate)
        {
            return;
            
        }

        //if(playerDataManagement.currentState == FSMMovement)


        if(sod.angle >= sod.maxAngle)
        {
            //sod.angle = sod.maxAngle;
            Debug.Log("Turn right");
            if(coroutineRunning == false) StartCoroutine(RotatePlayer());
        }
        else if(sod.angle <= sod.minAngle)
        {
            //sod.angle = sod.minAngle;
            Debug.Log("Turn left");
            if(coroutineRunning == false) StartCoroutine(RotatePlayer());
        }
        else
        {
            //bodyRootSOD.doRotate = false;
        }
    }

    private IEnumerator RotatePlayer()
    {
        coroutineRunning = true;
        bodyRootSOD.doRotate = true;
        yield return new WaitForSeconds(rotateTrueTime);
        bodyRootSOD.doRotate = false;
        coroutineRunning = false;
    }


}

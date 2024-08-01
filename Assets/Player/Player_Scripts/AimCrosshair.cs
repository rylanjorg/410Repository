using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AimCrosshair : MonoBehaviour
{
    private enum ShootType
    {
        FromGun,
        FromCamera,
    }

    [SerializeField] private ShootType shootType;
    [SerializeField] private Image Crosshair;
    [SerializeField] private new Camera camera;
    [SerializeField] private GameObject AimTarget;
    [SerializeField] private GameObject CrosshairPlane;
    [SerializeField] public Transform gunTipTransform;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] float offsetDistance = 1.0f; // Change this to the desired offset distance
    [SerializeField] Vector3 cameraOffset = new Vector3(0, 0, 0);


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Align the object's rotation with the camera's forward vector
        CrosshairPlane.transform.rotation = Quaternion.LookRotation(camera.transform.forward, camera.transform.up);

        //UpdateCrosshair();
    }

    private Vector3 GetRaycastOrigin()
    {
        
        return gunTipTransform.position;
    }

    private Vector3 GetGunForward()
    {
        return transform.forward;
    }

    private void WorldToScreenPoint(Vector3 hitPoint)
    {

    }

    private void UpdateCrosshair()
    {
        Vector3 gunTipPoint = GetRaycastOrigin();
        Vector3 forward;

        if(shootType == ShootType.FromGun)
        {
            forward = GetGunForward();
        }
        else
        {
           forward = camera.transform.forward + cameraOffset;
        }


        Vector3 hitPoint = gunTipPoint + forward * 10;

        if(Physics.Raycast(gunTipPoint, forward, out RaycastHit hit, float.MaxValue, layerMask))
        {
           hitPoint = hit.point;
        }

        // Project hitPoint onto the camera plane
        Vector3 toPoint = hitPoint - camera.transform.position;
        float dotProduct = Vector3.Dot(toPoint, camera.transform.forward);
        Vector3 projection = hitPoint - camera.transform.forward * dotProduct;

        // Add an offset to the projection

        Vector3 offsetProjection = projection + camera.transform.forward * offsetDistance;

        AimTarget.transform.position = offsetProjection;
        
        /*
        if(shootType == ShootType.FromGun)
        {
            Vector3 gunTipPoint = GetRaycastOrigin();
            Vector3 forward = GetGunForward();

            Vector3 hitPoint = gunTipPoint + forward * 10;
            if(Physics.RayCast(gunTipPoint, forward, out RaycastHit hit, float.MaxValue))
            {
                hitPoint = hit.point;
            }
            Vector3 screenSpaceLocation = WorldToScreenPoint(hitPoint);


            if(RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, screenSpaceLocation, null, out Vector2 localPoint))
            {
                crosshair.transform.localPosition = localPoint;
            }
            else
            {
                crosshair.transform.localPosition = Vector2.zero;//new Vector3(Screen.width / 2, Screen.height / 2, 0);
            }
        }*/
        
    }
}

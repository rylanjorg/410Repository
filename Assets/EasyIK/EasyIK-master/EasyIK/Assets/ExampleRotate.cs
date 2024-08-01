// Usually you use transform.LookAt for this.
// But this can give you more control over the angle

using UnityEngine;
using System.Collections;

public class ExampleRotate : MonoBehaviour
{
    public Transform target;
    public float angleX;
    public float angleZ;

    void Update()
    {
        Vector3 relative = transform.InverseTransformPoint(target.position);
        angleX = Mathf.Atan2(relative.y, relative.z) * Mathf.Rad2Deg;
        angleZ = Mathf.Atan2(relative.x, relative.z) * Mathf.Rad2Deg;
        transform.Rotate(angleX, this.transform.rotation.y, angleZ);
    }
}
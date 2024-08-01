using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondOrderDynamicsFloat
{
     private const float PI = Mathf.PI;
    private float xp;
    private float y, yd;
    private float k1,k2,k3;
    private float T_crit;

    public void SetConstants(float f,float z,float r)
    {
        //compute constants
        k1 = z / (PI * f);
        k2 = 1 / ((2 * PI * f) * (2 * PI * f)); 
        k3 = r * z/ (2 * PI * f);
        T_crit = 0.8f * (Mathf.Sqrt(4 * k2 + k1 * k1) - k1);
    }

    public SecondOrderDynamicsFloat(float f,float z,float r,float x0)
    {
        //compute constants
        k1 = z / (PI * f);
        k2 = 1 / ((2 * PI * f) * (2 * PI * f)); 
        k3 = r * z/ (2 * PI * f);
        T_crit = 0.8f * (Mathf.Sqrt(4 * k2 + k1 * k1) - k1);
        //initalize variables
        xp = x0;
        y = x0;
        yd = 0.0f;
    }

    public float Update(float T, float x, float xd)
    {
        /*if (xd == null) //estimate velocity
        {
            xd = (x - xp) /T;
            xp = x;
        }*/

        int iterations = (int)Mathf.Ceil(T / T_crit); //compute number of iterations
        T = T / iterations; //compute new timestep
        for (int i = 0; i < iterations; i++)
        {
            y = y +T * yd;
            yd = yd + T * (x + k3*xd - y - k1*yd) / k2; //integrate velocity by acceleraction 
        }

        //float k2_stable = Mathf.Max(k2, 1.1f * (T*T/4 + T*k1/2)); //clamp k2 to guarentee stability
        //y = y +T * yd;
        //yd = yd + T * (x + k3*xd - y - k1*yd) / k2_stable; //integrate velocity by acceleraction 
        return y;    
    }
}

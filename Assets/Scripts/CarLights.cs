using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarLights : MonoBehaviour
{
    private float m_horizontalMov;
    public Light light1;
    public Light light2;
    public Light light3;
    public Light light4;
    private CarController m_CarController;
    private void FixedUpdate()
    {
        m_horizontalMov = Input.GetAxis("Vertical");
        if ( m_horizontalMov < 0)
        {
            light1.intensity = 16;
            light2.intensity = 16;
            light3.intensity = 16;
            light4.intensity = 16;
        }
        else 
        {
            light1.intensity = 2;
            light2.intensity = 2;
            light3.intensity = 2;
            light4.intensity = 2;
        }
    } 
    
}

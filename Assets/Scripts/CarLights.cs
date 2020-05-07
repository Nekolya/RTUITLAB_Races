using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarLights : MonoBehaviour
{
    private float m_horizontalMov;
    private CarController m_CarController;
    private void FixedUpdate()
    {
        m_horizontalMov = Input.GetAxis("Vertical");
        if ( m_horizontalMov < 0)
        {
            
            gameObject.GetComponent<MeshRenderer>().enabled = true;

        }
        else 
        {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
    } 
    
}

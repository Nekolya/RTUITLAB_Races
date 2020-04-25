using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{

    private float m_horizontalMov;
    private float m_verticalMov;
    private float m_steerAngle;

    public WheelCollider FL, FR, BL, BR;
    public Transform FLT, FRT, BLT, BRT;
    public float maxSteerAngle = 50;
    public float motorForce = 50;

    public void GetInput()
    {
        m_horizontalMov = Input.GetAxis("Horizontal");
        m_verticalMov = Input.GetAxis("Vertical");
        //Debug.Log(m_verticalMov);

    }

    public void Steer()
    {
        m_steerAngle = maxSteerAngle*m_horizontalMov*0.8f;
        if (m_verticalMov<0) 
        {
            BL.steerAngle = -m_steerAngle*1.1f;
            BR.steerAngle = -m_steerAngle*1.1f;
            FL.steerAngle = -m_steerAngle*1.1f;
            FR.steerAngle = -m_steerAngle*1.1f;
        }
        else
        {
            BL.steerAngle = m_steerAngle*0.06f;
            BR.steerAngle = m_steerAngle*0.06f;
            FL.steerAngle = m_steerAngle;
            FR.steerAngle = m_steerAngle;
        }
    }

    public void Accelerate()
    {
        if (m_verticalMov<0) 
        {
            FR.motorTorque = m_verticalMov * motorForce*0.1f;
            FL.motorTorque = m_verticalMov * motorForce*0.1f;
            BR.motorTorque = m_verticalMov * motorForce*0.01f;
            BL.motorTorque = m_verticalMov * motorForce*0.01f;
        }
        else
        {
            FR.motorTorque = m_verticalMov * motorForce;
            FL.motorTorque = m_verticalMov * motorForce;
            BR.motorTorque = m_verticalMov * motorForce*0.3f;
            BL.motorTorque = m_verticalMov * motorForce*0.3f;
        }
            

    }

    public void UpdateWheelPosition()
    {
        UpdateWheelPose(FR, FRT);
        UpdateWheelPose(FL, FLT);
        UpdateWheelPose(BR, BRT);
        UpdateWheelPose(BL, BLT);
    }

    public void UpdateWheelPose(WheelCollider _collider, Transform _transform)
    {
        Vector3 _pos = _transform.position;
        Quaternion _quat = _transform.rotation;

        _collider.GetWorldPose(out _pos, out _quat);

        _transform.position = _pos;
        _transform.rotation = _quat;
    }

    private void FixedUpdate() 
    {
        GetInput();
        Steer();
        Accelerate();
        UpdateWheelPosition();
    }
    
}

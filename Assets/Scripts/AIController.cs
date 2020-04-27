using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class AIController : MonoBehaviour
{

    private float m_horizontalMov;
    private float m_verticalMov;
    private float m_steerAngle;

    public WheelCollider FL, FR, BL, BR;
    public Transform FLT, FRT, BLT, BRT;
    public float maxSteerAngle = 50;
    public float motorForce = 50;
    public float range = 10;
    public int DRIFT = 10;
    public int WAIT = 15;
    public GameObject waypoints;


    private Transform point;
    private int length;
    private bool passed;
    private int x;
    private int y;
    private int z;
    private float driftTime;
    private float waitTime;

    Queue<Transform> points;

    public bool Passed {get {return passed;} set {passed = value;}}
    

    // Start is called before the first frame update
    void Start()
    {
        points  = new Queue<Transform>();
        length = waypoints.transform.childCount;
        for (int i = 0; i < length; i++)
        {
            point = waypoints.transform.GetChild(i);
            points.Enqueue(point);
        }
        driftTime = 0;
        waitTime = 0;
        passed = false; 
    }


    public void GetMov()
    {
        if (points.Count !=0 ){
            point = points.Peek();

            float _dx = point.position.x - transform.position.x;
            float _dz = point.position.z - transform.position.z;

            float _neededAngle = Mathf.Atan(((float)_dx)/_dz)*180/Mathf.PI;
            
            //if(_dx < 0 ) //2 and 3
                //_neededAngle += 180.0f;
            //else if ((_dx > 0) & (_dz < 0)) // 4
            //    _neededAngle += 360.0f;

            float _currentRot = transform.localEulerAngles.y;

            //if (_currentRot > 0)
            //    _currentRot -= 360.0f;

            

            /////horizontal part
            if (_neededAngle < 0)
                _neededAngle += 360;

            else if(_neededAngle>360)
                _neededAngle -= 360;

            float _absolute = Math.Abs(_neededAngle - _currentRot);

            if (_absolute<15.0f)
                m_horizontalMov = 0.0f;

            else if (_neededAngle > _currentRot)
                m_horizontalMov = 0.3f;
            else 
                m_horizontalMov = -0.3f;

            if (_absolute > 180.0f)
                m_horizontalMov = -m_horizontalMov;


            /////vertical and drift part
            if (driftTime>0){
                m_verticalMov = -1;
                Debug.Log(driftTime);
                driftTime -= 1;
            }
            else if(waitTime>0){
                Debug.Log(waitTime);
                m_verticalMov = 0;
                waitTime -= 1;
            }
            else
                m_verticalMov = 1f;

            //Debug.Log(_neededAngle);
            //Debug.Log(_currentRot);
            //Debug.Log(_absolute); 
            //Debug.Log(m_horizontalMov); 
        }
        else
        {
            m_verticalMov = 0;
            m_horizontalMov = 0;
        }
        
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

    public void CheckZone(){
        float _pointX = point.position.x;
        float _pointZ = point.position.z;
        float _carX = transform.position.x;
        float _carZ = transform.position.z;

        if ( _pointX - range <_carX & _carX < _pointX + range 
        & _pointZ - range <_carZ & _carZ < _pointZ + range ){
            passed = true;
        }
    }

    private void FixedUpdate() 
    {
        GetMov();
        Steer();
        Accelerate();
        UpdateWheelPosition();
        CheckZone();

        if(passed)
        {
            Debug.Log(point.name);
            if (points.Count !=0 )
                points.Dequeue();
            passed = false;
            driftTime = DRIFT;
            waitTime = WAIT;
        }
    }
}

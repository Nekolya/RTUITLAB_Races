using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFix : MonoBehaviour
{
    public GameObject car;


    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles = new Vector3(0, car.transform.eulerAngles.y, 0);
    }
}

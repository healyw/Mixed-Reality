using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    public Camera CameraFacing;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(0,-1.5f,8.24f);
        transform.LookAt(CameraFacing.transform.position);
        transform.Rotate(0, 180, 0);
    }
}

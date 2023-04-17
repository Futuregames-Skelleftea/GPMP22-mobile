using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float smothing;
    public float rotationSmooth;
    public Transform player;


    void Start()
    {
        
    }


    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, player.position, smothing);
        transform.rotation = Quaternion.Slerp(transform.rotation, player.rotation, rotationSmooth);
        transform.rotation = Quaternion.Euler(new Vector3(0, transform.rotation.eulerAngles.y, 0));
    }
}

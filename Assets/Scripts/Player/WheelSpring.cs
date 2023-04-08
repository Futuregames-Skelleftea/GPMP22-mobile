using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelSpring : MonoBehaviour
{
    [SerializeField] Transform anchor;


    private void FixedUpdate()
    {
        transform.rotation = Quaternion.LookRotation(anchor.forward);
    }
}

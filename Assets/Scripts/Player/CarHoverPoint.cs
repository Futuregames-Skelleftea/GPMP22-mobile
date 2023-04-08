using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarHoverPoint : MonoBehaviour
{
    private RaycastHit hit;
    [SerializeField] float hoverForce;  //the force of each hover spot, determines the resting height
    [SerializeField] float maxRayDistance;  //how long the raycast to the ground will be
    [SerializeField] float maxForce;        //the maximum force that can be added by each hover spot
    [SerializeField] Rigidbody carRigidbody;    //rigidbody of the "car"
    [SerializeField] LayerMask layerMask; //layermask thats used so that the ray doesnt hit the car

    private void FixedUpdate()
    {
        Physics.Raycast(transform.position ,Vector3.down, out hit, maxRayDistance,layerMask, QueryTriggerInteraction.Ignore);    //shoots a ray down
        Vector3 forceToAdd = Vector3.ClampMagnitude(Vector3.up * hoverForce / hit.distance, maxForce);  //clamps the force so that it doesnt go crazy when the distance is very small
        carRigidbody.AddForceAtPosition(forceToAdd, transform.position, ForceMode.Force);   //adds the force at the position of the hover spot
    }
}

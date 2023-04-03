using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehaviour : MonoBehaviour
{
    public GameObject ParticlePrefab;
    public TrailRenderer Trail;

    private SpringJoint2D _springJoint;

    private bool _trailTriggered;


    private void OnEnable()
    {
        _springJoint = GetComponent<SpringJoint2D>();
        Trail = GetComponent<TrailRenderer>();
        _trailTriggered = false;
    }

    private void Update()
    {
        if (_trailTriggered) return; //guard clause

        if (_springJoint.enabled == false)
        {
            _trailTriggered = true;
            Trail.emitting = true;
        }
    }

    private void OnDisable()
    {
        Instantiate(ParticlePrefab, this.transform.position, Quaternion.identity);
    }
}

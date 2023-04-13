using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoints : MonoBehaviour
{
    [SerializeField]
    private bool _active;
    [SerializeField]
    private bool _addPoint;
    [SerializeField]
    private Checkpoints _previusCheckpoint;

    private void OnTriggerEnter(Collider other)
    {
        // check if collided with Car
        if (!other.gameObject.transform.parent.TryGetComponent(out Car car)) return;
        
        // Check if previus checkpoint have been activated
        if (_previusCheckpoint._active)
        {
            // deactivate previus and activate this one
            _previusCheckpoint._active = false;
            _active = true;

            // add points if checkpoint adds points
            if (_addPoint)
                GameManager.Instance.Score += 1;
        }
    }
}

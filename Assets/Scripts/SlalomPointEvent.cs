using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SlalomPointEvent : MonoBehaviour
{
    public int Points;


    [SerializeField] private BoxCollider _gate;

    
    public UnityEvent<int> PointIncrease;
    public UnityEvent LapIncrease;

    private void Awake()
    {
        _gate = GetComponent<BoxCollider>();
    }

    public void ActivateGate()
    {
        _gate.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LapIncrease?.Invoke();
            PointIncrease?.Invoke(Points);
            _gate.enabled = false;
            Debug.Log(_gate.enabled);
        }
    }
}

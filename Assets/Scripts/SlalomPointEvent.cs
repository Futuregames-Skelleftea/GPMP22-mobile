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

    private void Awake() => _gate = GetComponent<BoxCollider>(); 

    //method for activating gate easily through unityevents
    public void ActivateGate() => _gate.enabled = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //events
            LapIncrease?.Invoke();
            PointIncrease?.Invoke(Points);
            //disabling collider, keeping this gate from being triggered out of sync
            _gate.enabled = false;
        }
    }
}

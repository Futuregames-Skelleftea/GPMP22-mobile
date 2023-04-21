using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnTouch : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //Destroys other Game objekts it tuches
        Destroy(other.gameObject); 
    }
}

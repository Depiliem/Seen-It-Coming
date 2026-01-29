using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Laser : MonoBehaviour
{
    public float lifeTime = 2f;
    
    void Start()
    {
        Destroy(gameObject, lifeTime);
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Panggil sistem nyawa nanti
            Destroy(other.gameObject);
        }
    }
}

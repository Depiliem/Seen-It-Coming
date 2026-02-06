using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            KillPlayer(other.gameObject);
        }
    }

    void KillPlayer(GameObject player)
    {
        Destroy(player);

        // Reset scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
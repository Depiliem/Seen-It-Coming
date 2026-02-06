using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerFallDeath : MonoBehaviour
{
    public float fallLimit = -10f; // tinggi jatuh minimum

    void Update()
    {
        if (transform.position.y < fallLimit)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}

using UnityEngine;

public class HexagonTile : MonoBehaviour
{
    [Header("Settings")]
    public float depth = 0.2f;     
    public float speed = 10f;       

    private Vector3 initialPos;    
    private Vector3 targetPos;      
    private bool isPressed = false;

    void Start()
    {
        initialPos = transform.position;
        targetPos = initialPos;
    }

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * speed);
    }

    
    void OnCollisionEnter(Collision other)
    {
        
        if (other.gameObject.CompareTag("Player"))
        {
            isPressed = true;
            
            targetPos = initialPos - (Vector3.up * depth);
        }
    }

  
    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPressed = false;
            targetPos = initialPos;
        }
    }
}
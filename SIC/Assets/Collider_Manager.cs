using UnityEngine;

public class ColliderManager : MonoBehaviour
{
    void Start()
    {
        var floors = GameObject.FindGameObjectsWithTag("Lantai");

        foreach (var obj in floors)
        {
            if (!obj.TryGetComponent<BoxCollider>(out var col))
            {
                col = obj.AddComponent<BoxCollider>();
            }

            MeshRenderer mesh = obj.GetComponent<MeshRenderer>();
            if (mesh != null)
            {
                col.center = mesh.bounds.center - obj.transform.position;
                col.size = mesh.bounds.size;
            }
        }
    }
}

using UnityEngine;

public class CCTVTracker : MonoBehaviour
{
    [Header("Target Tracking")]
    public Transform target;       // Drag Robot kamu ke sini
    public float turnSpeed = 2.0f; // Kecepatan putar (semakin kecil = semakin lambat/realistis)

    [Header("Axis Correction")]
    // Ganti ini kalau CCTV malah nengok membelakangi robot atau miring
    public Vector3 rotationOffset = new Vector3(0, 0, 0);

    void Update()
    {
        if (target == null) return;

        // 1. Hitung arah dari CCTV ke Robot
        Vector3 direction = target.position - transform.position;

        // 2. Tentukan rotasi tujuan (Target Rotation)
        Quaternion lookRotation = Quaternion.LookRotation(direction);

        // 3. Gabungkan dengan Offset (Koreksi jika modelnya miring)
        Quaternion finalRotation = lookRotation * Quaternion.Euler(rotationOffset);

        // 4. Gerakkan pelan-pelan (Smooth) menggunakan Slerp
        transform.rotation = Quaternion.Slerp(transform.rotation, finalRotation, turnSpeed * Time.deltaTime);
    }
}
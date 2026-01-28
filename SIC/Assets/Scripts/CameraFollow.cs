using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    public Transform target;        // Drag robot kamu ke sini
    public float distance = 5.0f;   // Jarak kamera ke robot
    public float height = 1.5f;     // Tinggi fokus (biar lihat pundak/kepala, bukan kaki)

    [Header("Settings")]
    public float mouseSensitivity = 5.0f;
    public float rotationSmoothTime = 0.12f; // Semakin besar, semakin "licin" delay-nya
    public Vector2 pitchMinMax = new Vector2(-20, 60); // Batas nunduk & dongak

    // Internal variables
    private Vector3 rotationSmoothVelocity;
    private Vector3 currentRotation;
    private float yaw;   // Tengok kiri-kanan
    private float pitch; // Tengok atas-bawah

    void Start()
    {
        // Menyembunyikan cursor mouse & menguncinya di tengah layar
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // 1. Baca input Mouse
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;

        // 2. Batasi sudut pandang atas-bawah
        pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);

        // 3. Proses Smoothing (Kunci gaya Human Fall Flat)
        currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref rotationSmoothVelocity, rotationSmoothTime);

        // 4. Terapkan Rotasi
        transform.eulerAngles = currentRotation;

        // 5. Terapkan Posisi (Mundur dari target sesuai rotasi)
        // Kita tambah Vector3.up * height agar kamera fokus ke badan atas, bukan kaki
        Vector3 targetPosition = target.position + Vector3.up * height;
        transform.position = targetPosition - transform.forward * distance;
    }
}
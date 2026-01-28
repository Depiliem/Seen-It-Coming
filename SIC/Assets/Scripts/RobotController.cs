using UnityEngine;

public class RobotController : MonoBehaviour
{
    [Header("Components")]
    public Animator animator;
    private Rigidbody rb; // Disimpan biar ringan (caching)

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f; // Kecepatan putar badan

    [Header("Jump & Ground Settings")]
    public float jumpForce = 5f;       // Kekuatan loncat
    public float groundCheckDistance = 0.5f; // Jarak deteksi lantai (DIUBAH BIAR GAK PENDEK)

    void Start()
    {
        // Mengambil komponen Rigidbody saat game mulai
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // ---------------------------------------------------------
        // 1. CEK TANAH (DIPERBAIKI)
        // ---------------------------------------------------------
        // Menggambar garis merah di Scene View untuk debug (biar kelihatan)
        Debug.DrawRay(transform.position + Vector3.up * 0.1f, Vector3.down * groundCheckDistance, Color.red);

        // Kita tembak raycast sedikit dari atas (offset 0.1f) ke bawah
        bool isGrounded = Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, groundCheckDistance);

        animator.SetBool("IsGrounded", isGrounded);

        // ---------------------------------------------------------
        // 2. LOGIKA GERAK (KAMERA RELATIVE)
        // ---------------------------------------------------------
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Ambil arah depan & kanan dari KAMERA UTAMA
        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;

        // Ratakan y-nya jadi 0 (supaya robot gak jalan nanjak ke langit)
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        // Gabungkan input tombol dengan arah kamera
        Vector3 movement = (camForward * vertical) + (camRight * horizontal);

        if (movement.magnitude > 0)
        {
            // --- ROTASI HALUS (SMOOTH) ---
            // Mengubah arah hadap robot ke arah jalan
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            // Slerp bikin muternya gak kaku (snapping), tapi bertahap
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Gerakkan robot
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }

        // Kirim kecepatan ke Animator
        animator.SetFloat("Speed", movement.magnitude, 0.1f, Time.deltaTime);

        // ---------------------------------------------------------
        // 3. LOGIKA LOMPAT
        // ---------------------------------------------------------
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            animator.SetTrigger("Jump");

            // Reset velocity Y dulu biar lompatan konsisten (opsional tapi bagus)
            Vector3 velocity = rb.velocity; // Unity 6 pakai linearVelocity, Unity lama pakai velocity
            velocity.y = 0;
            rb.velocity = velocity;

            // Tambahkan gaya
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}
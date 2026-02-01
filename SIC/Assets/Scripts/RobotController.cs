using UnityEngine;

public class RobotController : MonoBehaviour
{
    [Header("Components")]
    public Animator animator;
    private Rigidbody rb;

    [Header("Movement Settings")]
    public float walkSpeed = 3f;   // Kecepatan Jalan
    public float runSpeed = 8f;    // Kecepatan Lari (Shift)
    public float rotationSpeed = 10f;

    [Header("Jump & Ground Settings")]
    public float jumpForce = 5f;
    public float groundCheckDistance = 0.5f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // 1. CEK TANAH
        Debug.DrawRay(transform.position + Vector3.up * 0.1f, Vector3.down * groundCheckDistance, Color.red);
        bool isGrounded = Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, groundCheckDistance);
        animator.SetBool("IsGrounded", isGrounded);

        // 2. LOGIKA GERAK (KAMERA RELATIVE)
        // PERBAIKAN: Pakai GetAxisRaw biar tidak licin/delay saat lepas tombol
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // PERBAIKAN: Cek tombol Shift untuk lari
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;

        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 movement = (camForward * vertical) + (camRight * horizontal);

        // Agar jalan diagonal tidak ngebut
        movement = movement.normalized;

        if (movement.magnitude > 0.1f)
        {
            // --- ROTASI ---
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // --- GERAK (Versi Kamu: Translate) ---
            // Translate menggerakkan transform langsung (mengabaikan berat fisik)
            transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);
        }

        // Kirim kecepatan ke Animator (0.5 jalan, 1 lari)
        float animSpeed = 0f;
        if (movement.magnitude > 0.1f)
        {
            animSpeed = isRunning ? 1f : 0.5f;
        }
        animator.SetFloat("Speed", animSpeed, 0.1f, Time.deltaTime);

        // 3. LOGIKA LOMPAT
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            animator.SetTrigger("Jump");

            Vector3 velocity = rb.velocity;
            velocity.y = 0;
            rb.velocity = velocity;

            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}
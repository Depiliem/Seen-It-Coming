using UnityEngine;

public class RobotController : MonoBehaviour
{
    [Header("Components")]
    public Animator animator;
    private Rigidbody rb;
    public AudioSource audioSource; // Komponen AudioSource Utama

    [Header("Audio Clips")]
    public AudioClip footstepSound; // Suara langkah (jalan/lari)
    public AudioClip jumpSound;     // Suara lompat

    [Header("Movement Settings")]
    public float walkSpeed = 3f;
    public float runSpeed = 8f;
    public float rotationSpeed = 10f;

    [Header("Jump & Ground Settings")]
    public float jumpForce = 5f;
    public float groundCheckDistance = 0.5f;

    [Header("Audio Timing")]
    public float walkStepInterval = 0.5f; // Jeda suara saat jalan
    public float runStepInterval = 0.3f;  // Jeda suara saat lari (lebih cepat)
    private float stepTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // 1. CEK TANAH
        bool isGrounded = Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, groundCheckDistance);
        animator.SetBool("IsGrounded", isGrounded);

        // 2. LOGIKA GERAK
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;
        camForward.y = 0; camRight.y = 0;
        camForward.Normalize(); camRight.Normalize();

        Vector3 movement = (camForward * vertical) + (camRight * horizontal);
        movement = movement.normalized;

        if (movement.magnitude > 0.1f)
        {
            // ROTASI
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // GERAK
            transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);

            // --- AUDIO LANGKAH KAKI (FOOTSTEPS) ---
            if (isGrounded)
            {
                stepTimer -= Time.deltaTime;
                if (stepTimer <= 0)
                {
                    // Variasi Pitch sedikit agar tidak robotik (opsional)
                    audioSource.pitch = Random.Range(0.9f, 1.1f);
                    float stepVolume = isRunning ? 2f : 1.7f;
                    audioSource.PlayOneShot(footstepSound, stepVolume);

                    // Reset timer berdasarkan apakah sedang lari atau jalan
                    stepTimer = isRunning ? runStepInterval : walkStepInterval;
                }
            }
        }
        else
        {
            stepTimer = 0; // Reset timer jika berhenti agar saat jalan lagi suara langsung bunyi
        }

        // Animator Speed
        float animSpeed = 0f;
        if (movement.magnitude > 0.1f) animSpeed = isRunning ? 1f : 0.5f;
        animator.SetFloat("Speed", animSpeed, 0.1f, Time.deltaTime);

        // 3. LOGIKA LOMPAT & AUDIO LOMPAT
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            animator.SetTrigger("Jump");

            // Audio Lompat (Sekali bunyi)
            audioSource.pitch = 1f; // Reset pitch ke normal
            audioSource.PlayOneShot(jumpSound);

            Vector3 velocity = rb.velocity;
            velocity.y = 0;
            rb.velocity = velocity;
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}
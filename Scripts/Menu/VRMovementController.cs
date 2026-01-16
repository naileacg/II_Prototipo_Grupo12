using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AudioSource))] // 1. Aseguramos que haya un AudioSource
public class VRMovementController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 3.0f;

    [Header("Audio")]
    public AudioClip footstepClip;
    // Opcional: Variar ligeramente el tono para que no suene robótico
    [Range(0.8f, 1.2f)] public float pitchRandomness = 1.0f; 

    [Header("Input Configuration")]
    public InputAction moveAction;

    [Header("References")]
    public Transform vrCamera;

    private CharacterController characterController;
    private AudioSource audioSource; // 2. Referencia al componente de audio

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>(); // Inicializamos la referencia

        if (vrCamera == null)
        {
            vrCamera = Camera.main.transform;
        }

        if (moveAction.bindings.Count == 0)
        {
            moveAction.AddBinding("<Gamepad>/leftStick");
            moveAction.AddCompositeBinding("Dpad")
                .With("Up", "<Keyboard>/w")
                .With("Down", "<Keyboard>/s")
                .With("Left", "<Keyboard>/a")
                .With("Right", "<Keyboard>/d");
        }
    }

    void OnEnable()
    {
        moveAction.Enable();
    }

    void OnDisable()
    {
        moveAction.Disable();
    }

    void Update()
    {
        MoveCharacter();
    }

    void MoveCharacter()
    {
        // 1. Leer el valor del input
        Vector2 inputVector = moveAction.ReadValue<Vector2>();
        
        // Creamos una variable para saber si nos estamos moviendo o no
        bool isMoving = inputVector.sqrMagnitude >= 0.01f;

        Vector3 finalVelocity = Vector3.zero;

        // 2. Cálculo de movimiento (Solo si hay input)
        if (isMoving)
        {
            float inputX = inputVector.x;
            float inputZ = inputVector.y;

            Vector3 movement = vrCamera.forward * inputZ + vrCamera.right * inputX;
            movement.y = 0;
            movement.Normalize();

            finalVelocity = movement * moveSpeed;
        }

        // 3. Gravedad (Se aplica siempre, te muevas o no)
        if (!characterController.isGrounded)
        {
            finalVelocity.y -= 9.81f * Time.deltaTime;
        }

        characterController.Move(finalVelocity * Time.deltaTime);

        // --- LÓGICA DE AUDIO (Ahora sí detecta cuando paras) ---

        if (isMoving && characterController.isGrounded)
        {
            // CASO A: Me muevo y estoy en el suelo -> REPRODUCIR
            if (!audioSource.isPlaying)
            {
                audioSource.pitch = Random.Range(0.9f, 1.1f); // Pequeña variación
                audioSource.clip = footstepClip;
                audioSource.Play();
            }
        }
        else
        {
            // CASO B: He parado (o estoy saltando) -> DETENER
            // Aquí es donde entra cuando sueltas el joystick
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }
}
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonMovement : MonoBehaviour
{
    public float speed = 5;

    [Header("Running")]
    public bool canRun = true;
    public bool IsRunning { get; private set; }
    public float runSpeed = 9;
    public KeyCode runningKey = KeyCode.LeftShift;

    [Header("Stamina")]
    public float maxStamina = 5f; // Maximum stamina in seconds
    public float staminaRecoveryRate = 1f; // Stamina recovered per second
    public float staminaDepletionRate = 1f; // Stamina depleted per second
    public AudioSource fatigueSound; // Sound for fatigue (assign in Inspector)

    private float currentStamina;
    private bool isFatigued;

    Rigidbody myRigidbody; // Cambié el nombre de la variable
    /// <summary> Functions to override movement speed. Will use the last added override. </summary>
    public List<System.Func<float>> speedOverrides = new List<System.Func<float>>();

    void Awake()
    {
        // Get the rigidbody on this.
        myRigidbody = GetComponent<Rigidbody>(); // Actualicé la referencia aquí

        // Initialize stamina
        currentStamina = maxStamina;

        // Ensure the AudioSource is properly configured
        if (fatigueSound != null)
        {
            fatigueSound.loop = true;
            fatigueSound.playOnAwake = false;
        }
    }

    void FixedUpdate()
    {
        HandleStamina();

        // Update IsRunning from input.
        IsRunning = canRun && !isFatigued && Input.GetKey(runningKey);

        // Get targetMovingSpeed.
        float targetMovingSpeed = IsRunning ? runSpeed : speed;
        if (speedOverrides.Count > 0)
        {
            targetMovingSpeed = speedOverrides[speedOverrides.Count - 1]();
        }

        // Get targetVelocity from input.
        Vector2 targetVelocity = new Vector2(Input.GetAxis("Horizontal") * targetMovingSpeed, Input.GetAxis("Vertical") * targetMovingSpeed);

        // Apply movement.
        myRigidbody.velocity = transform.rotation * new Vector3(targetVelocity.x, myRigidbody.velocity.y, targetVelocity.y); // Actualicé la referencia aquí
    }

    private void HandleStamina()
    {
        if (IsRunning)
        {
            // Deplete stamina when running
            currentStamina -= staminaDepletionRate * Time.deltaTime;

            if (currentStamina <= 0)
            {
                currentStamina = 0;
                isFatigued = true;
                PlayFatigueSound();
            }
        }
        else
        {
            // Recover stamina when not running
            currentStamina += staminaRecoveryRate * Time.deltaTime;

            if (currentStamina >= maxStamina)
            {
                currentStamina = maxStamina;
                isFatigued = false;
                StopFatigueSound();
            }
        }
    }

    private void PlayFatigueSound()
    {
        if (fatigueSound != null && !fatigueSound.isPlaying)
        {
            fatigueSound.Play();
        }
    }

    private void StopFatigueSound()
    {
        if (fatigueSound != null && fatigueSound.isPlaying)
        {
            fatigueSound.Stop();
        }
    }
}

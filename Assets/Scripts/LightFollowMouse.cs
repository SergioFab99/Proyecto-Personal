using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFollowMouse : MonoBehaviour
{
    public Transform playerCamera;  // Cámara del jugador
    public Light flashlight;  // Luz de la linterna
    public float flickerDurationMin = 0.5f;  // Duración mínima del parpadeo
    public float flickerDurationMax = 1.5f;  // Duración máxima del parpadeo
    public float flickerSpeed = 0.03f;  // Velocidad del parpadeo rápido
    public float maxBatteryLife = 300f;  // Duración máxima de la batería en segundos (5 minutos)
    public float lowBatteryThreshold = 60f;  // Umbral de batería baja en segundos
    public float energyDrainRate = 1f;  // Velocidad a la que se drena la batería por segundo
    public float rechargeRate = 20f;  // Cantidad de batería recargada por segundo cuando está apagada

    private bool isFlickering = false;
    private bool canFlicker = false;  // Controla cuando la linterna puede empezar a fallar
    private float nextFlickerAllowedTime = 0f;  // Tiempo para permitir el siguiente fallo
    public float currentBatteryLife;  // Vida actual de la batería
    private bool isOutOfBattery = false;  // Indica si la batería está completamente agotada

    private void Start()
    {
        currentBatteryLife = 60f;  // Inicializar la batería con 60 segundos
    }

    private void Update()
    {
        // Verifica si la linterna está encendida
        if (flashlight.enabled && !isOutOfBattery)
        {
            // Drena la batería mientras la linterna está encendida
            currentBatteryLife -= energyDrainRate * Time.deltaTime;

            // Si la batería está por debajo del umbral, permitir que falle
            if (currentBatteryLife <= lowBatteryThreshold)
            {
                canFlicker = true;
            }

            // Apaga la linterna si la batería se agota completamente
            if (currentBatteryLife <= 0f)
            {
                TurnOffFlashlight();
                isOutOfBattery = true;
            }
        }
        else if (!flashlight.enabled && !isOutOfBattery) // Recarga la batería cuando está apagada manualmente
        {
            RechargeBattery(rechargeRate * Time.deltaTime);
        }

        // Si la linterna no está parpadeando y está en estado de fallo
        if (!isFlickering && canFlicker && Time.time >= nextFlickerAllowedTime)
        {
            StartCoroutine(FlickerLight());
        }

        // Rota la luz para que siga la dirección de la cámara
        transform.rotation = playerCamera.rotation;
    }

    // Coroutine para hacer que la linterna parpadee como si se estuviera apagando
    IEnumerator FlickerLight()
    {
        isFlickering = true;

        // Duración aleatoria del parpadeo
        float flickerDuration = Random.Range(flickerDurationMin, flickerDurationMax);
        float flickerEndTime = Time.time + flickerDuration;

        // Mientras la duración del parpadeo no haya terminado, parpadea rápidamente
        while (Time.time < flickerEndTime)
        {
            flashlight.enabled = !flashlight.enabled;  // Apaga/enciende la luz rápidamente
            yield return new WaitForSeconds(flickerSpeed);  // Espera antes de cambiar el estado de la luz
        }

        // Después del parpadeo, asegura que la luz vuelva a estar encendida si hay batería
        if (currentBatteryLife > 0f && flashlight.enabled)
        {
            flashlight.enabled = true;
        }

        isFlickering = false;

        // Establece un tiempo mínimo para permitir el siguiente fallo
        nextFlickerAllowedTime = Time.time + Random.Range(20f, 30f);
    }

    // Método para apagar la linterna
    private void TurnOffFlashlight()
    {
        flashlight.enabled = false;
        canFlicker = false;
    }

    // Método para recargar la batería (solo cuando está apagada manualmente)
    private void RechargeBattery(float amount)
    {
        if (!flashlight.enabled && !isOutOfBattery)  // Solo recargar si está apagada manualmente
        {
            currentBatteryLife = Mathf.Clamp(currentBatteryLife + amount, 0, maxBatteryLife);
        }

        // Aunque se recargue la batería, si está agotada completamente, no podrá encenderse
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFollowMouse : MonoBehaviour
{
    public Transform playerCamera;  // Asigna la cámara del jugador en el Inspector
    public Light flashlight;  // Asigna la luz de la linterna en el Inspector
    public float flickerDurationMin = 0.5f;  // Duración mínima del parpadeo
    public float flickerDurationMax = 1.5f;  // Duración máxima del parpadeo
    public float flickerSpeed = 0.05f;  // Velocidad del parpadeo rápido

    private bool isFlickering = false;
    private float timeElapsed = 0f;  // Tiempo transcurrido desde el inicio
    private bool canFlicker = false;  // Controla cuando la linterna puede empezar a fallar
    private float nextFlickerAllowedTime = 0f;  // Tiempo para permitir el siguiente fallo

    private void Update()
    {
        timeElapsed += Time.deltaTime;  // Sumar el tiempo transcurrido

        // Si han pasado más de 60 segundos, habilita la posibilidad de que la linterna falle
        if (timeElapsed >= 60f)
        {
            canFlicker = true;
        }

        // Si la linterna no está parpadeando y ha pasado más de 60 segundos
        if (!isFlickering && canFlicker && Time.time >= nextFlickerAllowedTime)
        {
            // Hacer que la linterna falle aleatoriamente entre 20 y 30 segundos
            if (timeElapsed >= 60f + Random.Range(20f, 30f))
            {
                StartCoroutine(FlickerLight());
            }
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

        // Después del parpadeo, asegura que la luz vuelva a estar encendida
        flashlight.enabled = true;
        isFlickering = false;

        // Establece un tiempo mínimo para permitir el siguiente fallo
        nextFlickerAllowedTime = Time.time + Random.Range(20f, 30f);
    }
}

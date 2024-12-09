using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleLight : MonoBehaviour
{
    public Light spotLight;        // Referencia a la luz que queremos activar/desactivar
    public AudioClip toggleSound;  // El clip de audio que se reproducirá al alternar la luz
    private AudioSource audioSource;

    // Referencia al script LightFollowMouse para controlar el estado de la batería
    public LightFollowMouse lightFollowMouse;

    void Start()
    {
        // Obtener o agregar el componente AudioSource al GameObject
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = toggleSound;
    }

    void Update()
    {
        // Verifica si el jugador presiona la tecla F
        if (Input.GetKeyDown(KeyCode.F))
        {
            // Si la batería está agotada, no permitir encender la luz
            if (lightFollowMouse.currentBatteryLife <= 0f)
            {
                Debug.Log("No puedes encender la linterna, la batería está agotada.");
                return; // Salir de la función
            }

            // Alterna el estado de la luz (si está activa, la desactiva y viceversa)
            spotLight.enabled = !spotLight.enabled;

            // Reproducir el audio si la luz se enciende o apaga
            audioSource.Play();
        }
    }
}

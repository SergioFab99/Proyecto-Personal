using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleLight : MonoBehaviour
{
    public Light spotLight;        // Referencia a la luz que queremos activar/desactivar
    public AudioClip toggleSound;  // El clip de audio que se reproducir� al alternar la luz
    private AudioSource audioSource;

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
            // Alterna el estado de la luz (si est� activa, la desactiva y viceversa)
            spotLight.enabled = !spotLight.enabled;

            // Reproducir el audio
            audioSource.Play();
        }
    }
}

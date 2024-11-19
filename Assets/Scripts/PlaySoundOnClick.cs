using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlaySoundOnClick : MonoBehaviour, IPointerEnterHandler
{
    public AudioClip soundClip; // El clip de sonido que deseas reproducir
    private AudioSource audioSource; // El componente AudioSource

    void Start()
    {
        // Obtener o agregar el componente AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();

        // Configurar el AudioSource
        audioSource.clip = soundClip;
        audioSource.playOnAwake = false;
    }

    // Método que se llama cuando el mouse entra en el área del botón
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Reproducir el sonido cuando el mouse pasa por encima del botón
        if (audioSource != null && soundClip != null)
        {
            audioSource.Play();
        }
    }
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ScreamManager : MonoBehaviour
{
    [Header("Screamer Settings")]
    [SerializeField] private Transform panelParent;           // Panel que contiene las imágenes
    [SerializeField] private Image panelBackground;           // Fondo negro del panel
    [SerializeField] private float screamerDuration = 0.3f;  // Duración durante la cual el screamer estará activo
    [SerializeField] private Vector2 intervalRange = new Vector2(50, 70); // Intervalo aleatorio entre screamers (en segundos)
    [SerializeField] private float initialDelay = 120f;      // Tiempo antes del primer screamer (en segundos)

    private int currentIndex = 0;                           // Índice del screamer actual

    [Header("Audio Settings")]
    [SerializeField] private AudioSource[] screamerAudioSources; // Arreglo de diferentes sonidos
    private List<int> availableSoundIndexes;                  // Lista para controlar sonidos no repetidos

    private void Start()
    {
        // Validar que haya al menos un AudioSource asignado
        if (screamerAudioSources == null || screamerAudioSources.Length == 0)
        {
            Debug.LogError("No hay AudioSources asignados. Por favor, arrástralos al inspector.");
            return;
        }

        // Inicializa la lista de índices de sonidos disponibles
        ResetSoundIndexes();

        // Asegúrate de que el fondo del Panel sea transparente al inicio
        if (panelBackground != null)
        {
            SetPanelBackgroundAlpha(0f); // Fondo transparente
        }

        // Asegúrate de que todas las imágenes estén desactivadas al inicio
        DeactivateAllChildren();

        // Inicia la secuencia después de un retraso inicial
        StartCoroutine(ScreamerSequence());
    }

    private void DeactivateAllChildren()
    {
        if (panelParent == null)
        {
            Debug.LogError("Panel Parent no está asignado.");
            return;
        }

        foreach (Transform child in panelParent)
        {
            Image image = child.GetComponent<Image>();
            if (image != null)
            {
                child.gameObject.SetActive(false);
            }
            else
            {
                Debug.LogWarning($"El hijo {child.name} no tiene un componente Image y será ignorado.");
            }
        }
    }

    private IEnumerator ScreamerSequence()
    {
        // Espera el retraso inicial antes de comenzar la secuencia
        yield return new WaitForSeconds(initialDelay);

        while (currentIndex < panelParent.childCount)
        {
            // Activa el fondo negro
            if (panelBackground != null)
            {
                SetPanelBackgroundAlpha(1f); // Fondo opaco
            }

            // Activa la imagen actual
            Transform currentChild = panelParent.GetChild(currentIndex);
            currentChild.gameObject.SetActive(true);

            Debug.Log($"Activando: {currentChild.name}");

            // Reproduce un sonido aleatorio no repetido y limítalo a 1 segundo
            StartCoroutine(PlaySoundWithLimit());

            // Espera la duración del screamer
            yield return new WaitForSeconds(screamerDuration);

            // Desactiva la imagen
            currentChild.gameObject.SetActive(false);
            Debug.Log($"Desactivando: {currentChild.name}");

            // Fondo vuelve a ser transparente
            if (panelBackground != null)
            {
                SetPanelBackgroundAlpha(0f);
            }

            // Incrementa el índice para pasar al siguiente hijo
            currentIndex++;

            // Si todavía quedan hijos, espera un intervalo aleatorio antes de continuar
            if (currentIndex < panelParent.childCount)
            {
                float waitTime = Random.Range(intervalRange.x, intervalRange.y);
                Debug.Log($"Esperando {waitTime} segundos antes del siguiente screamer.");
                yield return new WaitForSeconds(waitTime);
            }
        }

        Debug.Log("Todos los screamers han sido activados una vez.");
    }

    private IEnumerator PlaySoundWithLimit()
    {
        if (availableSoundIndexes.Count == 0)
        {
            // Reinicia la lista de sonidos cuando todos hayan sido reproducidos
            ResetSoundIndexes();
        }

        // Selecciona un índice aleatorio de la lista de disponibles
        int randomIndex = Random.Range(0, availableSoundIndexes.Count);
        int soundIndex = availableSoundIndexes[randomIndex];

        // Reproduce el AudioSource correspondiente
        if (screamerAudioSources[soundIndex] != null)
        {
            AudioSource selectedAudio = screamerAudioSources[soundIndex];
            selectedAudio.Play();

            // Espera 1 segundo antes de detener el audio
            yield return new WaitForSeconds(1f);

            // Detén el audio manualmente
            selectedAudio.Stop();
        }

        // Elimina el índice utilizado de la lista
        availableSoundIndexes.RemoveAt(randomIndex);
    }

    private void ResetSoundIndexes()
    {
        // Llena la lista con índices de todos los sonidos disponibles
        availableSoundIndexes = new List<int>();
        for (int i = 0; i < screamerAudioSources.Length; i++)
        {
            availableSoundIndexes.Add(i);
        }
    }

    private void SetPanelBackgroundAlpha(float alpha)
    {
        if (panelBackground != null)
        {
            Color color = panelBackground.color;
            color.a = alpha;
            panelBackground.color = color;
        }
    }
}

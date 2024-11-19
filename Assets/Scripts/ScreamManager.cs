using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ScreamManager : MonoBehaviour
{
    [Header("Screamer Settings")]
    public Transform panelParent;        // Panel que contiene las imágenes
    public Image panelBackground;        // Fondo negro del panel
    private float screamerDuration = 0.3f; // Duración durante la cual el screamer estará activo
    public Vector2 intervalRange = new Vector2(50, 70); // Intervalo aleatorio entre screamers
    private float initialDelay = 240f;    // Tiempo antes del primer screamer

    private int currentIndex = 0;        // Índice del screamer actual

    [Header("Audio Settings")]
    public AudioSource[] screamerAudioSources; // Arreglo de diferentes sonidos
    private List<int> availableSoundIndexes;   // Lista para controlar sonidos no repetidos

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

            // Reproduce un sonido aleatorio no repetido
            PlayRandomSound();

            Debug.Log($"Activando: {currentChild.name}");

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

    private void PlayRandomSound()
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
            screamerAudioSources[soundIndex].volume = 1f; // Asegura que el volumen esté activo
            screamerAudioSources[soundIndex].Play();
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
        Color color = panelBackground.color;
        color.a = alpha;
        panelBackground.color = color;
    }
}

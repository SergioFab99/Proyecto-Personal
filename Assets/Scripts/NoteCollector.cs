using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteCollector : MonoBehaviour
{
    public float detectionRadius = 4f;  // Radio de detección alrededor de la nota
    public AudioClip recolectarSonido;  // Clip de audio que se reproducirá al recolectar la nota
    private AudioSource audioSource;    // AudioSource para reproducir el sonido
    private Transform player;           // Referencia al transform del jugador
    private GameManager gameManager;    // Referencia al GameManager
    private bool isCollected = false;   // Bandera para evitar recolecciones múltiples

    void Start()
    {
        // Buscar el transform del jugador usando el tag "Player"
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        // Obtener referencia al GameManager
        gameManager = FindObjectOfType<GameManager>();

        // Agregar un AudioSource si no existe
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;  // Para que no suene al inicio
        audioSource.clip = recolectarSonido;

        if (player == null)
            Debug.LogError("El jugador no fue encontrado. Asegúrate de que el GameObject del jugador tenga el tag 'Player'.");

        if (gameManager == null)
            Debug.LogError("GameManager no encontrado en la escena.");
    }

    void Update()
    {
        // Verificar si el jugador está cerca, presiona 'E', y la nota no ha sido recolectada
        if (!isCollected && player != null && Vector3.Distance(transform.position, player.position) <= detectionRadius && Input.GetKeyDown(KeyCode.E))
        {
            isCollected = true; // Marcar como recolectado

            // Reproducir el sonido
            if (audioSource != null && recolectarSonido != null)
            {
                audioSource.Play();
            }

            // Notificar al GameManager y destruir la nota después del sonido
            StartCoroutine(RecolectarDespuesDeSonido());
        }
    }

    // Corrutina para esperar a que el sonido termine antes de destruir el objeto
    private IEnumerator RecolectarDespuesDeSonido()
    {
        // Espera hasta que el sonido termine
        yield return new WaitWhile(() => audioSource.isPlaying);

        // Notificar al GameManager
        gameManager.NotaRecolectada(this.gameObject);

        // Destruir la nota
        Destroy(gameObject);
    }

    // Dibujar el radio de detección en la vista de la escena (opcional, para visualización)
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}

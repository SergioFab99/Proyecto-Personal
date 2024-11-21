using System.Collections;
using UnityEngine;

public class NoteCollector : MonoBehaviour
{
    public float detectionRadius = 4f;
    public AudioClip recolectarSonido;
    private AudioSource audioSource;
    private Transform player;
    private GameManager gameManager;
    private bool isCollected = false;
    private SpriteRenderer spriteRenderer;

    private Vector3 initialScale; // Escala inicial tomada del objeto

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        gameManager = FindObjectOfType<GameManager>();
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.clip = recolectarSonido;

        // Obtener el SpriteRenderer y la escala inicial
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
            Debug.LogError("El objeto no tiene un SpriteRenderer asignado.");

        initialScale = transform.localScale;

        if (player == null)
            Debug.LogError("El jugador no fue encontrado. Asegúrate de que el GameObject del jugador tenga el tag 'Player'.");

        if (gameManager == null)
            Debug.LogError("GameManager no encontrado en la escena.");
    }

    void Update()
    {
        if (!isCollected && player != null && Vector3.Distance(transform.position, player.position) <= detectionRadius && Input.GetKeyDown(KeyCode.E))
        {
            isCollected = true;

            if (audioSource != null && recolectarSonido != null)
            {
                audioSource.Play();
            }

            // Iniciar la animación de enrollado
            StartCoroutine(RecolectarConEnrollado());
        }
    }

    private IEnumerator RecolectarConEnrollado()
    {
        float duration = 1f; // Duración de la animación
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            // Simular el enrollado respetando la escala inicial
            float rollEffect = Mathf.Sin(t * Mathf.PI); // Movimiento sinusoidal
            transform.localScale = new Vector3(initialScale.x * (1f - rollEffect), initialScale.y, initialScale.z); // Modificar solo el ancho
            transform.localRotation = Quaternion.Euler(0, 0, rollEffect * 45f); // Añadir un ligero giro

            // Simular profundidad con un desplazamiento en Z
            transform.position += new Vector3(0, 0, rollEffect * 0.01f);

            yield return null;
        }

        // Notificar al GameManager y destruir la nota
        gameManager.NotaRecolectada(this.gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}

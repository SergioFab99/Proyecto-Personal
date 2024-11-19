using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FollowPlayer : MonoBehaviour
{
    public Transform player; // Referencia al transform del jugador
    public float speed = 5f; // Velocidad base del enemigo
    private float rotationSpeed = 5f; // Velocidad de rotación para mirar al jugador

    public float detectionRadius = 20f; // Radio en el cual empieza la música
    public float maxVolume = 1f; // Volumen máximo de la música
    public float fadeSpeed = 0.5f; // Velocidad de aumento de volumen

    public AudioSource audioSource; // AudioSource ahora es público para arrastrarlo desde el editor
    private bool isNearPlayer = false; // Indica si el objeto está en el radio cercano al jugador

    public float obstacleAvoidanceDistance = 3f; // Distancia para detectar obstáculos
    public float obstacleAvoidanceStrength = 2f; // Fuerza de desviación al evitar obstáculos
    public float stuckThreshold = 0.1f; // Umbral para determinar si el enemigo está atascado
    public float stuckTimeLimit = 2f; // Tiempo máximo que puede permanecer atascado antes de reducir el collider

    public Vector3 reducedColliderSize = new Vector3(0.5f, 1f, 0.5f); // Tamaño reducido del collider
    private Vector3 originalColliderSize; // Tamaño original del collider
    private BoxCollider boxCollider; // Referencia al BoxCollider del enemigo
    private bool isColliderReduced = false; // Verifica si el collider está reducido

    private Vector3 lastPosition; // Última posición registrada para detectar si está atascado
    private float stuckTimer = 0f; // Temporizador para desbloquearse si está atascado
    private float speedMultiplier = 1f; // Multiplicador de velocidad basado en las notas recolectadas

    void Start()
    {
        if (audioSource != null)
        {
            audioSource.volume = 0f;
            audioSource.Stop(); // Aseguramos que no se esté reproduciendo al inicio
        }

        lastPosition = transform.position; // Inicializar la última posición

        // Obtener el BoxCollider y guardar su tamaño original
        boxCollider = GetComponent<BoxCollider>();
        if (boxCollider != null)
        {
            originalColliderSize = boxCollider.size;
        }
        else
        {
            Debug.LogError("No se encontró un BoxCollider en el enemigo.");
        }
    }

    void Update()
    {
        if (player != null)
        {
            // Dirección hacia el jugador
            Vector3 directionToPlayer = player.position - transform.position;
            directionToPlayer.Normalize(); // Normalizamos la dirección para evitar aceleración

            // Evitar obstáculos
            Vector3 adjustedDirection = AvoidObstacles(directionToPlayer);

            // Manejar si el enemigo está atascado
            HandleStuck();

            // Movimiento hacia el jugador (o en dirección ajustada) con incremento de velocidad
            transform.position += adjustedDirection * (speed * speedMultiplier) * Time.deltaTime;

            // Rotación para mirar hacia el jugador
            Quaternion lookRotation = Quaternion.LookRotation(adjustedDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);

            // Calcular distancia al jugador
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // Activar la música cuando esté dentro del radio de 20 metros y ajustar gradualmente el volumen
            if (distanceToPlayer <= detectionRadius)
            {
                if (!isNearPlayer)
                {
                    // Iniciar la música si no estaba ya cerca del jugador
                    audioSource.Play();
                    isNearPlayer = true;
                }
                
                // Aumentar volumen gradualmente hasta el máximo
                audioSource.volume = Mathf.MoveTowards(audioSource.volume, maxVolume, fadeSpeed * Time.deltaTime);
            }
            else
            {
                // Reducir el volumen gradualmente cuando se aleja del jugador
                audioSource.volume = Mathf.MoveTowards(audioSource.volume, 0f, fadeSpeed * Time.deltaTime);
                
                // Parar la música si el volumen llega a cero y el objeto está fuera del radio
                if (audioSource.volume <= 0f && isNearPlayer)
                {
                    audioSource.Stop();
                    isNearPlayer = false;
                }
            }

            // Verificar si colisiona o está lo suficientemente cerca del jugador
            if (distanceToPlayer <= 3f) // Aquí definimos "1f" como la distancia mínima para colisionar
            {
                SceneManager.LoadScene("GameOver"); // Cargar la escena GameOver
            }

            // Actualizar la posición para verificar si está atascado
            UpdateStuckTimer();
        }
    }

    // Método para evitar obstáculos usando raycasts avanzados
    private Vector3 AvoidObstacles(Vector3 directionToPlayer)
    {
    RaycastHit hit;
    float rayDistance = obstacleAvoidanceDistance;

    // Usar SphereCast para detectar obstáculos en un área amplia
    if (Physics.SphereCast(transform.position, 0.5f, directionToPlayer, out hit, rayDistance))
    {
        // Calcular una dirección para rodear el obstáculo
        Vector3 avoidDirection = Vector3.Cross(hit.normal, Vector3.up);
        return avoidDirection.normalized;
    }

    return directionToPlayer.normalized;
    }

    // Manejar si el enemigo está atascado
    private void HandleStuck()
    {
        if (stuckTimer > stuckTimeLimit && !isColliderReduced)
        {
            // Configurar el collider como trigger para atravesar obstáculos
            if (boxCollider != null)
            {
                boxCollider.isTrigger = true;
                isColliderReduced = true;
                Debug.Log("Collider configurado como trigger para atravesar obstáculos.");
            }
        }
        else if (isColliderReduced && stuckTimer == 0f)
        {
            // Restaurar el collider a su estado normal
            if (boxCollider != null)
            {
                boxCollider.isTrigger = false;
                isColliderReduced = false;
                Debug.Log("Collider restaurado a su estado normal.");
            }
        }
    }

    // Actualizar el temporizador para detectar si está atascado
    private void UpdateStuckTimer()
    {
        float distanceMoved = Vector3.Distance(transform.position, lastPosition);

        if (distanceMoved < stuckThreshold)
        {
            stuckTimer += Time.deltaTime;
        }
        else
        {
            stuckTimer = 0f; // Reiniciar si se está moviendo
        }

        lastPosition = transform.position;
    }

    // Método público para incrementar la velocidad del enemigo
    public void IncrementSpeed()
    {
        speedMultiplier += 0.05f; // Incrementar la velocidad en un 5%
        Debug.Log($"Nueva velocidad del enemigo: {speed * speedMultiplier}");
    }
}

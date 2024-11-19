using UnityEngine;

public class ProxyController : MonoBehaviour
{
    public Transform player;           // Referencia al jugador
    public float chaseDistance = 10f;  // Distancia mínima para empezar a perseguir
    public float moveSpeed = 2f;       // Velocidad de movimiento del Proxy

    private Animator animator;
    private bool isChasing = false;

    void Start()
    {
        // Obtener el componente Animator del Proxy
        animator = GetComponent<Animator>();

        if (player == null)
        {
            Debug.LogError("Jugador no asignado en el ProxyController.");
        }
    }

    void Update()
    {
        if (player != null)
        {
            // Calcular la distancia al jugador
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // Si el jugador está dentro de la distancia de persecución, activar el chase
            if (distanceToPlayer <= chaseDistance && !isChasing)
            {
                isChasing = true;
                animator.SetBool("isChasing", true);
            }
            // Si el jugador está fuera de la distancia de persecución, desactivar el chase
            else if (distanceToPlayer > chaseDistance && isChasing)
            {
                isChasing = false;
                animator.SetBool("isChasing", false);
            }

            // Si el Proxy está persiguiendo al jugador, moverlo y orientarlo
            if (isChasing)
            {
                // Calcular la dirección hacia el jugador
                Vector3 direction = (player.position - transform.position).normalized;

                // Mover el Proxy en la dirección del jugador
                transform.position += direction * moveSpeed * Time.deltaTime;

                // Rotar el Proxy para mirar hacia el jugador
                transform.rotation = Quaternion.LookRotation(direction);
            }
        }
    }
}
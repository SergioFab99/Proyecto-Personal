using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para cargar escenas

public class TriggerGameOver : MonoBehaviour
{
    // Se llama cuando otro collider entra en el trigger
    private void OnTriggerEnter(Collider other)
    {
        // Verifica si el objeto que entra en el trigger es el jugador
        if (other.CompareTag("Player"))
        {
            // Aquí cargas la escena de Game Over
            SceneManager.LoadScene("GameOver"); // Cambia "GameOver" por el nombre de tu escena
        }
    }
}

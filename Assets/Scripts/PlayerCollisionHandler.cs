using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCollisionHandler : MonoBehaviour
{
    public string gameOverScene = "GameOver";  // Nombre de la escena de GameOver

    private void OnCollisionEnter(Collision collision)
    {
        // Confirmar que la colisión es con el enemigo (Proxy)
        if (collision.gameObject.CompareTag("Proxy"))
        {
            Debug.Log("Colisión con Proxy detectada. Cargando escena GameOver.");
            SceneManager.LoadScene(gameOverScene);  // Cargar la escena de GameOver
        }
    }
}

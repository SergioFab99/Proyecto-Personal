using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    void Start()
    {
        // Llamar al método para cambiar de escena después de 4 segundos
        Invoke("ChangeScene", 4f);
    }

    void ChangeScene()
    {
        // Cambia a la siguiente escena en el índice de Build Settings
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("No hay más escenas en el Build Settings.");
        }
    }
}

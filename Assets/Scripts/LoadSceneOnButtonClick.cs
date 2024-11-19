using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadSceneOnButtonClick : MonoBehaviour
{
    public Button button;
    public string sceneName; // Nombre de la escena a cargar

    void Start()
    {
        if (button == null)
        {
            button = GetComponent<Button>();
        }

        button.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        // Verifica si se ha asignado una escena por nombre
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("No se ha asignado ninguna escena para cargar.");
        }
    }
}

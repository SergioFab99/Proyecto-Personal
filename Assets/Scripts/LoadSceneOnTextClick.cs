using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class LoadSceneOnTextClick : MonoBehaviour, IPointerClickHandler
{
    public string sceneName;  // Nombre de la escena a cargar

    // Método que se ejecuta cuando se hace clic en el texto
    public void OnPointerClick(PointerEventData eventData)
    {
        // Cargar la escena especificada
        SceneManager.LoadScene(sceneName);
    }
}

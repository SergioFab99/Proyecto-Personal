using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;  // Incluye esta línea si usas TextMeshPro, aunque no es estrictamente necesario aquí

public class ManejadorBotonReintentar: MonoBehaviour
{
    // Función para recargar la escena Nivel1
    public void ReintentarNivel()
    {
        SceneManager.LoadScene("Nivel1");
    }
}

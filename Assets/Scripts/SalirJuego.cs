using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Asegúrate de incluir esta línea para el botón.

public class SalirJuego : MonoBehaviour
{
    public Button salirButton; // Arrastra el botón desde el editor a este campo.

    void Start()
    {
        if (salirButton != null)
        {
            // Vinculamos la función de salida al evento OnClick del botón
            salirButton.onClick.AddListener(SalirDelJuego);
        }
        else
        {
            Debug.LogError("Botón de salida no asignado en el Inspector.");
        }
    }

    public void SalirDelJuego()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}

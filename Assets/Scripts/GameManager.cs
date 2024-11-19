using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // Importar TextMeshPro

public class GameManager : MonoBehaviour
{
    public Transform notasParent;      // Referencia al objeto padre que contiene todas las notas
    public TMP_Text progresoTexto;     // Referencia al texto TextMeshPro para mostrar el progreso
    private int totalNotas;            // Total de notas en la escena
    private int notasRecolectadas = 0; // Contador de notas recolectadas

    void Start()
    {
        // Asegurarse de que `notasParent` esté asignado
        if (notasParent == null)
        {
            Debug.LogError("No se ha asignado el objeto padre de las notas.");
        }

        // Asegurarse de que el texto esté asignado
        if (progresoTexto == null)
        {
            Debug.LogError("El texto de progreso (TextMeshPro) no está asignado en el Canvas.");
        }
        else
        {
            progresoTexto.gameObject.SetActive(false); // Hacer el texto invisible al inicio
        }

        // Contar las notas iniciales
        totalNotas = notasParent.childCount;
    }

    // Método llamado por cada nota cuando es recolectada
    public void NotaRecolectada(GameObject nota)
    {
        Destroy(nota);  // Destruye la nota recolectada
        notasRecolectadas++; // Incrementa el contador
        MostrarProgresoTemporal(); // Muestra el texto de progreso por 4 segundos
        Debug.Log($"Nota recolectada. Notas restantes: {notasParent.childCount - 1}");

        // Verifica si se recolectaron todas las notas
        if (notasRecolectadas == totalNotas)
        {
            Debug.Log("Todas las notas recolectadas. Cargando escena 'Ganaste'...");
            SceneManager.LoadScene("Ganaste");
        }
    }

    // Muestra el progreso temporalmente
    private void MostrarProgresoTemporal()
    {
        if (progresoTexto != null)
        {
            progresoTexto.text = $"Página {notasRecolectadas}/{totalNotas} recolectada";
            progresoTexto.gameObject.SetActive(true); // Hacer visible el texto
            StartCoroutine(EsconderTexto()); // Esconder después de 4 segundos
        }
    }

    // Corrutina para esconder el texto después de 4 segundos
    private IEnumerator EsconderTexto()
    {
        yield return new WaitForSeconds(4f); // Esperar 4 segundos
        progresoTexto.gameObject.SetActive(false); // Hacer invisible el texto
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SpotLightMover : MonoBehaviour
{
    [Header("Movimiento de Luz")]
    [Tooltip("Velocidad del movimiento de la luz")]
    [Range(0.1f, 10.0f)]
    public float velocidad = 1.0f;

    [Tooltip("Distancia desde la posición inicial para el movimiento de izquierda a derecha")]
    [Range(1.0f, 50.0f)]
    public float rangoMovimiento = 5.0f;

    [Header("Efecto de Parpadeo")]
    [Tooltip("Velocidad del parpadeo de la luz")]
    [Range(0.1f, 5.0f)]
    public float velocidadParpadeo = 1.0f;

    [Tooltip("Intensidad máxima de la luz")]
    [Range(0.0f, 10.0f)]
    public float intensidadMaxima = 4.0f;

    private float posicionInicialX;
    private Light spotLight;
    private float parpadeoTimer;
    private bool movimientoCompleto = false;
    private float timerCambioEscena = 10.0f;

    void Start()
    {
        // Guardar la posición inicial para calcular el movimiento
        posicionInicialX = transform.position.x;
        // Obtener la luz del componente
        spotLight = GetComponent<Light>();
    }

    void Update()
    {
        if (!movimientoCompleto)
        {
            // Movimiento lineal de izquierda a derecha hasta alcanzar el rango de movimiento
            float nuevaPosicionX = transform.position.x + velocidad * Time.deltaTime;
            transform.position = new Vector3(nuevaPosicionX, transform.position.y, transform.position.z);

            // Verificar si hemos alcanzado el límite de movimiento
            if (transform.position.x >= posicionInicialX + rangoMovimiento)
            {
                movimientoCompleto = true; // Marca el movimiento como completo
            }
        }
        else
        {
            // Iniciar el temporizador para cambiar de escena después de 10 segundos
            timerCambioEscena += Time.deltaTime;
            if (timerCambioEscena >= 10f)
            {
                CambiarEscena();
            }
        }

        // Efecto de parpadeo
        parpadeoTimer += Time.deltaTime * velocidadParpadeo;
        float intensidad = Mathf.Abs(Mathf.Sin(parpadeoTimer)) * intensidadMaxima;
        spotLight.intensity = intensidad;
    }

    void CambiarEscena()
    {
        int escenaActual = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(escenaActual + 1); // Cambiar a la siguiente escena en los Build Settings
    }
}

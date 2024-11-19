using UnityEngine;

public class LightMaskMover: MonoBehaviour
{
    public RectTransform lightMask; // La imagen que actuará como luz
    public float speed = 100f; // Velocidad de movimiento

    private void Update()
    {
        // Mueve la máscara de izquierda a derecha
        lightMask.localPosition += Vector3.right * speed * Time.deltaTime;

        // Reinicia la posición cuando sale del texto
        if (lightMask.localPosition.x > 500) // ajusta según el tamaño del Canvas y el texto
        {
            lightMask.localPosition = new Vector3(-500, lightMask.localPosition.y, 0);
        }
    }
}

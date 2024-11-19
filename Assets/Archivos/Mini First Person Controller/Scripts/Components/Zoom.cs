using UnityEngine;

[ExecuteInEditMode]
public class Zoom : MonoBehaviour
{
    Camera myCamera; // Cambié el nombre de la variable
    public float defaultFOV = 60;
    public float maxZoomFOV = 15;
    [Range(0, 1)]
    public float currentZoom;
    public float sensitivity = 1;

    void Awake()
    {
        // Get the camera on this gameObject and the defaultZoom.
        myCamera = GetComponent<Camera>(); // Actualicé la referencia aquí
        if (myCamera)
        {
            defaultFOV = myCamera.fieldOfView; // Actualicé la referencia aquí
        }
    }

    void Update()
    {
        // Update the currentZoom and the camera's fieldOfView.
        currentZoom += Input.mouseScrollDelta.y * sensitivity * .05f;
        currentZoom = Mathf.Clamp01(currentZoom);
        myCamera.fieldOfView = Mathf.Lerp(defaultFOV, maxZoomFOV, currentZoom); // Actualicé la referencia aquí
    }
}

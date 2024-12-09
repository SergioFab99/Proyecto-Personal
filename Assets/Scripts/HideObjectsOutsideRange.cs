using UnityEngine;

public class HideObjectsOutsideRange : MonoBehaviour
{
    public Light flashlight;
    public float range;

    void Update()
    {
        Renderer[] renderers = FindObjectsOfType<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            float distance = Vector3.Distance(transform.position, renderer.transform.position);
            renderer.enabled = distance <= range;
        }
    }
}
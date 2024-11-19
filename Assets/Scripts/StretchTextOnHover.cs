using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class StretchTextOnHoverTMP : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI buttonText;
    public float stretchFactor = 1.5f; // Factor de estiramiento
    public float animationSpeed = 10f; // Velocidad de la animación

    private Vector3 originalScale;

    void Start()
    {
        if (buttonText == null)
        {
            buttonText = GetComponentInChildren<TextMeshProUGUI>();
        }
        originalScale = buttonText.rectTransform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(ScaleText(stretchFactor));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(ScaleText(originalScale.x));
    }

    private System.Collections.IEnumerator ScaleText(float targetScale)
    {
        Vector3 targetVector = new Vector3(targetScale, originalScale.y, originalScale.z);
        while (Vector3.Distance(buttonText.rectTransform.localScale, targetVector) > 0.01f)
        {
            buttonText.rectTransform.localScale = Vector3.Lerp(buttonText.rectTransform.localScale, targetVector, Time.deltaTime * animationSpeed);
            yield return null;
        }
        buttonText.rectTransform.localScale = targetVector;
    }
}

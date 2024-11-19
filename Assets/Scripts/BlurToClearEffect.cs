using System.Collections;
using UnityEngine;
using TMPro;

public class BlurToClearEffect : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public float startDelay = 0f;  // Retraso antes de empezar el efecto
    public float delayBetweenLetters = 0.1f;  // Retraso entre la aparición de cada letra
    public float fadeDuration = 0.5f;  // Duración del desvanecimiento de cada letra
    public int orderInSequence = 0;  // Orden de aparición en la secuencia

    private void Start()
    {
        if (textComponent == null)
        {
            textComponent = GetComponent<TextMeshProUGUI>();
        }

        // Inicializa el texto invisible
        InitializeInvisibleText();

        // Comienza el efecto después del startDelay
        StartCoroutine(DelayedStartEffect());
    }

    private void InitializeInvisibleText()
    {
        textComponent.ForceMeshUpdate();
        TMP_TextInfo textInfo = textComponent.textInfo;
        Color32[] newVertexColors;

        // Configura cada carácter como invisible
        for (int i = 0; i < textInfo.characterCount; i++)
        {
            if (textInfo.characterInfo[i].isVisible)
            {
                int vertexIndex = textInfo.characterInfo[i].vertexIndex;
                newVertexColors = textInfo.meshInfo[textInfo.characterInfo[i].materialReferenceIndex].colors32;

                for (int j = 0; j < 4; j++)
                {
                    Color32 c = newVertexColors[vertexIndex + j];
                    c.a = 0;  // Transparencia completa para que sea invisible
                    newVertexColors[vertexIndex + j] = c;
                }
            }
        }

        // Actualiza el componente de texto para reflejar el cambio de transparencia
        textComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
    }

    private IEnumerator DelayedStartEffect()
    {
        // Espera el tiempo establecido en startDelay
        yield return new WaitForSeconds(startDelay);

        // Comienza el efecto de aparición de letras
        StartCoroutine(ShowTextLetterByLetter());
    }

    IEnumerator ShowTextLetterByLetter()
    {
        TMP_TextInfo textInfo = textComponent.textInfo;

        // Luego, desvanecer cada letra individualmente
        for (int i = 0; i < textInfo.characterCount; i++)
        {
            StartCoroutine(FadeInLetter(i));
            yield return new WaitForSeconds(delayBetweenLetters);
        }
    }

    IEnumerator FadeInLetter(int index)
    {
        TMP_TextInfo textInfo = textComponent.textInfo;
        Color32[] newVertexColors;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0, 1, elapsedTime / fadeDuration);

            if (textInfo.characterInfo[index].isVisible)
            {
                int vertexIndex = textInfo.characterInfo[index].vertexIndex;
                newVertexColors = textInfo.meshInfo[textInfo.characterInfo[index].materialReferenceIndex].colors32;

                for (int j = 0; j < 4; j++)
                {
                    Color32 c = newVertexColors[vertexIndex + j];
                    c.a = (byte)(alpha * 255);  // Desvanecer la letra
                    newVertexColors[vertexIndex + j] = c;
                }
            }

            textComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

            yield return null;
        }
    }
}

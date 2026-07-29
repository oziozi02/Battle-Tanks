using UnityEngine;
using TMPro;
using System.Collections;

public class ScorePopup : MonoBehaviour
{
    public float floatSpeed = 1f;
    public float duration = 1f;

    private TextMeshPro textMesh;

    void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
    }

    public void SetText(string text)
    {
        textMesh.text = text;
        StartCoroutine(FloatAndFade());
    }

    IEnumerator FloatAndFade()
    {
        float elapsed = 0f;
        Color startColor = textMesh.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            transform.position += Vector3.up * floatSpeed * Time.deltaTime;

            float alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
            textMesh.color = new Color(startColor.r, startColor.g, startColor.b, alpha);

            yield return null;
        }

        Destroy(gameObject);
    }
}
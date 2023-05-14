using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tooltip : MonoBehaviour
{
    public static Tooltip instance {get; private set;}

    [SerializeField] private RectTransform rectTransform, canvasRectTransform, backgroundTransform;
    [SerializeField] private TextMeshProUGUI tooltipText;
    [SerializeField] private const int DEFAULT_FONT_SIZE = 24;

    private void Awake() 
    {
        instance = this;
        HideTooltip();
    }

    private void Update() 
    {
        Vector2 anchoredPosition = Input.mousePosition / canvasRectTransform.localScale.x;

        if (anchoredPosition.x + backgroundTransform.rect.width > canvasRectTransform.rect.width)
            anchoredPosition.x = canvasRectTransform.rect.width - backgroundTransform.rect.width;
        
        if (anchoredPosition.y + backgroundTransform.rect.height > canvasRectTransform.rect.height)
            anchoredPosition.y = canvasRectTransform.rect.height - backgroundTransform.rect.height;

        rectTransform.anchoredPosition = anchoredPosition;
    }

    private void SetText(string text)
    {
        tooltipText.SetText(text);
        tooltipText.ForceMeshUpdate(true);

        Vector2 textSize = tooltipText.GetRenderedValues(false);
        Vector2 paddingSize = new Vector2(8, 8);
        backgroundTransform.sizeDelta = textSize + paddingSize;
    }

    private void SetRandomText()
    {
        string abc = "abcdefghijklmnopqrstuvwxyz\n\n\n\n\n\n\n";
        string text = "Testing...";
        for (int i = 0; i < Random.Range(5, 50); i++)
        {
            text += abc[Random.Range(0, abc.Length)];
        }
        SetText(text);
    }

    private void ShowTooltip(string text)
    {
        ShowTooltip(text, DEFAULT_FONT_SIZE);
    }

    private void ShowTooltip(string text, int fontSize)
    {
        SetText(text);
        tooltipText.fontSize = fontSize;
        gameObject.SetActive(true);
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }

    public static void ShowTooltip_Static(string text)
    {
        instance.ShowTooltip(text);
    }

    public static void ShowTooltip_Static(string text, int fontSize)
    {
        instance.ShowTooltip(text, fontSize);
    }

    public static void HideTooltip_Static()
    {
        instance.HideTooltip();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// [ExecuteInEditMode]
public class Line : MonoBehaviour
{
    [SerializeField] private Image horizontalLine, verticalLine, cornerLine;
    [SerializeField] private RectTransform horizontalGameObject, verticalGameObject, cornerObject;

    [SerializeField] public RectTransform gameObject1;
    [SerializeField] public RectTransform gameObject2;

    // void Update()
    // {
    //     SetLinePositions();
    // }

    private void Start()
    {
        SetLinePositions();
    }

    public void SetLinePositions()
    {
        if (gameObject1 == null || gameObject2 == null)
            return;
  
        horizontalGameObject.anchoredPosition = new Vector2((gameObject1.anchoredPosition.x + gameObject2.anchoredPosition.x) / 2, gameObject1.anchoredPosition.y);
        horizontalGameObject.sizeDelta = new Vector2(Mathf.Abs(gameObject1.anchoredPosition.x - gameObject2.anchoredPosition.x), 10);
        
        verticalGameObject.anchoredPosition = new Vector2(gameObject2.anchoredPosition.x, (gameObject1.anchoredPosition.y + gameObject2.anchoredPosition.y) / 2);
        verticalGameObject.sizeDelta = new Vector2(10, Mathf.Abs(gameObject2.anchoredPosition.y - gameObject1.anchoredPosition.y));

        cornerObject.anchoredPosition = new Vector2(gameObject2.anchoredPosition.x, gameObject1.anchoredPosition.y);
        cornerObject.sizeDelta = new Vector2(10, 10);
    }

    public void SetColours(Color color)
    {
        horizontalLine.color = color;
        verticalLine.color = color;
        cornerLine.color = color;
    }
}

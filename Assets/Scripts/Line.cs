using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class Line : MonoBehaviour
{
    [SerializeField] private Image horizontalLine, verticalLine;
    [SerializeField] private RectTransform horizontalGameObject, verticalGameObject;

    [SerializeField] private RectTransform gameObject1;
    [SerializeField] private RectTransform gameObject2;

    void Update()
    {
        if (gameObject1 != null && gameObject2 != null)
        {
            Debug.Log($"{new Vector2(Mathf.Abs(gameObject1.transform.position.x - gameObject2.transform.position.x), 10)}");
            Debug.Log($"{Mathf.Abs(gameObject1.transform.position.x - gameObject2.transform.position.x)}");
            Debug.Log($"{gameObject1.transform.position.x - gameObject2.transform.position.x}");
            Debug.Log($"RectTransform: {gameObject1.rect}");
            Debug.Log($"Before: {gameObject1.transform.position.x} After: {gameObject1.rect.position.x}");
            Debug.Log($"{gameObject2.transform.position.x}");
            
            horizontalGameObject.sizeDelta = new Vector2(Mathf.Abs(gameObject1.transform.position.x - gameObject2.transform.position.x), 10);
            horizontalGameObject.transform.position = new Vector3((gameObject1.transform.position.x + gameObject2.transform.position.y) / 2, gameObject1.transform.position.y);
            verticalGameObject.transform.position = gameObject2.transform.position;
        }
    }
}

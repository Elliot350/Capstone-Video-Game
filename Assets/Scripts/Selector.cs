using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Selector : MonoBehaviour
{
    private Camera cam;
    public static Selector instance;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3Int GetCurTilePosition()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return new Vector3Int(0, 0, 0);
        
        // Plane plane = new Plane(Vector3.up, Vector3.zero);
        // Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        // float rayOut = 0.0f;

        // if (plane.Raycast(cam.ScreenPointToRay(Input.mousePosition), out rayOut))
        // {
        //     Vector3 newPos = ray.GetPoint(rayOut) - new Vector3(0.5f, 0.0f, 0.5f);
        //     return new Vector3(Mathf.CeilToInt(newPos.x), 0, Mathf.CeilToInt(newPos.z));
        // }

        return new Vector3Int((int) Mathf.Round(cam.ScreenToWorldPoint(Input.mousePosition).x), (int) Mathf.Round(cam.ScreenToWorldPoint(Input.mousePosition).y), 0);


        // return new Vector2(0, 0);
    }
}

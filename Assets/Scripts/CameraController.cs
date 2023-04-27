using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private float zoomStep, minZoom, maxZoom;
    [SerializeField] private SpriteRenderer mapRenderer;
    private float mapMaxX, mapMinX, mapMaxY, mapMinY;
    private Vector3 dragOrigin;

    public int mouseButton = 1;

    // Update is called once per frame
    void FixedUpdate()
    {
        PanCamera();
        ScrollCamera();
    }

    private void PanCamera()
    {
        // Debug.Log($"{cam.ScreenToWorldPoint(Input.mousePosition)}");
        // Save the position of mouse in world space when drag starts (first time clicked)
        if (Input.GetMouseButtonDown(mouseButton))
        {
            dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
        }
        
        // Calculate distance between drag origin and new position if it is still held down
        if (Input.GetMouseButton(mouseButton))
        {
            Vector3 difference = dragOrigin - cam.ScreenToWorldPoint(Input.mousePosition);

            // Debug.Log($"Origin {dragOrigin} New Position {cam.ScreenToWorldPoint(Input.mousePosition)} = Difference {difference}");

            // Move the camera by that distance
            cam.transform.position = ClampCamera(cam.transform.position + difference);
        }

    }

    private void ScrollCamera()
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            ZoomIn();
        }
        
        if (Input.mouseScrollDelta.y < 0)
        {
            ZoomOut();
        }
    }

    public void ZoomIn()
    {
        float newSize = cam.orthographicSize - zoomStep;
        cam.orthographicSize = Mathf.Clamp(newSize, minZoom, maxZoom);

        cam.transform.position = ClampCamera(cam.transform.position);
    }

    public void ZoomOut()
    {
        float newSize = cam.orthographicSize + zoomStep;
        cam.orthographicSize = Mathf.Clamp(newSize, minZoom, maxZoom);
    
        cam.transform.position = ClampCamera(cam.transform.position);
    }

    private Vector3 ClampCamera(Vector3 targetPosition)
    {
        mapMinX = mapRenderer.transform.position.x - mapRenderer.bounds.size.x / 2f;
        mapMaxX = mapRenderer.transform.position.x + mapRenderer.bounds.size.x / 2f;
    
        mapMinY = mapRenderer.transform.position.y - mapRenderer.bounds.size.y / 2f;
        mapMaxY = mapRenderer.transform.position.y + mapRenderer.bounds.size.y / 2f;

        float camHeight = cam.orthographicSize;
        float camWidth = cam.orthographicSize * cam.aspect;

        float minX = mapMinX + camWidth;
        float maxX = mapMaxX - camWidth;
        float minY = mapMinY + camHeight;
        float maxY = mapMaxY - camHeight;

        return new Vector3(Mathf.Clamp(targetPosition.x, minX, maxX), Mathf.Clamp(targetPosition.y, minY, maxY), -10);
    }
}

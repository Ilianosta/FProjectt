using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera mainCamera;

    // PAN 
    private Vector3 dragOrigin;
    private Vector3 cameraStartPosition;

    // ZOOM 
    [SerializeField] private float zoomStep, minCamSize, maxCamSize;

    // LIMITS
    [SerializeField] private SpriteRenderer limitSpriteRenderer;
    private float limitMinX, limitMaxX, limitMinY, limitMaxY;

    private void Awake()
    {
        mainCamera = Camera.main;

        limitMinX = limitSpriteRenderer.transform.position.x - limitSpriteRenderer.bounds.size.x / 2;
        limitMaxX = limitSpriteRenderer.transform.position.x + limitSpriteRenderer.bounds.size.x / 2;

        limitMinY = limitSpriteRenderer.transform.position.y - limitSpriteRenderer.bounds.size.y / 2;
        limitMaxY = limitSpriteRenderer.transform.position.y + limitSpriteRenderer.bounds.size.y / 2;
    }

    private void Start()
    {
        cameraStartPosition = mainCamera.transform.position;
    }

    private void LateUpdate()
    {
        PanCamera();

        if (Input.mouseScrollDelta.y > 0) ZoomIn();
        else if (Input.mouseScrollDelta.y < 0) ZoomOut();
    }

    private void PanCamera()
    {
        if (Input.GetMouseButtonDown(0)) dragOrigin = (mainCamera.ScreenToWorldPoint(Input.mousePosition));

        if (Input.GetMouseButton(0))
        {
            Vector3 difference = dragOrigin - mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mainCamera.transform.position = ClampCamera(mainCamera.transform.position + difference);
        }

        if (Input.GetMouseButtonDown(1)) mainCamera.transform.position = cameraStartPosition;
    }

    private void ZoomIn()
    {
        float newSize = mainCamera.orthographicSize - zoomStep;
        mainCamera.orthographicSize = Mathf.Clamp(newSize, minCamSize, maxCamSize);
        mainCamera.transform.position = ClampCamera(mainCamera.transform.position);
    }
    private void ZoomOut()
    {
        float newSize = mainCamera.orthographicSize + zoomStep;
        mainCamera.orthographicSize = Mathf.Clamp(newSize, minCamSize, maxCamSize);

        mainCamera.transform.position = ClampCamera(mainCamera.transform.position);
    }

    private Vector3 ClampCamera(Vector3 targetPosition)
    {
        float camHeight = mainCamera.orthographicSize;
        float camWidht = mainCamera.orthographicSize * mainCamera.aspect;

        float minX = limitMinX + camWidht;
        float maxX = limitMaxX - camWidht;

        float minY = limitMinY + camHeight;
        float maxY = limitMaxY - camHeight;

        float newX = Mathf.Clamp(targetPosition.x, minX, maxX);
        float newY = Mathf.Clamp(targetPosition.y, minY, maxY);

        return new Vector3(newX, newY, targetPosition.z);
    }
}

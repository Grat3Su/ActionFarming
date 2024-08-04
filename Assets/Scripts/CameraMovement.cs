using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform target;  // 카메라가 따라갈 대상 (캐릭터)
    public Vector3 offset;    // 카메라와 캐릭터 사이의 거리

    // 카메라의 최소 및 최대 경계 설정
    public Vector3 minCameraBound;
    public Vector3 maxCameraBound;

    public float zoomSpeed = 10f;
    public float minZoom = 5f;
    public float maxZoom = 15f;

    Camera mainCamera;

    void LateUpdate()
    {
        Vector3 desiredPosition = target.position + offset;

        // 새로운 위치가 맵 경계를 벗어나지 않도록 제한
        desiredPosition.x = Mathf.Clamp(desiredPosition.x, minCameraBound.x, maxCameraBound.x);
        desiredPosition.y = Mathf.Clamp(desiredPosition.y, minCameraBound.y, maxCameraBound.y);
        desiredPosition.z = -10;

        transform.position = desiredPosition;

        mainCamera = this.GetComponent<Camera>();
    }

    void Update()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        if (scrollInput != 0)
        {
            mainCamera.orthographicSize -= scrollInput * zoomSpeed;
            mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize, minZoom, maxZoom);
        }
    }
}

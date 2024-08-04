using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform target;  // ī�޶� ���� ��� (ĳ����)
    public Vector3 offset;    // ī�޶�� ĳ���� ������ �Ÿ�

    // ī�޶��� �ּ� �� �ִ� ��� ����
    public Vector3 minCameraBound;
    public Vector3 maxCameraBound;

    void LateUpdate()
    {
        Vector3 desiredPosition = target.position + offset;

        // ���ο� ��ġ�� �� ��踦 ����� �ʵ��� ����
        desiredPosition.x = Mathf.Clamp(desiredPosition.x, minCameraBound.x, maxCameraBound.x);
        desiredPosition.y = Mathf.Clamp(desiredPosition.y, minCameraBound.y, maxCameraBound.y);
        desiredPosition.z = -10;

        transform.position = desiredPosition;
    }
}

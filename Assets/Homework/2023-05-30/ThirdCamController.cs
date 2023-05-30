using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdCamController : MonoBehaviour
{
    [SerializeField] Transform cameraRoot;
    [SerializeField] float cameraSensitivity;
    [SerializeField] float lookDistance;

    private Vector2 lookDelta;
    private float xRotation;
    private float yRotation;

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
    }
    private void Update()
    {
        Rotate();
    }

    private void LateUpdate()
    {
        Look();
    }
    private void Rotate()
    {
        // �����ִ� ��ü�� ��ġ
        Vector3 lookPoint = Camera.main.transform.position + Camera.main.transform.forward * lookDistance;
        // �����ִ� ������ �������� ������ ������
        lookPoint.y = transform.position.y;
        // ������Ʈ ����
        transform.LookAt(lookPoint);
    }
    private void Look()
    {
        // ����
        yRotation += lookDelta.x * cameraSensitivity * Time.deltaTime;
        // ������
        xRotation -= lookDelta.y * cameraSensitivity * Time.deltaTime;
        // �ִ�ġ
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        // ī�޶� ��Ʈ�� ȸ���ϴ½�����
        cameraRoot.rotation = Quaternion.Euler(xRotation, yRotation, 0);
    }
    private void OnLook(InputValue value)
    {
        lookDelta = value.Get<Vector2>();
    }
}

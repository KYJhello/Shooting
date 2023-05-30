using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FirstCamController : MonoBehaviour
{
    [SerializeField] float mouseSensitivity;
    [SerializeField] Transform cameraRoot;

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
    private void LateUpdate()
    {
        Look();
    }
    private void Look()
    {
        yRotation += lookDelta.x * mouseSensitivity * Time.deltaTime;
        xRotation -= lookDelta.y * mouseSensitivity * Time.deltaTime;
        // 화면 돌아가는거 제한 두기
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        cameraRoot.localRotation = Quaternion.Euler(xRotation, 0, 0);
        transform.localRotation = Quaternion.Euler(0, yRotation, 0);
    }
    private void OnLook(InputValue value)
    {
        lookDelta = value.Get<Vector2>();
    }

}

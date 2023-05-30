using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMoverMK2 : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpSpeed;

    private CharacterController controller;
    private Vector3 moveDir;
    // 점프 및 중력 구현을 위한 ySpeed
    private float ySpeed = 0;



    private void Awake()
    {
        controller = GetComponent<CharacterController>();

    }

    private void Update()
    {
        Move();
        Jump();
        
    }
    private void Move()
    {
        controller.Move(transform.forward * moveDir.z * moveSpeed * Time.deltaTime);
        controller.Move(transform.right * moveDir.x * moveSpeed * Time.deltaTime);
    }
    private void Jump()
    {
        ySpeed += Physics.gravity.y * Time.deltaTime;

        // 바닥에 있고 하강중이라면
        if (IsGrounded() && ySpeed < 0)
        {
            ySpeed = -1;
        }

        controller.Move(Vector3.up * ySpeed * Time.deltaTime);
    }
    private void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();
        moveDir = new Vector3(input.x, 0, input.y);
    }
    private void OnJump(InputValue value)
    {
        if (IsGrounded())
        {
            ySpeed = jumpSpeed;
        }
    }

    private bool IsGrounded()
    {
        RaycastHit hit;
        return Physics.SphereCast(transform.position + Vector3.up * 1f, 
            0.5f, Vector3.down, out hit, 0.6f);
    }
}

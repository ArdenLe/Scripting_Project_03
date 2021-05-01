using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController: MonoBehaviour
{
    public CharacterController controller;
    public Animator anim;
    public Transform cam;
    public float baseSpeed = 6f;
    public float runSpeed = 12f;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    bool moving = false;
    bool sprinting = false;
    float currentSpeed;

    private void Awake()
    {
        currentSpeed = baseSpeed;
    }

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * currentSpeed * Time.deltaTime);

            moving = true;
            anim.SetBool("isWalking", true);
        }
        else
        {
            moving = false;
            anim.SetBool("isWalking", false);
        }

        sprinting = Input.GetKey(KeyCode.LeftShift);
        if (sprinting && moving)
        {
            currentSpeed = runSpeed;
            anim.SetBool("isRunning", true);
        }
        if (!sprinting || !moving)
        {
            currentSpeed = baseSpeed;
            anim.SetBool("isRunning", false);
        }

    }
}

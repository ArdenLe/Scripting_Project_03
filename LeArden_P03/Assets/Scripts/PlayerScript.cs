using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [Header("References")]
    public CharacterController controller;
    public Animator anim;
    public Transform cam;
    public GameObject art1;
    public GameObject art2;
    public AudioClip dashSFX;
    public ParticleSystem dashVFX;
    public ParticleSystem dashTrail;
    [Header("Adjustables")]
    public float baseSpeed = 6f;
    public float runSpeed = 12f;
    public float dashSpeed = 15f;
    public float dashTime = 0.3f;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    bool moving = false;
    bool sprinting = false;
    public bool dashing = false;
    float currentSpeed;
    AudioSource _as = null;

    private void Awake()
    {
        currentSpeed = baseSpeed;
        _as = GetComponent<AudioSource>();
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

        if (Input.GetKeyDown(KeyCode.Mouse1) && dashing == false)
        {
            StartCoroutine(Dash());
            StartCoroutine(DashAnimation());
            sprinting = true;
            _as.PlayOneShot(dashSFX, _as.volume);
        }

        if (sprinting && moving)
        {
            currentSpeed = runSpeed;
            anim.SetBool("isRunning", true);
        }
        if (!sprinting || !moving)
        {
            currentSpeed = baseSpeed;
            anim.SetBool("isRunning", false);
            sprinting = false;
        }
    }

    IEnumerator Dash()
    {
        float startTime = Time.time;

        while (Time.time < startTime + dashTime)
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * dashSpeed * Time.deltaTime);

            yield return null;
        }

    }

    IEnumerator DashAnimation()
    {
        anim.SetBool("isDashing", true);
        dashing = true;
        art1.SetActive(false);
        art2.SetActive(false);

        yield return new WaitForSeconds(dashTime);

        dashVFX.Play();
        dashTrail.Play();
        anim.SetBool("isDashing", false);
        dashing = false;
        art1.SetActive(true);
        art2.SetActive(true);
    }

    
}

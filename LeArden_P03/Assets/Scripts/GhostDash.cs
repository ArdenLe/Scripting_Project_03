using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostDash : MonoBehaviour
{
    PlayerController playerScript;

    public float dashSpeed;
    public float dashTime;

    void Start()
    {
        playerScript = GetComponent<PlayerController>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            StartCoroutine(Dash());
        }
    }

    IEnumerator Dash()
    {
        float startTime = Time.time;

        while (Time.time < startTime + dashTime)
        {
            playerScript.controller.Move(playerScript.moveDir * dashSpeed * Time.deltaTime);

            yield return null;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public Collider bodyCollider;
    private void OnTriggerStay(Collider other)
    {
        PlayerScript playerScript =
            other.gameObject.GetComponent<PlayerScript>();
        if (playerScript != null && playerScript.dashing == true)
        {
            bodyCollider.isTrigger = true;
        }
        else
        {
            bodyCollider.isTrigger = false;
        }
    }
}

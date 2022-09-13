using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Traps : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            TrapDamage(collision);
        }
    }

    private void TrapDamage(Collider2D player)
    {
        PlayerStats stats = player.GetComponent<PlayerStats>();

        stats.TakeDamage(999);
        Debug.Log("Trapped");
    }
}

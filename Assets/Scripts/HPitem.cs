using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPitem : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Pickup(collision);
        }
    }

    private void Pickup(Collider2D player)
    {
        PlayerStats stats = player.GetComponent<PlayerStats>();

        stats.maxHealth += 50;

        stats.hpRegenItem = true;

        GetComponent<Collider2D>().enabled = false;
        GetComponent<Animator>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
    }
}

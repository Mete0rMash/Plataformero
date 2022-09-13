using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerStats : MonoBehaviour
{
	public float maxHealth = 200f;
    public bool hpRegenItem = false;
	public float currentHealth;
    private bool isRegenHealth;
    private Animator anim;
    public UnityEngine.UI.Slider healthBar;

    public GameManager gm;

    private void Awake()
    {
        anim = this.GetComponent<Animator>();

        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (hpRegenItem)
        {
            if(currentHealth != maxHealth && !isRegenHealth) StartCoroutine(RegenHP());
        }

        healthBar.value = currentHealth;
    }

    private IEnumerator RegenHP()
    {
        isRegenHealth = true;
        while (currentHealth < maxHealth && currentHealth != 0)
        {
            currentHealth += 5;
            yield return new WaitForSeconds(5);
        }
        isRegenHealth = false;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        anim.SetTrigger("player_hit");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        anim.SetTrigger("player_death");

        GetComponent<Collider2D>().enabled = false;
        GetComponent<CharacterController2D>().enabled = false;
        GetComponent<Combo>().enabled = false;
        GetComponent<PlayerMovement>().enabled = false;

        gm.EndGame();
    }
}

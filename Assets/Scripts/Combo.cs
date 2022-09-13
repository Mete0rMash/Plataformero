using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combo : MonoBehaviour
{
    public Animator anim;


    public int noOfClicks = 0;


    float lastClickedTime = 0f;
    public float maxComboDelay = 0.9f;

    private DamagePlayer damage;
    public GameObject hitbox;


    void Awake()
    {
        anim = GetComponent<Animator>();

        damage = hitbox.GetComponent<DamagePlayer>();
    }


    void Update()
    {
        if (Time.time - lastClickedTime > maxComboDelay)
        {
            noOfClicks = 0;
        }


        if (Input.GetMouseButtonDown(0))
        {
            lastClickedTime = Time.time;
            noOfClicks++;


            if (noOfClicks == 1)
            {
                anim.SetBool("attack1", true);
            }


            noOfClicks = Mathf.Clamp(noOfClicks, 0, 2);
        }
    }


    public void return1()
    {
        if (noOfClicks >= 2)
        {
            anim.SetBool("attack2", true);
        }
        else
        {
            anim.SetBool("attack1", false);
            noOfClicks = 0;
        }
    }

    public void return2()
    {
        anim.SetBool("attack1", false);
        anim.SetBool("attack2", false);
        noOfClicks = 0;
    }

    public void ChangeDamage(int newDMG)
    {
        damage.damage = newDMG;
    }
}

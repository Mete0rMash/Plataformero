using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    bool gameHasEnded = false;

    public GameObject gameOverUI;

    public GameObject tbcUI;

    public void EndGame()
    {
        if (!gameHasEnded)
        {
            gameHasEnded = true;
            GameOver();
        }
    }

    public void GameOver()
    {
        gameOverUI.SetActive(true);
    }

    public void TBC()
    {
        tbcUI.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.GetComponent<Collider2D>().enabled = false;
            collision.GetComponent<CharacterController2D>().enabled = false;
            collision.GetComponent<Combo>().enabled = false;
            collision.GetComponent<PlayerMovement>().enabled = false;

            TBC();
        }
    }
}

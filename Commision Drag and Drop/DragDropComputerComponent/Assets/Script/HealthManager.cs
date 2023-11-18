using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    [SerializeField]
    private Image[] heart;
    [SerializeField]
    private GameObject gameOverPanel;
    [SerializeField]
    private GameObject winningPanel;
    [SerializeField]
    private AudioSource hurtSfx;
    [SerializeField]
    private AudioSource correctSfx;
    private int health = 3;
    int correctDrops = 0;

    public void decreaseHeart()
    {
        if (health <= 0)
        {
            gameOver();
            return;
        }
        hurtSfx.Play();
        health--;
        GameObject.Find($"Heart ({health})").GetComponent<Image>().color = new Color32(0,0,0, 230);
        GameObject.Find("Main Camera").GetComponent<cameraShake>().StartCoroutine("Shaking");
    }
    public void increaseComponentDropped()
    {
        correctSfx.Play();
        correctDrops++;

        if (correctDrops >= 7)
        {
            winning();
        }
    }
    private void gameOver()
    {
        gameOverPanel.SetActive(true);
    }
    private void winning()
    {
        winningPanel.SetActive(true);
    }
    public void restartBtn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

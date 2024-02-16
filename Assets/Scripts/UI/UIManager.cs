using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text ScoreText;
    public GameObject gameOverPanel;
    public GameObject leaderboardPanel;
    private void OnEnable()
    {
        EventHandler.GetPointEvent += OnGetPointEvent;
        EventHandler.GameOverEvent += OnGameOverEvent;
    }

    private void OnDisable()
    {
        Time.timeScale = 1;
        EventHandler.GetPointEvent -= OnGetPointEvent;
        EventHandler.GameOverEvent -= OnGameOverEvent;
    }


    private void Start()
    {
        ScoreText.text = "000";
    }

    private void OnGetPointEvent(int point)
    {
        ScoreText.text = point.ToString();
    }
    private void OnGameOverEvent()
    {
        gameOverPanel.SetActive(true);

        if (gameOverPanel.activeInHierarchy)
        {
            Time.timeScale = 0;
        }


    }
#region 按钮添加方法
    public void RestartGame()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        gameOverPanel.SetActive(false);
        TransitioniManager.instance.Transition("Gameplay");
    }

    public void BackToMenu()
    {
        gameOverPanel.SetActive(false);
        TransitioniManager.instance.Transition("Title");
    }

    public void OpenLeaderBoard()
    {
        leaderboardPanel.SetActive(true);
    }

#endregion 
}

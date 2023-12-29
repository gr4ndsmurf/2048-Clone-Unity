using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SocialPlatforms.Impl;

public class UpperUI : MonoBehaviour
{
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text bestScoreText;
    [SerializeField] Button newGameButton;

    [SerializeField] private CanvasGroup gameOverCanvasGroup;

    private void Awake()
    {
        newGameButton.onClick.AddListener(() =>
        {
            GameManager.Instance.NewGame();
            gameOverCanvasGroup.alpha = 0f;
            gameOverCanvasGroup.interactable = false;
        });
    }

    private void Start()
    {
        LoadBestScoreText();

        GameManager.Instance.OnScoreUpdate += GameManager_OnScoreUpdate;
        GameManager.Instance.OnBestScoreUpdate += GameManager_OnBestScoreUpdate;
    }

    private void GameManager_OnBestScoreUpdate()
    {
        LoadBestScoreText();
    }

    private void GameManager_OnScoreUpdate(int score)
    {
        scoreText.text = score.ToString();
    }

    private void LoadBestScoreText()
    {
        bestScoreText.text = GameManager.Instance.LoadHighScore().ToString();
    }
}

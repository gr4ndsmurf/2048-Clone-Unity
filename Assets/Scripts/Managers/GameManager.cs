using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {  get; private set; }

    public event Action OnGameOver;
    public event Action<int> OnScoreUpdate;
    public event Action OnBestScoreUpdate;

    [SerializeField] private Board board;

    private int score;
    private int bestScore;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        NewGame();
    }

    public void NewGame()
    {
        SetScore(0);
        OnBestScoreUpdate?.Invoke();

        board.ClearBoard();
        board.CreateTile();
        board.CreateTile();

        board.enabled = true;
    }

    public void GameOver()
    {
        board.enabled = false;
        OnGameOver?.Invoke();
    }

    public void IncreaseScore(int points)
    {
        SetScore(score + points);
    }

    private void SetScore(int score)
    {
        this.score = score;
        OnScoreUpdate?.Invoke(score);
        SaveBestScore();
    }

    private void SaveBestScore()
    {
        bestScore = LoadHighScore();

        if (score > bestScore)
        {
            PlayerPrefs.SetInt(Consts.SaveValues.BEST_SCORE, score);
        }
    }

    public int LoadHighScore()
    {
        return PlayerPrefs.GetInt(Consts.SaveValues.BEST_SCORE, 0);
    }
}

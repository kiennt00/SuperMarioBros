using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameplay : UICanvas
{
    [SerializeField] Text textScore;
    [SerializeField] Text textCoins;
    [SerializeField] Text textWorld;
    [SerializeField] Text textTime;
    [SerializeField] Text textLives;

    private int _score = 0, _coins = 0, _time = 400, _lives = 10;

    private void Start()
    {
        InvokeRepeating("ReduceTime", 1f, 1f);
    }

    private void Update()
    {
        if (_time <= 0)
        {
            TimeOut();
        }
    }

    void ReduceTime()
    {
        _time--;
        textTime.text = _time.ToString();
    }

    void TimeOut()
    {
        RemoveLives(1);
    }

    public override void Open()
    {
        base.Open();
    }

    public override void Close(float delayTime)
    {
        base.Close(delayTime);
    }

    public void AddScore(int score)
    {
        _score += score;
        textScore.text = _score.ToString();
    }

    public void AddCoins(int coins)
    {
        _coins += coins;
        textCoins.text = _coins.ToString();
    }

    public void AddLives(int lives)
    {
        _lives += lives;
        textLives.text = _lives.ToString();
    }

    public void RemoveLives(int lives)
    {
        _lives -= lives;
        textLives.text = _lives.ToString();
    }
}

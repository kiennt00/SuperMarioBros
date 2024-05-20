using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameplay : UICanvas
{
    [SerializeField] Text textScore;
    private int _score = 0;

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


}

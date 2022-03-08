using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI score;
    [SerializeField] TextMeshProUGUI hiScore;
    [SerializeField] TextMeshProUGUI speed;

    void Start()
    {
        this.hiScore.text = PlayerPrefs.GetInt("score").ToString();
    }

    public void SetScore(int score)
    {
        int currentScore = (score + ConvertStringToInt(this.score.text));
        this.score.text = currentScore.ToString();
        SetHiScore(currentScore);
    }

    void SetHiScore(int score)
    {
        if (score > PlayerPrefs.GetInt("score"))
        {
            PlayerPrefs.SetInt("score", score);
            this.hiScore.text = score.ToString();
        }
    }

    public void SetSpeed()
    {
        this.speed.text = (ConvertStringToInt(speed.text) + 1).ToString();
    }

    int ConvertStringToInt(string stringNumber)
    {
        int score;
        int.TryParse(stringNumber, out score);
        return score;
    }
}

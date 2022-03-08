using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HUDController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _score;
    [SerializeField] TextMeshProUGUI _hiScore;
    [SerializeField] TextMeshProUGUI _goal;
    [SerializeField] TextMeshProUGUI _speed;
    [SerializeField] GameObject _panelGameover;
    [SerializeField] TextMeshProUGUI _pressEnterKey;
    [SerializeField] float _delayBlink;

    void Start()
    {
        _panelGameover.SetActive(false);
        this._hiScore.text = PlayerPrefs.GetInt("score").ToString();
    }

    void LateUpdate()
    {
        Retry();
    }

    public void SetScore(int score)
    {
        int currentScore = (score + ConvertStringToInt(this._score.text));
        this._score.text = currentScore.ToString();
        SetHiScore(currentScore);
    }

    void SetHiScore(int score)
    {
        if (score > PlayerPrefs.GetInt("score"))
        {
            PlayerPrefs.SetInt("score", score);
            this._hiScore.text = score.ToString();
        }
    }

    public void SetSpeed()
    {
        this._speed.text = (ConvertStringToInt(_speed.text) + 1).ToString();
    }

    public void SetGoal(string goal)
    {
        this._goal.text = goal;
    }

    int ConvertStringToInt(string stringNumber)
    {
        int score;
        int.TryParse(stringNumber, out score);
        return score;
    }

    public void GameOver()
    {
        _panelGameover.SetActive(true);
        StartCoroutine(Blink());
    }

    IEnumerator Blink()
    {
        _pressEnterKey.enabled = true;
        yield return new WaitForSeconds(_delayBlink);
        _pressEnterKey.enabled = false;
        yield return new WaitForSeconds(_delayBlink);
        StartCoroutine(Blink());
    }

    void Retry()
    {
        if (_panelGameover.activeSelf && Input.GetKey(KeyCode.Return) || Input.GetKey("enter"))
        {
            SceneManager.LoadScene("Game");
        }
    }
}

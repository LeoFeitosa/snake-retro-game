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
    [SerializeField] TextMeshProUGUI _textGameover;
    [SerializeField] GameObject _panelStartGame;
    [SerializeField] TextMeshProUGUI _textStartGame;
    [SerializeField] float _timeToReload;
    [SerializeField] float _delayBlink;

    public bool StartGame { get; set; }

    void Start()
    {
        _panelStartGame.SetActive(true);
        _panelGameover.SetActive(false);
        StartCoroutine(Blink(_textStartGame));
        this._hiScore.text = PlayerPrefs.GetInt("score").ToString();
    }

    void LateUpdate()
    {
        HidePanelStart();
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
        StartCoroutine(Reload());
    }

    IEnumerator Reload()
    {
        if (_panelGameover.activeSelf)
        {
            yield return new WaitForSeconds(_timeToReload);
            SceneManager.LoadScene("Game");
        }
    }

    public void HidePanelStart()
    {
        if (_panelStartGame.activeSelf && (Input.GetKey(KeyCode.Return) || Input.GetKey("enter")))
        {
            StartGame = true;
            _panelStartGame.SetActive(false);
            _panelGameover.SetActive(false);
            StartCoroutine(Blink(_textGameover));
        }
    }

    IEnumerator Blink(TextMeshProUGUI txt)
    {
        txt.enabled = true;
        yield return new WaitForSeconds(_delayBlink);
        txt.enabled = false;
        yield return new WaitForSeconds(_delayBlink);
        StartCoroutine(Blink(txt));
    }
}

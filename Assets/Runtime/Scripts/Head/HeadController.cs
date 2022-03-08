using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadController : MonoBehaviour
{
    SpawnController _spawnBody;
    GameObject _bodyInGame;
    List<Transform> _bodyList;
    Vector2 _direction = Vector2.zero;
    bool _dead = false;
    float _delayStart;

    [Header("Movement limit added to spawn limit")]
    [SerializeField] float _limitsExtra;

    [Header("Body")]
    [SerializeField] GameObject _bodyPrefab;
    [SerializeField] float _timeToMove = 0.3f;
    [SerializeField] float _timeLapse = 0.1f;
    [SerializeField] float _timeDelayButton = 0.8f;
    float _timeButton = 0f;

    [Header("SFX")]
    [SerializeField] AudioClip _audioDie;
    [SerializeField] float _dieVolume = 1f;
    [SerializeField] AudioClip _audioEat;
    [SerializeField] float _eatVolume = 1f;
    [SerializeField] AudioClip _audioMovement;
    [SerializeField] float _movementVolume = 1f;
    [SerializeField] AudioClip _audioOpening;
    [SerializeField] float _openingVolume = 1f;
    [SerializeField] AudioClip _audioSpeedUp;
    [SerializeField] float _speedUpVolume = 1f;

    void Start()
    {
        _spawnBody = FindObjectOfType<SpawnController>();
        _bodyInGame = _spawnBody.GenerateBody();
        _direction = Vector2.left;

        _bodyList = new List<Transform>();
        _bodyList.Add(this.transform);

        StartCoroutine(Initialize());

        AudioController.Instance.PlayMusic(_audioOpening, _openingVolume);
    }

    void Update()
    {
        SetDirection();
    }

    IEnumerator Initialize()
    {
        yield return new WaitForSeconds(_audioOpening.length);
        StartCoroutine(Move());
    }

    void SetDirection()
    {
        _timeButton += Time.deltaTime;

        if (_timeButton >= _timeDelayButton)
        {
            _timeButton = 0;

            if (Input.GetKey(KeyCode.W) && _direction != Vector2.down)
                _direction = Vector2.up;
            else if (Input.GetKey(KeyCode.S) && _direction != Vector2.up)
                _direction = Vector2.down;
            else if (Input.GetKey(KeyCode.A) && _direction != Vector2.right)
                _direction = Vector2.left;
            else if (Input.GetKey(KeyCode.D) && _direction != Vector2.left)
                _direction = Vector2.right;
        }
    }

    IEnumerator Move()
    {
        AudioController.Instance.PlayAudioCue(_audioMovement, _movementVolume);

        CheckCollisionWithYourself();
        UpdatePositionBody();

        transform.position = new Vector2(
             transform.position.x + (_direction.x / 2),
             transform.position.y + (_direction.y / 2)
            );

        CheckDie();
        CheckIfAte();
        yield return new WaitForSeconds(_timeToMove);

        if (!_dead)
        {
            StartCoroutine(Move());
        }
    }

    void UpdateSpeed()
    {
        if ((_bodyList.Count - 1) % 10 == 0)
        {
            AudioController.Instance.PlayMusic(_audioSpeedUp, _speedUpVolume);
        }
    }

    void CheckIfAte()
    {
        if (_bodyInGame.transform.position == transform.position)
        {
            AudioController.Instance.PlayAudioCue(_audioEat, _eatVolume);

            CreateFood();
        }
    }

    void CreateFood()
    {
        Destroy(_bodyInGame.gameObject);
        _bodyInGame = _spawnBody.GenerateBody();

        for (int i = 0; i <= _bodyList.Count - 1; i++)
        {
            if (_bodyInGame.transform.position == _bodyList[i].position)
            {
                CreateFood();
            }
        }

        IncreaseBody();
    }

    void IncreaseBody()
    {
        Transform bodyPrefab = Instantiate(_bodyPrefab.transform);
        bodyPrefab.position = _bodyList[_bodyList.Count - 1].position;
        _bodyList.Add(bodyPrefab.transform);

        UpdateSpeed();
    }

    void UpdatePositionBody()
    {
        for (int i = _bodyList.Count - 1; i > 0; i--)
        {
            if (_bodyList[i])
            {
                _bodyList[i].position = _bodyList[i - 1].position;
            }
        }
    }

    void CheckCollisionWithYourself()
    {
        for (int i = 2; i <= _bodyList.Count - 1; i++)
        {
            if (this.transform.position == _bodyList[i].position)
            {
                Die();
            }
        }
    }

    void CheckDie()
    {
        if (!_dead &&
            (transform.position.x == _spawnBody.BoardLimitsNegative.position.x - _limitsExtra ||
            transform.position.x == _spawnBody.BoardLimitsPositive.position.x + _limitsExtra ||
            transform.position.y == _spawnBody.BoardLimitsPositive.position.y + _limitsExtra ||
            transform.position.y == _spawnBody.BoardLimitsNegative.position.y - _limitsExtra))
        {
            Die();
        }
    }

    void Die()
    {
        _dead = true;
        AudioController.Instance.PlayAudioCue(_audioDie, _dieVolume);
        _direction = Vector2.zero;
        StartCoroutine(DestroyBody(_timeLapse));
    }

    IEnumerator DestroyBody(float seconds)
    {
        if (seconds <= 0)
        {
            yield return null;
        }

        for (int i = _bodyList.Count - 1; i >= 0; i--)
        {
            yield return new WaitForSeconds(seconds);

            if (_bodyList[i].transform)
            {
                Destroy(_bodyList[i].gameObject);
                _bodyList.RemoveAt(i);
            }
        }
    }
}

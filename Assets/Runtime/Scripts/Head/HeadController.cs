using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadController : MonoBehaviour
{
    SpawnController _spawnBody;
    GameObject _bodyInGame;
    List<Transform> _bodyList;
    [SerializeField] GameObject _bodyPrefab;
    [SerializeField] float _timeToMove = 0.3f;
    Vector2 _direction = Vector2.zero;

    void Start()
    {
        _spawnBody = FindObjectOfType<SpawnController>();
        _bodyInGame = _spawnBody.GenerateBody();
        _direction = Vector2.left;

        _bodyList = new List<Transform>();
        _bodyList.Add(transform);

        StartCoroutine(Move());
    }

    void Update()
    {
        SetDirection();
    }

    private void SetDirection()
    {
        if (Input.GetKey(KeyCode.W) && _direction != Vector2.down)
            _direction = Vector2.up;
        else if (Input.GetKey(KeyCode.S) && _direction != Vector2.up)
            _direction = Vector2.down;
        else if (Input.GetKey(KeyCode.A) && _direction != Vector2.right)
            _direction = Vector2.left;
        else if (Input.GetKey(KeyCode.D) && _direction != Vector2.left)
            _direction = Vector2.right;
    }

    IEnumerator Move()
    {
        UpdatePositionBody();
        CheckIfAte();

        transform.position = new Vector2(
             transform.position.x + (_direction.x / 2),
             transform.position.y + (_direction.y / 2)
            );

        yield return new WaitForSeconds(_timeToMove);

        StartCoroutine(Move());
    }

    void CheckIfAte()
    {
        if (_bodyInGame.transform.position == transform.position)
        {
            Destroy(_bodyInGame.gameObject);
            _bodyInGame = _spawnBody.GenerateBody();

            for (int i = _bodyList.Count - 1; i > 0; i--)
            {
                if (_bodyList[i].position == _bodyInGame.transform.position)
                {
                    Destroy(_bodyInGame.gameObject);
                    _bodyInGame = _spawnBody.GenerateBody();
                    CheckIfAte();
                }
            }

            IncreaseBody();
        }
    }

    void IncreaseBody()
    {
        Transform bodyPrefab = Instantiate(_bodyPrefab.transform);
        bodyPrefab.position = _bodyList[_bodyList.Count - 1].position;
        _bodyList.Add(bodyPrefab);
    }

    void UpdatePositionBody()
    {
        for (int i = _bodyList.Count - 1; i > 0; i--)
        {
            _bodyList[i].position = _bodyList[i - 1].position;
        }
    }
}

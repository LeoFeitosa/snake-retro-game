using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadController : MonoBehaviour
{
    SpawnController _spawn;
    bool _isMoving;
    Vector2 _originPossition;
    Vector2 _targetPossition;
    [SerializeField] float _timeToMove = 0.3f;

    BodyController _bodyInGame;

    void Start()
    {
        _spawn = FindObjectOfType<SpawnController>();
        _bodyInGame = _spawn.GenerateBody();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        CheckIfAte();
    }

    private void Move()
    {
        if (!_isMoving)
        {
            if (Input.GetKey(KeyCode.W))
            {
                StartCoroutine(SetDirection(Vector2.up));
            }
            if (Input.GetKey(KeyCode.S))
            {
                StartCoroutine(SetDirection(Vector2.down));
            }
            if (Input.GetKey(KeyCode.A))
            {
                StartCoroutine(SetDirection(Vector2.left));
            }
            if (Input.GetKey(KeyCode.D))
            {
                StartCoroutine(SetDirection(Vector2.right));
            }
        }
    }

    IEnumerator SetDirection(Vector2 direction)
    {
        _isMoving = true;

        float elapsedTime = 0;

        _originPossition = transform.position;
        _targetPossition = _originPossition + (direction / 2);

        while (elapsedTime < _timeToMove)
        {
            transform.position = Vector2.Lerp(_originPossition, _targetPossition, (elapsedTime / _timeToMove));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = _targetPossition;

        _isMoving = false;
    }

    void CheckIfAte()
    {
        if (_bodyInGame.transform.position == transform.position)
        {
            Destroy(_bodyInGame.gameObject);
            _bodyInGame = _spawn.GenerateBody();
            while (_bodyInGame.transform.position == transform.position)
            {
                Destroy(_bodyInGame.gameObject);
                _bodyInGame = _spawn.GenerateBody();
            }
        }
    }
}

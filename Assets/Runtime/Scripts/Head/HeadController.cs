using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadController : MonoBehaviour
{
    SpawnController _spawnBody;
    BodyController _bodyInGame;
    Vector2 _direction = Vector2.zero;
    [SerializeField] float _timeToMove = 0.3f;

    void Start()
    {
        _spawnBody = FindObjectOfType<SpawnController>();
        _bodyInGame = _spawnBody.GenerateBody();
        _direction = Vector2.left;

        StartCoroutine(Move());
    }

    void Update()
    {
        CheckIfAte();
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
            while (_bodyInGame.transform.position == transform.position)
            {
                Destroy(_bodyInGame.gameObject);
                _bodyInGame = _spawnBody.GenerateBody();
            }
        }
    }
}

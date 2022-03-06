using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    [SerializeField] Transform _limitsPositive;
    [SerializeField] Transform _limitsNegative;
    [SerializeField] GameObject _body;

    public Transform BoardLimitsPositive => _limitsPositive.transform;
    public Transform BoardLimitsNegative => _limitsNegative.transform;

    public GameObject GenerateBody()
    {
        return Instantiate(_body, RandomPosition(), Quaternion.identity);
    }

    Vector2 RandomPosition()
    {
        Vector2 position = Vector2.zero;

        position = new Vector2(
            Mathf.RoundToInt(
                Random.Range(_limitsNegative.position.x, _limitsPositive.position.x)),
            Mathf.RoundToInt(
                Random.Range(_limitsNegative.position.y, _limitsPositive.position.y))
            );

        return position;
    }
}

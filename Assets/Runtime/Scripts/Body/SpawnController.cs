using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    [SerializeField] Transform _limits;
    [SerializeField] BodyController _body;

    public BodyController GenerateBody()
    {
        return Instantiate(_body, RandomPosition(), Quaternion.identity);
    }

    Vector2 RandomPosition()
    {
        Vector2 position = Vector2.zero;

        position = new Vector2(
            Mathf.RoundToInt(
                Random.Range(_limits.transform.position.x * -1, _limits.transform.position.x)),
            Mathf.RoundToInt(
                Random.Range(_limits.transform.position.y * -1, _limits.transform.position.y))
            );

        return position;
    }
}

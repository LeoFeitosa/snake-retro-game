using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodController : MonoBehaviour
{
    [SerializeField] float _delayBlink;

    [SerializeField] GameObject square;

    void Start()
    {
        StartCoroutine(Blink());
    }

    IEnumerator Blink()
    {
        square.SetActive(true);
        yield return new WaitForSeconds(_delayBlink);
        square.SetActive(false);
        yield return new WaitForSeconds(_delayBlink);
        StartCoroutine(Blink());
    }
}

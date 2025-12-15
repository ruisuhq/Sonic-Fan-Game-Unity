using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingBrillo : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Desvanecer());
    }

    private IEnumerator Desvanecer()
    {
        yield return new WaitForSeconds(0.35f);
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Puente : MonoBehaviour
{
    public SpriteRenderer[] Nodos;


    void Start()
    {
        Nodos = GetComponentsInChildren<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    void Update()
    {
        
    }
}

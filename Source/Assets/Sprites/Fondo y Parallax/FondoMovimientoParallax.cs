using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FondoMovimientoParallax : MonoBehaviour
{
    [SerializeField] private Vector2 velocidadMovimiento;
    private Vector2 offset;
    private Material material;
    private Rigidbody2D Sonic;
    
    
    // Start is called before the first frame update
    private void Awake()
    {
        material = GetComponent<SpriteRenderer>().material;
        Sonic = GameObject.FindGameObjectWithTag("Sonic").GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    private void Update()
    {
        offset = (Sonic.velocity.x *0.1f) * velocidadMovimiento * Time.deltaTime;
        material.mainTextureOffset += offset;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingEstatico : MonoBehaviour
{
    public GameObject RingBrillo;
    [SerializeField] private AudioClip RecolectarRing;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Sonic")
        {
            ControladorDeSonidos.Instance.EjecutarSonido(RecolectarRing);
            Instantiate(RingBrillo, transform.position, Quaternion.Euler(0, 0, 0));
            Destroy(gameObject);
        }
    }
}

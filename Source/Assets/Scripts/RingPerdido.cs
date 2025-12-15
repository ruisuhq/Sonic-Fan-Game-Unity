using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingPerdido : MonoBehaviour
{
    public GameObject RingBrillo;
    [SerializeField] private AudioClip RecolectarRing;

    public Rigidbody2D ringRb2d;

    public float Velocidad = 10f;
    

    void Start()
    {
        // Obtiene e instancia el componente Rigidbody2D.
        ringRb2d = GetComponent<Rigidbody2D>();
        // Envía inmediatamente el anillo perdido disparando hacia adelante desde su ubicación (la rotación se controla en el script SonicController_FSM).
        ringRb2d.AddForce(transform.up * Velocidad, ForceMode2D.Impulse);
        StartCoroutine(RingDesvanecer());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Sonic")
        {
            ControladorDeSonidos.Instance.EjecutarSonido(RecolectarRing);
            Instantiate(RingBrillo, transform.position, Quaternion.Euler(0, 0, 0));
            Destroy(gameObject);
        }
    }

    private IEnumerator RingDesvanecer()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}

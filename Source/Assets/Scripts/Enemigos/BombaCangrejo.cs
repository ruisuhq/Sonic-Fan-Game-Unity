using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombaCangrejo : MonoBehaviour
{
    // Declara la variable Rigidbody2D para la bomba de cangrejo.
    public Rigidbody2D rb2dBombaCangrejo;

    // Establece la velocidad de la bomba de cangrejo.
    public float velocidad = 5f;

    void Start()
    {
        // Recupera e instancia el componente Rigidbody2D.
        rb2dBombaCangrejo = GetComponent<Rigidbody2D>();
        // Envía inmediatamente la bomba de cangrejo disparando hacia adelante desde su ubicación (la rotación se controla en el script del cangrejo).
        rb2dBombaCangrejo.AddForce(transform.up * velocidad, ForceMode2D.Impulse);
    }

    void Update()
    {
        // Se destruye a sí misma después de 2 segundos cuando ha caído fuera de la pantalla, para limitar el número de bombas en la escena.
        Invoke("Autodestruccion", 2f);
    }

    private void Autodestruccion()
    {
        Destroy(gameObject);
    }
}

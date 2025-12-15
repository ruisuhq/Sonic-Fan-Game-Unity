using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonicMirandoArriba : SonicEstadoBase
{
    public override void EntrarEstado(Sonic_controlador_nuevo Sonic)
    {
        // Habilita los colliders de cápsula y caja y deshabilita el collider circular.
        Sonic.ColiderCirculo.enabled = false;
        Sonic.ColiderCapsula.enabled = true;
        // Sonic.ColiderCaja.enabled = true;

        // Activa la animación de Mirando Arriba.
        Sonic.Animar("Sonic_MirandoArriba");

        Sonic.esMortal = false;
    }

    public override void OnCollisionEnter2D(Sonic_controlador_nuevo Sonic)
    {
        // No hace nada específico en esta situación.
    }

    public override void Update(Sonic_controlador_nuevo Sonic)
    {
        if (Input.GetKeyUp("w") || Input.GetKeyUp("up"))
        {
            // Devuelve el control al script de EstadoInactivo cuando se suelta la tecla.
            Sonic.Transicion(Sonic.Parado);
        }

        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            // Establece una nueva velocidad horizontal positiva.
            Sonic.rb2d.velocity = new Vector2(Sonic.VelocidadInicial, Sonic.rb2d.velocity.y);
            // Transfiere el control al script de EstadoCorriendoDerecha.
            Sonic.Transicion(Sonic.CorriendoDerecha);
        }

        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            // Establece una nueva velocidad horizontal negativa.
            Sonic.rb2d.velocity = new Vector2(-Sonic.VelocidadInicial, Sonic.rb2d.velocity.y);
            // Transfiere el control al script de EstadoCorriendoIzquierda.
            Sonic.Transicion(Sonic.CorriendoIzquierda);
        }
    }
}

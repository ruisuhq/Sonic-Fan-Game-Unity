using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonicSaltandoDerecha : SonicEstadoBase
{
    public override void EntrarEstado(Sonic_controlador_nuevo Sonic)
    {
        //Disables Sonic's box and capsule colliders and enables his circle collider.
        //player.BoxCollider2d.enabled = false;
        Sonic.ColiderCapsula.enabled = false;
        Sonic.ColiderCirculo.enabled = true;

        //Activates the spinning animation.
        Sonic.Animar("Sonic_Girando");

        //Reverts the sprite to normal for rightward movement.
        Sonic.VoltearSprite(false);
        Sonic.esMortal = true;
    }

    public override void OnCollisionEnter2D(Sonic_controlador_nuevo Sonic)
    {
        if(Sonic.Pisando() || Sonic.PisandoGirando())
        {
            // Verifica si se están presionando las teclas de abajo al chocar con una superficie.
            if (Input.GetKey("s") || Input.GetKey("down"))
            {
                // Mantiene la misma velocidad horizontal y vertical.
                Sonic.rb2d.velocity = new Vector2(Sonic.rb2d.velocity.x, Sonic.rb2d.velocity.y);
                // Transfiere el control al script de EstadoRodandoDerecha.
                Sonic.Transicion(Sonic.RodandoDerecha);
            }
            else
            {
                // Transfiere el control al script de EstadoCorriendoDerecha.
                Sonic.Transicion(Sonic.CorriendoDerecha);
            }
        }
    }

    public override void Update(Sonic_controlador_nuevo Sonic)
    {
        // Verifica si el jugador sigue presionando la tecla de movimiento relacionada con este estado.
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            // Cuando updatedSpeed es igual a VelocidadInicial, se aumenta por el valor de aceleración multiplicado por Time.deltaTime.
            if (Sonic.VelocidadActual == Sonic.VelocidadInicial)
            {
                Sonic.VelocidadActual = Sonic.VelocidadInicial + Sonic.Aceleracion * Time.deltaTime;
            }
            // Cuando updatedSpeed está por encima de VelocidadInicial, se aumenta por el valor de aceleración multiplicado por Time.deltaTime.
            else if (Sonic.VelocidadActual > Sonic.VelocidadInicial)
            {
                Sonic.VelocidadActual += Sonic.Aceleracion * Time.deltaTime;
            }

            // Cuando updatedSpeed supera el valor de VelocidadMáxima, se reinicia a VelocidadMáxima.
            if (Sonic.VelocidadActual >= Sonic.VelocidadMaxima)
            {
                Sonic.VelocidadActual = Sonic.VelocidadMaxima;
            }

            // Da a Sonic una velocidad horizontal actualizada mientras mantiene la velocidad vertical igual.
            Sonic.rb2d.velocity = new Vector2(Sonic.VelocidadActual, Sonic.rb2d.velocity.y);
        }
        // Cuando el jugador no mantiene presionada la tecla de movimiento, Sonic se desacelera, asegurando que sus animaciones cambien con su velocidad.
        else
        {
            if (Sonic.VelocidadActual > Sonic.VelocidadInicial)
            {
                Sonic.VelocidadActual -= Sonic.Aceleracion * Time.deltaTime;
            }
        }

        // Verifica si el jugador está ingresando la tecla de movimiento opuesta.
        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            // Establece la velocidad horizontal de Sonic a un valor negativo de VelocidadInicial mientras mantiene la velocidad vertical actual.
            Sonic.rb2d.velocity = new Vector2(-Sonic.VelocidadInicial, Sonic.rb2d.velocity.y);
            // Transfiere el control al script de EstadoSaltandoIzquierda.
            Sonic.Transicion(Sonic.SaltandoIzquierda);
        }
        
    }
}

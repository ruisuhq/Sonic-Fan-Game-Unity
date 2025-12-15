using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonicCorriendoDerecha : SonicEstadoBase
{
    public override void EntrarEstado(Sonic_controlador_nuevo Sonic)
    {
        // Habilita los colliders de cápsula y caja de Sonic y deshabilita el collider circular.
        Sonic.ColiderCirculo.enabled = false;
        Sonic.ColiderCapsula.enabled = true;

        // Activa la animación de correr.
        Sonic.Animar("Sonic_Corre");

        // Deshace la inversión del sprite de Sonic para el movimiento hacia la derecha.
        Sonic.VoltearSprite(false);
        Sonic.esMortal = false;
    }
    public override void OnCollisionEnter2D(Sonic_controlador_nuevo Sonic)
    {
        
    }

    public override void Update(Sonic_controlador_nuevo Sonic)
    {
        // Ajusta la animación basándose en la velocidad de Sonic.
        Sonic.ChecarVelocidad();

        // Verifica si Sonic está en el suelo y se ha presionado el botón de salto.
        if (Sonic.Pisando() && Input.GetButtonDown("Jump"))
        {
            // Da a Sonic una nueva velocidad vertical mientras deja la velocidad horizontal igual.
            Sonic.rb2d.velocity = new Vector2(Sonic.rb2d.velocity.x, Sonic.FuerzaSalto);
            // Transfiere el control al script de EstadoSaltandoDerecha.
            Sonic.EjecutarSonido(Sonic.SonidoSalto);
            Sonic.Transicion(Sonic.SaltandoDerecha);
        }

        // Verifica si el jugador está empujando y si se presiona la tecla de movimiento hacia la derecha.
        if (Sonic.EstaEmpujando() && Input.GetAxisRaw("Horizontal") > 0)
        {
            // Transfiere el control al script de EstadoEmpujando.
            Sonic.Transicion(Sonic.EmpujandoDerecha);
        }

        // Verifica si el jugador continúa presionando la tecla de movimiento hacia la derecha.
        float direccionHorizontal = Input.GetAxisRaw("Horizontal");
        if (direccionHorizontal > 0)
        {
            // Cuando la velocidad actual es igual a la velocidad inicial, se aumenta por el valor de aceleración multiplicado por el tiempo delta.
            if (Sonic.VelocidadActual == Sonic.VelocidadInicial)
            {
                Sonic.VelocidadActual = Sonic.VelocidadInicial + Sonic.Aceleracion * Time.deltaTime;
            }
            // Cuando la velocidad actual es mayor que la velocidad inicial, se aumenta por el valor de aceleración multiplicado por el tiempo delta.
            else if (Sonic.VelocidadActual > Sonic.VelocidadInicial)
            {
                Sonic.VelocidadActual += Sonic.Aceleracion * Time.deltaTime;
            }

            // Cuando la velocidad actual supera el valor de la velocidad máxima, se restablece a la velocidad máxima.
            if (Sonic.VelocidadActual >= Sonic.VelocidadMaxima)
            {
                Sonic.VelocidadActual = Sonic.VelocidadMaxima;
            }

            // Da a Sonic una velocidad horizontal actualizada mientras mantiene la velocidad vertical igual.
            Sonic.rb2d.velocity = new Vector2(Sonic.VelocidadActual, Sonic.rb2d.velocity.y);
        }
        // Cuando el jugador no mantiene presionada la tecla de movimiento hacia la derecha, Sonic se desacelera.
        else
        {
            if (Sonic.VelocidadActual > Sonic.VelocidadInicial)
            {
                Sonic.VelocidadActual -= Sonic.Aceleracion * Time.deltaTime;
            }
        }
        if (direccionHorizontal < 0)
        {
            // Restablece la variable updatedSpeed antes de cambiar a moverse en la dirección opuesta.
            Sonic.VelocidadActual = Sonic.VelocidadInicial;
            Sonic.RigidBody2d.velocity = new Vector2(Sonic.VelocidadInicial, Sonic.RigidBody2d.velocity.y);
            // Transfiere el control al script de CorriendoIzquierda.
            Sonic.Transicion(Sonic.CorriendoIzquierda);
        }

        // Si la velocidad actual es menor o igual a la velocidad inicial, Sonic se detiene.
        if (Sonic.VelocidadActual <= Sonic.VelocidadInicial)
        {
            Sonic.VelocidadActual = Sonic.VelocidadInicial;
            // Transfiere el control al script de EstadoInactivo.
            Sonic.Transicion(Sonic.Parado);
        }

        // Si se presiona la tecla S o la tecla de flecha hacia abajo, Sonic cambia al estado de rodar hacia la derecha.
        if (Input.GetKey("s") || Input.GetKey("down"))
        {
            Sonic.EjecutarSonido(Sonic.SonidoRodar);
            Sonic.Transicion(Sonic.RodandoDerecha);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonicRodandoDerecha : SonicEstadoBase
{
    public override void EntrarEstado(Sonic_controlador_nuevo Sonic)
    {
        // Deshabilita los colliders de caja y cápsula de Sonic y habilita su collider circular.
        //player.BoxCollider2d.enabled = false;
        Sonic.ColiderCapsula.enabled = false;
        Sonic.ColiderCirculo.enabled = true;

        // Activa la animación de giro.
        Sonic.Animar("Sonic_Girando");

        // Revierte el sprite para el movimiento hacia la derecha.
        Sonic.VoltearSprite(false);

        Sonic.esMortal = true;
    }

    public override void OnCollisionEnter2D(Sonic_controlador_nuevo Sonic)
    {
    }

    public override void Update(Sonic_controlador_nuevo Sonic)
    {
        if (Sonic.PisandoGirando() && Sonic.VelocidadActual == Sonic.VelocidadInicial){
                if (Input.GetAxisRaw("Horizontal") > 0)
                {
                    Sonic.RigidBody2d.velocity = new Vector2(Sonic.VelocidadInicial, Sonic.RigidBody2d.velocity.y);
                    // Transfiere el control al script de CorriendoDerecha.
                    Sonic.Transicion(Sonic.CorriendoDerecha);
                }
                if (Input.GetKey("s") || Input.GetKey("down"))
                {
                    // Transfiere el control al script de EstadoAgachado.
                    Sonic.Transicion(Sonic.Agachado);
                }
                else 
                {
                    Sonic.Transicion(Sonic.Parado);
                }
            }

        if (Sonic.VelocidadActual > Sonic.VelocidadInicial)
            {
                Sonic.VelocidadActual -= Sonic.Aceleracion * Time.deltaTime;
            } 
            else {
                Sonic.VelocidadActual = Sonic.VelocidadInicial;
            }

        // Verifica si Sonic está en el suelo y si se presiona la barra espaciadora.
        if (Sonic.PisandoGirando() && Input.GetButtonDown("Jump"))
        {
            // Reinicia VelocidadActual a VelocidadInicial.
            Sonic.VelocidadActual = Sonic.VelocidadInicial;
            // Da a Sonic una nueva velocidad vertical mientras restablece su velocidad horizontal.
            Sonic.rb2d.velocity = new Vector2(Sonic.VelocidadInicial, Sonic.FuerzaSalto);
            // Transfiere el control al script de EstadoSaltandoDerecha.
            Sonic.EjecutarSonido(Sonic.SonidoSalto);
            Sonic.Transicion(Sonic.SaltandoDerecha);
        }

        // Verifica si se está presionando la tecla de movimiento opuesta.
        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            // Transfiere el control al script de EstadoRodandoIzquierda.
            Sonic.Transicion(Sonic.RodandoIzquierda);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonicSaltando : SonicEstadoBase
{
    public override void EntrarEstado(Sonic_controlador_nuevo Sonic)
    {
        // Deshabilita los colliders de caja y cápsula de Sonic y habilita el collider circular.
        // Sonic.ColiderCaja.enabled = false;
        Sonic.ColiderCapsula.enabled = false;
        Sonic.ColiderCirculo.enabled = true;

        // Activa la animación de giro.
        Sonic.Animar("Sonic_Girando");
        Sonic.esMortal = true;
    }

    public override void OnCollisionEnter2D(Sonic_controlador_nuevo Sonic)
    {
        // Cuando Sonic colisiona con el suelo, devuelve el control al script de EstadoInactivo.
        if(Sonic.Pisando() || Sonic.PisandoGirando()) Sonic.Transicion(Sonic.Parado);
    }

    public override void Update(Sonic_controlador_nuevo Sonic)
    {
        // Obtiene la dirección horizontal del input
        float direccionHorizontal = Input.GetAxisRaw("Horizontal");

        // Si la dirección es positiva (tecla D o derecha)
        if (direccionHorizontal > 0)
        {
            // Cuando el jugador presiona un botón para moverse hacia la derecha, Sonic recibe una nueva velocidad en x mientras su velocidad actual en y permanece igual.
            Sonic.rb2d.velocity = new Vector2(Sonic.VelocidadInicial, Sonic.rb2d.velocity.y);
            // Transfiere el control al script de EstadoSaltandoDerecha.
            Sonic.Transicion(Sonic.SaltandoDerecha);
        }

        // Si la dirección es negativa (tecla A o izquierda)
        if (direccionHorizontal < 0)
        {
            // Cuando el jugador presiona un botón para moverse hacia la izquierda, Sonic recibe una nueva velocidad en x mientras su velocidad actual en y permanece igual.
            Sonic.rb2d.velocity = new Vector2(-Sonic.VelocidadInicial, Sonic.rb2d.velocity.y);
            // Transfiere el control al script de EstadoSaltandoIzquierda.
            Sonic.Transicion(Sonic.SaltandoIzquierda);
        }
    }

}

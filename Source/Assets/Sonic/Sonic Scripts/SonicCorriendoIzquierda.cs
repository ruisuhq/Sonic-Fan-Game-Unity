using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonicCorriendoIzquierda : SonicEstadoBase
{
    public override void EntrarEstado(Sonic_controlador_nuevo Sonic)
    {
        // Habilita los colliders de cápsula y caja de Sonic y deshabilita el collider circular.
        Sonic.ColiderCirculo.enabled = false;
        Sonic.ColiderCapsula.enabled = true;

        // Activa la animación de correr.
        Sonic.Animar("Sonic_Corre");

        // Invierte el sprite de Sonic para el movimiento hacia la izquierda.
        Sonic.VoltearSprite(true);
        Sonic.esMortal = false;

    }
    public override void OnCollisionEnter2D(Sonic_controlador_nuevo Sonic)
    {
        
    }

    public override void Update(Sonic_controlador_nuevo Sonic)
    {
        // Ajusta la animación según la velocidad de Sonic.
        Sonic.ChecarVelocidad();

        // Verifica si Sonic está en el suelo y se ha presionado el botón de salto.
        if (Sonic.Pisando() && Input.GetButtonDown("Jump"))
        {
            // Da a Sonic una nueva velocidad vertical mientras deja la velocidad horizontal igual.
            Sonic.rb2d.velocity = new Vector2(Sonic.rb2d.velocity.x, Sonic.FuerzaSalto);
            // Transfiere el control al script de EstadoSaltandoIzquierda.
            Sonic.EjecutarSonido(Sonic.SonidoSalto);
            Sonic.Transicion(Sonic.SaltandoIzquierda);
        }

        // Obtiene la dirección horizontal del input
        float direccionHorizontal = Input.GetAxisRaw("Horizontal");

        if (Sonic.EstaEmpujando() && Input.GetAxisRaw("Horizontal") < 0)
        {
            // Transfiere el control al script de EstadoEmpujando.
            Sonic.Transicion(Sonic.EmpujandoIzquierda);
        }

        // Si la dirección es negativa (tecla A o izquierda)
        if (direccionHorizontal < 0)
        {   
            // Si la velocidad actual es igual a la velocidad inicial
            if (Sonic.VelocidadActual == Sonic.VelocidadInicial)
            {
                Sonic.VelocidadActual = Sonic.VelocidadInicial + Sonic.Aceleracion * Time.deltaTime;
            }
            // Si la velocidad actual es mayor que la velocidad inicial
            else if (Sonic.VelocidadActual > Sonic.VelocidadInicial)
            {
                Sonic.VelocidadActual += Sonic.Aceleracion * Time.deltaTime;
            }
            
            // Si la velocidad actual es mayor o igual a la velocidad máxima
            if (Sonic.VelocidadActual >= Sonic.VelocidadMaxima)
            {
                Sonic.VelocidadActual = Sonic.VelocidadMaxima;
            }

            Sonic.rb2d.velocity = new Vector2(-Sonic.VelocidadActual, Sonic.rb2d.velocity.y);
        }
        else
        {
            // Si la dirección no es negativa, Sonic se desacelera
            if (Sonic.VelocidadActual > Sonic.VelocidadInicial)
            {
                Sonic.VelocidadActual -= Sonic.Aceleracion * Time.deltaTime;
            }
        }
        
        // Si la dirección es positiva (tecla D o derecha)
        if (direccionHorizontal > 0)
        {
            Sonic.VelocidadActual = Sonic.VelocidadInicial;
            Sonic.rb2d.velocity = new Vector2(Sonic.VelocidadInicial, Sonic.rb2d.velocity.y);           
            Sonic.Transicion(Sonic.CorriendoDerecha);
        }

        // Si la velocidad actual es menor o igual a la velocidad inicial, Sonic se detiene
        if (Sonic.VelocidadActual <= Sonic.VelocidadInicial)
        {
            Sonic.VelocidadActual = Sonic.VelocidadInicial;
            Sonic.Transicion(Sonic.Parado);
        }

        // Si se presiona la tecla S o abajo, Sonic pasa al estado de rodar hacia la izquierda
        if (Input.GetKey("s") || Input.GetKey("down"))
        {
            Sonic.EjecutarSonido(Sonic.SonidoRodar);
            Sonic.Transicion(Sonic.RodandoIzquierda);
        }
    }
}

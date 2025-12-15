using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonicEmpujandoDerecha : SonicEstadoBase
{
    public override void EntrarEstado(Sonic_controlador_nuevo sonic)
    {
        // Habilita el collider de la cápsula al entrar en este estado.
        sonic.ColiderCapsula.enabled = true;
        sonic.ColiderCirculo.enabled = false;

        // Inicia la animación de Sonic_Lastimado.
        sonic.Animar("Sonic_Empujando");
        sonic.esMortal = false;
        sonic.VelocidadActual = sonic.VelocidadInicial;
    }
    public override void OnCollisionEnter2D(Sonic_controlador_nuevo sonic)
        {
        }
    public override void Update(Sonic_controlador_nuevo sonic)
    {
        if(Input.GetAxisRaw("Horizontal") > 0) sonic.VoltearSprite(false);
        if(Input.GetAxisRaw("Horizontal") <= 0f) sonic.Transicion(sonic.Parado);
        // Verifica si Sonic está en el suelo y se ha presionado el botón de salto.
        if (sonic.Pisando() && Input.GetButtonDown("Jump"))
        {
            // Da a Sonic una nueva velocidad vertical mientras deja la velocidad horizontal igual.
            sonic.rb2d.velocity = new Vector2(sonic.rb2d.velocity.x, sonic.FuerzaSalto);
            // Transfiere el control al script de EstadoSaltandoDerecha.
            sonic.EjecutarSonido(sonic.SonidoSalto);
            sonic.Transicion(sonic.Saltando);
        }
    }
}

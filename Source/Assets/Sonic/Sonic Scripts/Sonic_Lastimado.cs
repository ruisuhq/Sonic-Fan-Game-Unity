using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonicLastimado : SonicEstadoBase
{
    public override void EntrarEstado(Sonic_controlador_nuevo sonic)
    {
        // Habilita el collider de la cápsula al entrar en este estado.
        sonic.ColiderCapsula.enabled = false;
        sonic.ColiderCirculo.enabled = true;

        // Inicia la animación de Sonic_Lastimado.
        sonic.Animar("Sonic_Lastimado");
        sonic.estaLastimado = true;
        sonic.esMortal = true;
    }
    public override void OnCollisionEnter2D(Sonic_controlador_nuevo sonic)
        {
            sonic.VelocidadActual = sonic.VelocidadInicial;
            sonic.Transicion(sonic.Parado);
        }
    public override void Update(Sonic_controlador_nuevo sonic)
    {
    }
}

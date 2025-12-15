using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonicAgachado : SonicEstadoBase
{
    public override void EntrarEstado(Sonic_controlador_nuevo Sonic)
    {
        Sonic.ColiderCirculo.enabled = true;
        Sonic.ColiderCapsula.enabled = false;

        //Asegura que sonic no se mueva
        Sonic.RigidBody2d.velocity = new Vector2(0f, 0f);

        Sonic.Animar("Sonic_Agachado");

        Sonic.esMortal = false;
    }

    public override void OnCollisionEnter2D(Sonic_controlador_nuevo Sonic)
    {
        
    }

    public override void Update(Sonic_controlador_nuevo Sonic)
    {
        if (Input.GetKeyUp("s") || Input.GetKeyUp("down"))
        {
            Sonic.Transicion(Sonic.Parado);
        }
    }
}

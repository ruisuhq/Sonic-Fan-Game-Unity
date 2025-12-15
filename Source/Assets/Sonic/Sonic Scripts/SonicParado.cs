using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonicParado : SonicEstadoBase
{
    public Vector2 UltimaPosicion;
    private float TiempoSinMovimiento = 0f; 
    public override void EntrarEstado(Sonic_controlador_nuevo Sonic)
    {
        //Activa el colider capsula y desactiva el circulo para mejor precisión de colider
        Sonic.ColiderCirculo.enabled = false;
        Sonic.ColiderCapsula.enabled = true;

        //Activa la animación de Parado
        Sonic.Animar("Sonic_Parado");
        Sonic.esMortal = false;
        Sonic.VelocidadActual = Sonic.VelocidadInicial;
        
    }

    public override void OnCollisionEnter2D(Sonic_controlador_nuevo Sonic)
    {
        
    }

    public override void Update(Sonic_controlador_nuevo Sonic)
    {
        //Debug.Log("tiempo inactivo" + TiempoSinMovimiento);
        if ((Vector2)Sonic.transform.position != UltimaPosicion)
        {
                TiempoSinMovimiento = 0f;
                UltimaPosicion = Sonic.transform.position;
                
        } 
        else{
                TiempoSinMovimiento += Time.deltaTime;
            }
        if (TiempoSinMovimiento >= 5f)
        {
            // Ejecutar la animación de inactividad
            Sonic.Animar("Sonic_Esperando");
        }

        //Checa si sonic está pisando y si se precionó el botón de salto
        if (Sonic.Pisando() && Input.GetButtonDown("Jump"))
        {  
            //Impulsa a Sonic Verticalmente teniendo en cuenta su velocidad horizontal.
            Sonic.RigidBody2d.velocity = new Vector2(Sonic.RigidBody2d.velocity.x, Sonic.FuerzaSalto);
            Sonic.EjecutarSonido(Sonic.SonidoSalto);
            Sonic.Transicion(Sonic.Saltando);
        }

        float DireccionHorizontal = Input.GetAxisRaw("Horizontal");

        if (DireccionHorizontal > 0)
        {
            // Da a Sonic una velocidad positiva de VelocidadInicial mientras mantiene su velocidad vertical actual.
            Sonic.RigidBody2d.velocity = new Vector2(Sonic.VelocidadInicial*DireccionHorizontal, Sonic.RigidBody2d.velocity.y);
            // Transición al estado CorriendoDerecha.
            Sonic.Transicion(Sonic.CorriendoDerecha);
        }
        else if (DireccionHorizontal < 0)
        {
            // Da a Sonic una velocidad negativa de VelocidadInicial mientras mantiene su velocidad vertical actual.
            Sonic.RigidBody2d.velocity = new Vector2(Sonic.VelocidadInicial*DireccionHorizontal, Sonic.RigidBody2d.velocity.y);
            // Transición al estado CorriendoIzquierda.
            Sonic.Transicion(Sonic.CorriendoIzquierda);
        }


        if (Input.GetKey("s") || Input.GetKey("down"))
        {
            // Transición al script de Agachado.
            Sonic.Transicion(Sonic.Agachado);
        }

        if (Input.GetKey("w") || Input.GetKey("up"))
        {
            // Transición al script de MirandoArriba.
            Sonic.Transicion(Sonic.MirandoArriba);
        }

    }
}

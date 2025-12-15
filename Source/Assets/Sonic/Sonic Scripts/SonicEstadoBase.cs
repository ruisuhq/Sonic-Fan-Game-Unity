using UnityEngine;

//This sets up the abstract class on which all other states are based.
public abstract class SonicEstadoBase
{
    public abstract void EntrarEstado(Sonic_controlador_nuevo Sonic);
    public abstract void Update(Sonic_controlador_nuevo Sonic);
    public abstract void OnCollisionEnter2D(Sonic_controlador_nuevo Sonic);

}

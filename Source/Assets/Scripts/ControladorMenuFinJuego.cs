using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorMenuFinJuego : MonoBehaviour
{
    public ControladorSonicEscena finJuego;
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)|| Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            finJuego.ReiniciarJuego();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            finJuego.MenuPrincipal();
        }
    }
}

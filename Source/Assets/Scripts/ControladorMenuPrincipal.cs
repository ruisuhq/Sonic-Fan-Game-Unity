using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControladorMenuPrincipal : MonoBehaviour
{
    // Crea un campo público en el inspector para colocar el objeto CargadorEscena.
    public ControladorSonicEscena EmpezarJuego;

    void Update()
    {
        // Verifica si se presiona la tecla Enter
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            // Carga el siguiente nivel (Nivel 1)
            EmpezarJuego.CargarSiguienteNivel();
        }
    }
}


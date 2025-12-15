using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControladorSonicEscena : MonoBehaviour
{
    // Crea un campo público en el inspector para colocar el objeto SceneLoader.
    public CargadorEscena cargador;

    // Carga la siguiente escena en el orden de compilación de escenas.
    public void CargarSiguienteNivel()
    {
        cargador.CargarSiguienteNivel();
    }

    // Reinicia el juego cargando la escena anterior en el orden de compilación de escenas.
    public void ReiniciarJuego()
    {
        cargador.ReiniciarJuego();
    }

    // Carga el menú principal del Classic Arcade.
    public void MenuPrincipal()
    {
        cargador.CargarMenuPrincipal();
    }
    public void MenuFinJuego()
    {
        
        cargador.CargarMenuFinJuego();
    }

    public void ReiniciarNivel()
    {
        cargador.ReiniciarNivel();
    }
}


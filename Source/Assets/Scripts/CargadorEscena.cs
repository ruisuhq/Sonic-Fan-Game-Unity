using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CargadorEscena : MonoBehaviour
{
    public Animator transicion;

    public float tiempoTransicion = 1f;

    //Usar valor negativo para cargar escena anterior
    public void CargarSiguienteNivel(int indice = 1)
    {
        StartCoroutine(CargarNivel(SceneManager.GetActiveScene().buildIndex + indice));
    }
    public void CargarMenuPrincipal()
    {
        StartCoroutine(CargarNivel(0));
    }
    public void CargarMenuFinJuego()
    {
        StartCoroutine(CargarNivel(SceneManager.sceneCountInBuildSettings - 1));
    }
    // Carga el nivel basado en el índice de SceneManagment
    private IEnumerator CargarNivel(int indiceNivel)
    {
        transicion.SetTrigger("Empezar");
        yield return new WaitForSeconds(tiempoTransicion);
        SceneManager.LoadScene(indiceNivel);
    }
    public void ReiniciarNivel()
    {
        StartCoroutine(CargarNivel(SceneManager.GetActiveScene().buildIndex));
    }
    public void ReiniciarJuego()
    {
        StartCoroutine(CargarNivel(1));
    }

}

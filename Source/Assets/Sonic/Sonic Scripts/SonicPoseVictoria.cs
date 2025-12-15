using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SonicPoseVictoria : MonoBehaviour
{
    public GameObject SonicTiempo;
    public SonicTiempo TiempoNivel;

    // Crea un campo en el inspector para el canvas de fin de nivel.
    public Canvas NivelSuperado;
    public Sonic_controlador_nuevo Sonic;

    // Crea un campo en el inspector para el cargador de escenas.
    public CargadorEscena cargadorEscena;

    // Declara una variable Animator.
    private Animator animadorPoste;
    [SerializeField] private AudioClip SonidoPosteGirando;
    [SerializeField] private AudioClip SonidoNivelSuperado;

    // Booleano para asegurarse de que el evento OnTriggerEnter2D solo se active una vez.
    private bool activado = false;

    void Start()
    {
        TiempoNivel = SonicTiempo.GetComponent<SonicTiempo>();
        // Asigna el componente Animator a la variable.
        animadorPoste = GetComponent<Animator>();
        // Inicialmente desactiva el canvas de fin de nivel para que no sea visible.
        NivelSuperado.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica si hay una colisión con Sonic y si el evento ya se activó.
        if (collision.gameObject.tag == "Sonic" && !activado)
        {
            // Establece el booleano en verdadero para que este evento solo se active una vez.
            activado = true;

            TiempoNivel.PausarTemporizador();
            // Hace girar el poste.
            animadorPoste.Play("PosteGirar");
            ControladorDeSonidos.Instance.EjecutarSonido(SonidoPosteGirando);
            // Establece el poste con la imagen de Sonic y activa el canvas de fin de nivel.

            Invoke("SonicPoste", 3f);
            // Carga el siguiente nivel.
            Invoke("FindeNivel", 10f);
        }
    }

    private void SonicPoste()
    {
        animadorPoste.Play("PosteSonic");
        Sonic.Animar("Sonic_NivelSuperado");
        ControladorDeSonidos.Instance.DetenerMusicaDeFondo();
        ControladorDeSonidos.Instance.EjecutarSonido(SonidoNivelSuperado);
        Sonic.Muerto=true;
        NivelSuperado.enabled = true;
    }

    private void FindeNivel()
    {
        cargadorEscena.CargarSiguienteNivel();
    }
}


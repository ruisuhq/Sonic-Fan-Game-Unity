using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorDeSonidos : MonoBehaviour
{
    public static ControladorDeSonidos Instance;

    [SerializeField] private AudioSource Sonidos;
    [SerializeField] private AudioSource Musica;
    [SerializeField] private AudioSource Boss;
    [SerializeField] private float tiempoFadeOut = 0.5f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void EjecutarSonido(AudioClip sonido)
    {
        Sonidos.PlayOneShot(sonido);
    }

    public void DetenerMusicaDeFondo()
    {
        StartCoroutine(FadeOutMusica());
    }

    private IEnumerator FadeOutMusica()
    {
        float startVolume = Musica.volume;

        while (Musica.volume > 0)
        {
            Musica.volume -= startVolume * Time.deltaTime / tiempoFadeOut;
            yield return null;
        }

        Musica.Stop();
        Musica.volume = startVolume;
    }

    public void EmpezarMusicaDeFondo()
    {
        Musica.Play();
    }
    public void EmpezarBossDeFondo()
    {
        Boss.Play();
    }
    public void DetenerBossDeFondo()
    {
        StartCoroutine(FadeOutBoss());
    }
    private IEnumerator FadeOutBoss()
    {
        float startVolume = Boss.volume;

        while (Boss.volume > 0)
        {
            Boss.volume -= startVolume * Time.deltaTime / tiempoFadeOut;
            yield return null;
        }

        Boss.Stop();
        Boss.volume = startVolume;
    }
}

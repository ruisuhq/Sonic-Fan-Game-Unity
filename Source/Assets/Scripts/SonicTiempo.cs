using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SonicTiempo : MonoBehaviour
{
    public Canvas TiempoAcabado;
    public GameObject Sonic;
    private Sonic_controlador_nuevo sonic;

    public TMP_Text TextoTiempo;
    public float TiempoActual = 0;
    public bool Contando = false;
    void Start()
    {
        sonic = Sonic.GetComponent<Sonic_controlador_nuevo>();
        Contando = true;
        TiempoAcabado.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Contando)
        {
            if (TiempoActual < 599)
            {
                TiempoActual += Time.deltaTime;
            }
            else
            {
                TiempoActual = 599;
                Contando = false;
            }

            DisplayTime(TiempoActual);

            if (TiempoActual >= 599)
            {
                sonic.EvaluarVidas();
                if (ValoresDeJuego.Vidas > 0)
                {
                    TiempoAcabado.enabled = true;
                }
            }
        }
    }

    void DisplayTime(float TiempoMostrado)
    {
        TiempoMostrado += 1;

        float Minutos = Mathf.FloorToInt(TiempoMostrado / 60);
        float Segundos = Mathf.FloorToInt(TiempoMostrado % 60);

        TextoTiempo.text = string.Format("{0:00}:{1:00}", Minutos, Segundos);
    }

    public void PausarTemporizador()
    {
        Contando = false;
    }
    public void ReanudarTemporizador()
    {
        Contando = true;
    }
}

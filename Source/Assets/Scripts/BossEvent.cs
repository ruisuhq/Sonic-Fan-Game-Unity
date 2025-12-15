using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEvent : MonoBehaviour
{

    //InicioBoss
    public EdgeCollider2D InicioBoss;
    public GameObject Eggman;
    private Eggman_Controlador_nuevo scriptEggman;
    //Cosas que se habilitan y desabilitan
    public GameObject[] Barrera = new GameObject[2];
    public Cinemachine.CinemachineConfiner LimiteCamaraBoss;
    public PolygonCollider2D [] Limites = new PolygonCollider2D[2];
    
    

    private void Start()
    {
        // Desactivar las barreras al principio
        foreach (GameObject barrera in Barrera)
        {
            barrera.SetActive(false);
        }
        LimiteCamaraBoss.m_BoundingShape2D = Limites[0];
        scriptEggman = Eggman.GetComponent<Eggman_Controlador_nuevo>();

    }
    public void IniciarBossEvent()
    {
        InicioBoss.enabled = false;
            
        // Activar las barreras cuando el jugador entra
        foreach (GameObject barrera in Barrera)
        {
            barrera.SetActive(true);
        }

        // Activar el confinamiento de la cámara
            
        LimiteCamaraBoss.m_BoundingShape2D = Limites[1];
            

        // Instanciar al jefe Eggman en el punto de inicio
        ControladorDeSonidos.Instance.DetenerMusicaDeFondo();
        ControladorDeSonidos.Instance.EmpezarBossDeFondo();
        scriptEggman.Estado = 1;
    }
    public void FinalizarBoss()
    {
        foreach (GameObject barrera in Barrera)
        {
            barrera.SetActive(false);
        }
        ControladorDeSonidos.Instance.DetenerBossDeFondo();
        ControladorDeSonidos.Instance.EmpezarMusicaDeFondo();
        LimiteCamaraBoss.m_BoundingShape2D = Limites[0];
    }
}

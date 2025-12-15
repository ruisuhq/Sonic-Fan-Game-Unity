using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cangrejo : MonoBehaviour
{
    // Crea un campo en el inspector para vincular el objeto de juego de Sonic.
    public GameObject Sonic;
    // Crea una variable para almacenar un componente de script SonicController_FSM.
    private Sonic_controlador_nuevo sonicScript;
    // Crea campos en el inspector para colocar el animador y el renderizador de sprite del cangrejo.
    public Animator animadorCangrejo;
    public SpriteRenderer renderizadorSpriteCangrejo;

    // Crea una matriz donde se colocarán los objetos secundarios de spawnpoint del cangrejo.
    public Transform[] puntosSpawnBombas = new Transform[2];

    // Instancia una copia del prefab de la bomba de cangrejo.
    public GameObject bombaCangrejo;

    // Crea un campo de matriz en el inspector para colocar los dos puntos de ruta utilizados por el cangrejo.
    public Transform[] puntosDeRuta = new Transform[2];

    // Establece el valor bool en true para habilitar que la función Move() siga ejecutándose mientras el cangrejo no esté muerto.
    public bool puedeMoverse = true;

    // Establece la velocidad actual del cangrejo cuando se asigna el valor de velocidad.
    public float velocidadCangrejo = 1f;

    // Establece la velocidad inicial del cangrejo en 3 para que pueda comenzar a moverse inmediatamente.
    public float velocidad = 1f;
    public bool estaMuerto = false;

    // Se utiliza para recorrer la matriz de puntos de ruta.
    int actual = 0;
    [SerializeField] private AudioClip EnemigoExplosion;

    void Start()
    {
        // Recolecta los componentes necesarios al iniciar.
        sonicScript = Sonic.GetComponent<Sonic_controlador_nuevo>();
        animadorCangrejo = GetComponent<Animator>();
        renderizadorSpriteCangrejo = GetComponent<SpriteRenderer>();
        estaMuerto = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(sonicScript.estaLastimado == false){
        
            // Comprueba si el cangrejo colisiona con Sonic.
            if (collision.gameObject.tag == "Sonic")
            {
                // Comprueba si Sonic está girando actualmente y es inmortal.
                
                if (sonicScript.esMortal && !estaMuerto)
                {
                    // Detiene el movimiento y evita que se activen las declaraciones Invoke.
                    velocidad = 0;
                    puedeMoverse = false;
                    // Reproduce la animación de explosión y destruye el objeto cuando haya terminado.
                    animadorCangrejo.Play("Explosion_Enemigo");
                    ControladorDeSonidos.Instance.EjecutarSonido(EnemigoExplosion);
                    sonicScript.rb2d.velocity = new Vector2(sonicScript.rb2d.velocity.x, 10f);
                    estaMuerto = true;
                    Invoke("Destruido", 0.3f);
                }
            }
        }
    }

    void Update()
    {
        // Comprueba si todavía puede moverse (no puede si ha sido golpeado por Sonic mientras está girando).
        if (puedeMoverse)
        {
            Mover();
        }
    }

    private void Mover()
    {
        // Comienza a mover el cangrejo hacia el punto de ruta actual.
        transform.position = Vector2.MoveTowards(transform.position, puntosDeRuta[actual].transform.position, velocidad * Time.deltaTime);
        // Comprueba si la posición del cangrejo está a menos de 0.2 del punto de ruta actual.
        if (Vector2.Distance(puntosDeRuta[actual].transform.position, transform.position) < 0.2f)
        {
            // Establece la velocidad en cero para detener el cangrejo de moverse.
            velocidad = 0;
            // Si el punto de ruta actual es el primer punto de ruta, cambia la ubicación de ciertas animaciones y el objeto de lanzamiento de bombas en relación con el cangrejo.
            // También revierte la configuración flipX del sprite renderizado de las animaciones para que miren en la dirección correcta.
            if (actual == 0)
            {
                renderizadorSpriteCangrejo.flipX = true;
            }
            // Si el punto de ruta actual es el segundo punto de ruta, cambia la ubicación de ciertas animaciones y el objeto de lanzamiento de bombas en relación con el cangrejo.
            // También voltea los sprite renderizados de las animaciones para que miren en la dirección correcta.
            else if (actual == 1)
            {
                renderizadorSpriteCangrejo.flipX = false;
            }
            // Agrega uno a actual para que la próxima vez que se llame a Mover(), apunte al segundo punto de ruta.
            actual++;
            // Si el valor de actual se vuelve 2 o más, se restablece a 0 para que el cangrejo continúe patrullando entre los dos puntos de ruta.
            if (actual >= puntosDeRuta.Length)
            {
                actual = 0;
            }
            // Esta serie de funciones Invoked controla el tiempo de las animaciones, disparo de bombas y continuación de su movimiento.
            Invoke("CangrejoParado", 0.0f);
            Invoke("CangrejoDispara", 0.5f);
            Invoke("Disparar", 0.5f);
            Invoke("CangrejoCaminar", 1.0f);
            Invoke("CangrejoContinua", 1.0f);
        }
    }

    // Reproduce la animación de parada.
    private void CangrejoParado()
    {
        animadorCangrejo.Play("Cangrejo_Parado");
    }

    // Reproduce la animación de caminar.
    private void CangrejoCaminar()
    {
        animadorCangrejo.Play("Cangrejo_Caminando");
    }

    // Reproduce la animación de disparo.
    private void CangrejoDispara()
    {
        animadorCangrejo.Play("Cangrejo_Dispara");
    }

    // Restablece la velocidad del cangrejo a 5 para que pueda seguir moviéndose hacia el siguiente punto de ruta.
    private void CangrejoContinua()
    {
        if (puedeMoverse)
        {
            velocidad = velocidadCangrejo;
        }
    }

    // Instancia el prefab de bomba de cangrejo y establece su posición y rotación.
    void Disparar()
    {
        if (puedeMoverse)
        {
            Instantiate(bombaCangrejo, puntosSpawnBombas[0].position, Quaternion.Euler(0, 0, 15));
            Instantiate(bombaCangrejo, puntosSpawnBombas[1].position, Quaternion.Euler(0, 0, 345));
        }
    }

    // Destruye el objeto de juego.
    private void Destruido()
    {
        Destroy(gameObject);
    }
}

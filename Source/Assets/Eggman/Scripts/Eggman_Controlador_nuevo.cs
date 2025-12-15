using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eggman_Controlador_nuevo : MonoBehaviour
{

    //Declaraciones de uso para Eggman    
    public GameObject Sonic;
    private Sonic_controlador_nuevo sonicScript;
    public Animator animator;
    public SpriteRenderer renderizadorSpriteEggman;
    
    //movimiento
    public Transform[] PatronMovimiento = new Transform[2];
    public Transform Entrada;
    int PosicionActual = 0;
    public float velocidad;

    //Estados
    public int Estado = 0;
    public bool Inmortal;
    public int vida;

    //Comportamiento de la bola
    public GameObject BolaEggman;
    public GameObject[] anillos = new GameObject[4];
    public GameObject luces;
    public GameObject Anillos;

    //Movimiento de la bola
    public Rigidbody2D rb2D;
    public float velocidadBola;
    bool Derecha;

    //Hitbox
    private CircleCollider2D coliderCirculo;
    public CircleCollider2D ColiderCirculo { get { return coliderCirculo; } }

    //Sonidos
    [SerializeField] private AudioClip SonidoLastimado;
    [SerializeField] private AudioClip SonidoMuerto;

    //Muerte
    public GameObject[] SpawnPointExplosion = new GameObject[8];
    public GameObject[] Explosiones = new GameObject[2];
    public bool Muerto;
    public BossEvent bossEvent;
    
    void Awake()
    {
        //Activación de todos los componentes
        animator = GetComponent<Animator>();
        sonicScript = Sonic.GetComponent<Sonic_controlador_nuevo>();
        renderizadorSpriteEggman = GetComponent<SpriteRenderer>();
        coliderCirculo = GetComponent<CircleCollider2D>();
        coliderCirculo.enabled = true;
        Inmortal = false;
        transform.position = Entrada.transform.position;
        BolaEggman.SetActive(false);
        luces.SetActive(false);
        foreach (GameObject anillo in anillos)
        {
            anillo.SetActive(false);
        }
        rb2D = Anillos.GetComponent<Rigidbody2D>();
        Muerto = false;
        
    }
    
    //Colisión con Sonic y activación de estado lastimado
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(!sonicScript.estaLastimado){
        
            // Comprueba si el cangrejo colisiona con Sonic.
            if (collision.gameObject.tag == "Sonic")
            {
                // Comprueba si Sonic está girando actualmente y es inmortal.
                if (sonicScript.esMortal)
                {   
                    sonicScript.rb2d.velocity = new Vector2(sonicScript.rb2d.velocity.x, 10f);
                    if(Inmortal == false){
                        Lastimado();
                    }
                }
            }
        }
    }

    void FixedUpdate()
    {
        //Con proposito de Debug 

        // Debug.Log("Estado: " + Estado);
        // Debug.Log("Velocidad: " + velocidad);
        // Debug.Log("PosicionActual: " + PosicionActual);
        // Debug.Log("Posicion actual de Eggman: " + transform.position);
        // Debug.Log("Destino actual de Eggman: " + PatronMovimiento[PosicionActual].transform.position);
        // Debug.Log("Eggman real está muerto?" + Muerto);

        if(Estado == 1)
            {
                Entrar();
            }
        else if(Estado == 2)
            {
                Eggman_Mover();
            }
        else if (Estado == 3)
            {
                Eggman_MoverFuera();
            }
    }
    
    //Funciones

    //Estado 0: Movimiento de Eggman al spawnear
    private void Entrar()
    {
        transform.position = Vector2.MoveTowards(transform.position, PatronMovimiento[PosicionActual].transform.position, 3 * Time.deltaTime);
        luces.transform.position = new Vector3(transform.position.x, transform.position.y - 1.26f, 2f);
        if (Vector2.Distance(transform.position, PatronMovimiento[PosicionActual].transform.position) < 0.2f)
        {  
            velocidad = 0;
            Invoke("PrepararBolaCompleta", 0.0f);
            Invoke("Eggman_Mover", 4.0f);
        }
    }
    
    //Estado 1: Movimiento de Eggman patron
    private void Eggman_Mover()
    {
        //Prepara todo antes de entrar al estado 1
        if(Estado == 1){
        Estado = 2;
        transform.position = PatronMovimiento[0].transform.position;
        }
        
        StopAllCoroutines();

        if (Estado == 2){
        transform.position = Vector2.MoveTowards(transform.position, PatronMovimiento[PosicionActual].transform.position, velocidad * Time.deltaTime);
        luces.transform.position = new Vector3(transform.position.x, transform.position.y - 1.26f, 2f);
        if (Vector2.Distance(transform.position, PatronMovimiento[PosicionActual].transform.position) < 0.2f)
        {
            velocidad = 0f;
            velocidadBola = 90f;
            if (PosicionActual == 0)
            {
                renderizadorSpriteEggman.flipX = false;
                Derecha = false;
                
            }
            if (PosicionActual == 1)
            {
                renderizadorSpriteEggman.flipX = true;
                Derecha = true;
            }
            
            PosicionActual++;
            
            if (PosicionActual >= PatronMovimiento.Length)
            {
                PosicionActual = 0;
            }
            Invoke("Eggman_Idle", 0.0f);
            Invoke("Eggman_Movimiento", 1.0f);
            Invoke("Eggman_Patron", 1.0f);
        }
        MoverBola();
        }
    }
    
    //Estado 2: Movimiento de Eggman en salida
    private void Eggman_MoverFuera()
    {
        if(Estado == 4) 
        {
        Estado = 3;
        GameObject explosion = Instantiate(Explosiones[1], transform.position, Quaternion.identity);
        explosion.transform.localScale = new Vector3(8, 8, 8);
        GameObject explosion2 = Instantiate(Explosiones[0], luces.transform.position, Quaternion.identity);
        explosion2.transform.localScale = new Vector3(10, 10, 10);
        GameObject explosion3 = Instantiate(Explosiones[1], BolaEggman.transform.position, Quaternion.identity);
        explosion3.transform.localScale = new Vector3(8, 8, 8);
        Destroy(luces);
        Muerto = true;
        bossEvent.FinalizarBoss();
        }
        Invoke("Eggman_Salida", 0.0f);
        transform.position = Vector2.MoveTowards(transform.position, Entrada.position, 3 * Time.deltaTime);
    }
    
    //Estado Extra: Lastimado
    public void Lastimado()
    {
        vida -= 1;
        Inmortal = true;
        CancelInvoke("Eggman_Idle");
        CancelInvoke("Eggman_Movimiento");
        if (vida > 0){
            Animar("Eggman_Lastimado");
            EjecutarSonido(SonidoLastimado);
            if (velocidad == 0)
            {
                Invoke("Eggman_Idle", 1.0f);
            } else {
                Invoke("Eggman_Movimiento", 1.0f);
            }
            Invoke("Eggman_Patron", 1.0f);
        }
        else if (vida == 0){
            velocidad = 0;
            rb2D.simulated = false;
            Animar("Eggman_Muerto");
            EjecutarSonido(SonidoMuerto);
            ColiderCirculo.enabled = false;
            StartCoroutine(GenerarExplosiones());
            Invoke("Eggman_MoverFuera", 3.0f);
            Invoke("Destruido",6.0f);
            Estado = 4;
        }
        
    }
    
    //Funciones para el comportamiento de eggman
    private void Eggman_Patron(){
        
        velocidad = 3f;
        velocidadBola = 100f;
    }
    public void Animar(string animacion){
        animator.Play(animacion);
    }
    private void Eggman_Idle(){
        Inmortal=false;
        Animar("Eggman_Idle");
    }
    private void Eggman_Movimiento(){
        Inmortal=false;
        Animar("Eggman_Movimiento");
    }
    private void Eggman_Salida(){
        Inmortal=true;
        Animar("Eggman_Salida");
    }
    private void Destruido()
    {
        transform.position = Entrada.position;
        GameObject explosion = Instantiate(Explosiones[1], transform.position, Quaternion.identity);
        explosion.transform.localScale = new Vector3(15, 15, 15);
        Destroy(gameObject);
    }  
    public void EjecutarSonido(AudioClip sonido)
    {
        ControladorDeSonidos.Instance.EjecutarSonido(sonido);
    }
    
    //Funciones para el comportamiento de la bola
    IEnumerator PreparacionBola()
    {
        Vector3 PosiciónInicio = new Vector3(transform.position.x, transform.position.y, 0f);
        Vector3 PosicionFinal = new Vector3(transform.position.x, transform.position.y - 1.26f - 3.1f, 0f);


        float TiempoActual = 0f;
        float Duraciondescenso = 2f; // Duración del descenso

        while (TiempoActual < Duraciondescenso)
        {
            // Interpolación lineal para mover la bola desde la posición inicial a la posición final durante el tiempo especificado
            BolaEggman.transform.position = Vector3.Lerp(PosiciónInicio, PosicionFinal, TiempoActual / Duraciondescenso);

            TiempoActual += Time.deltaTime;
            yield return null;
        }
        BolaEggman.transform.position = PosicionFinal;
        
    }
    IEnumerator PreparacionLuces()
    {
        Vector3 PosiciónInicio = new Vector3(transform.position.x, transform.position.y, 1.5f);
        Vector3 PosicionFinal = new Vector3(transform.position.x, transform.position.y - 1.26f, 1.5f);
        float TiempoActual = 0f;
        float Duraciondescenso = 2f; // Duración del descenso
        while (TiempoActual < Duraciondescenso)
        {
            // Interpolación lineal para mover la luces desde la posición inicial a la posición final durante el tiempo especificado
            luces.transform.position = Vector3.Lerp(PosiciónInicio, PosicionFinal, TiempoActual / Duraciondescenso);

            TiempoActual += Time.deltaTime;
            yield return null;
        }
        luces.transform.position = PosicionFinal;
    }
    IEnumerator PreparacionRing(GameObject anillo)
    {
        Vector3 PosiciónInicio = new Vector3(transform.position.x, transform.position.y, 2f);
        Vector3 PosicionFinal = new Vector3(transform.position.x, transform.position.y - 1.26f - (0.5f * (System.Array.IndexOf(anillos, anillo) + 1)), 2f);
        float TiempoActual = 0f;
        float Duraciondescenso = 2f; // Duración del descenso
        while (TiempoActual < Duraciondescenso)
        {
            // Interpolación lineal para mover el anillo desde la posición inicial a la posición final durante el tiempo especificado
            anillo.transform.position = Vector3.Lerp(PosiciónInicio, PosicionFinal, TiempoActual / Duraciondescenso);

            TiempoActual += Time.deltaTime;
            yield return null;
        }
        anillo.transform.position = PosicionFinal;
    }
    private void PrepararBolaCompleta()
    {
        BolaEggman.SetActive(true);
        StartCoroutine(PreparacionBola());
        luces.SetActive(true);
        StartCoroutine(PreparacionLuces());
        foreach (GameObject anillo in anillos)
        {
            anillo.SetActive(true);
            StartCoroutine(PreparacionRing(anillo));
        }
    }
    private void MoverBola()
    {
        if(Derecha)
        {
            rb2D.angularVelocity = velocidadBola;
        }
        if(!Derecha)
        {
            rb2D.angularVelocity = -1*velocidadBola;
        }
    }

    //Muerte de Eggman

    IEnumerator GenerarExplosiones()
    {
        float tiempoTotal = 3f;
        int totalExplosiones = 18;
        float tiempoEntreExplosiones = tiempoTotal / totalExplosiones;

        while (tiempoTotal > 0f)
        {
            GameObject explosion = Instantiate(Explosiones[Random.Range(0, Explosiones.Length)], SpawnPointExplosion[Random.Range(0, SpawnPointExplosion.Length)].transform.position, Quaternion.identity);
            explosion.transform.localScale = new Vector3(8, 8, 8);
            tiempoTotal -= tiempoEntreExplosiones;
            yield return new WaitForSeconds(tiempoEntreExplosiones);
        }
    }
}   

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Sonic_controlador_nuevo : MonoBehaviour
{
    //Para el Raycast de si está pisando
    [SerializeField] private LayerMask Piso;
    [SerializeField] private LayerMask Tunel;

    //Sistema de vidas TMP Para Texto y los rings
    public TMP_Text VidasActuales;
    public TMP_Text RingsActuales;
    public TMP_Text PuntajeActual;
    public TMP_Text PuntajeFinal;
    public Canvas Perdiste;
    public GameObject RingPerdido;
    public Transform[] SpawnPointRingsPerdidos = new Transform[5];
    private Vector2 SaltoLastimado;
    public ControladorSonicEscena SonicReaparece;
    //Sonic es vulnerable
    public bool esMortal = false;
    public bool estaLastimado = false;
    public bool Muerto = false;
    //Rings de Sonic
    public int rings = 0;
    public int Puntaje = 0;

    System.Random random = new System.Random();

    //Variables de Sonic
    public float VelocidadInicial = 5f;
    public float VelocidadMaxima = 15f;
    public float Aceleracion = 5f;
    public float VelocidadActual = 5f;
    //Como en el juevo, mismo impulso
    public float FuerzaSalto = 16f;
    
    //Variables de los componentes de Sonic
    private SpriteRenderer sprite;
    public SpriteRenderer Sprites { get { return sprite; } }

    private Animator Animacion;

    private CapsuleCollider2D coliderCapsula;
    public CapsuleCollider2D ColiderCapsula { get { return coliderCapsula; } }

    private CircleCollider2D coliderCirculo;
    public CircleCollider2D ColiderCirculo { get { return coliderCirculo; } }

    public Rigidbody2D rb2d;
    public Rigidbody2D RigidBody2d { get { return rb2d; } }

    //Referencia al estado actual de sonic en cada función (Salto, Correr)
    private SonicEstadoBase EstadoActual;

    //Estados de Sonic
    public readonly SonicParado Parado = new SonicParado();
    public readonly SonicCorriendoDerecha CorriendoDerecha = new SonicCorriendoDerecha();
    public readonly SonicCorriendoIzquierda CorriendoIzquierda = new SonicCorriendoIzquierda();
    public readonly SonicSaltando Saltando = new SonicSaltando();
    public readonly SonicSaltandoDerecha SaltandoDerecha = new SonicSaltandoDerecha();
    public readonly SonicSaltandoIzquierda SaltandoIzquierda = new SonicSaltandoIzquierda();
    public readonly SonicAgachado Agachado = new SonicAgachado();
    public readonly SonicRodandoDerecha RodandoDerecha = new SonicRodandoDerecha();
    public readonly SonicRodandoIzquierda RodandoIzquierda = new SonicRodandoIzquierda();
    public readonly SonicMirandoArriba MirandoArriba = new SonicMirandoArriba();
    public readonly SonicLastimado lastimado = new SonicLastimado();
    public readonly SonicEmpujandoDerecha EmpujandoDerecha = new SonicEmpujandoDerecha();
    public readonly SonicEmpujandoIzquierda EmpujandoIzquierda = new SonicEmpujandoIzquierda();

    //Sonidos de Sonic
    [SerializeField] public AudioClip SonidoSalto;
    [SerializeField] public AudioClip SonidoRodar;
    [SerializeField] public AudioClip SonidoLastimado;
    [SerializeField] public AudioClip SonidoMuerto;

    //Rotar angulos de Sonic
    private float anguloOriginal;
    public float velocidadRotacion = 5F;
    
    private void Awake(){
        //Iniciar componentes de Sonic con Variables locales
        coliderCapsula = GetComponent<CapsuleCollider2D>();
        coliderCirculo = GetComponent<CircleCollider2D>();
        Animacion = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();

        ControladorDeSonidos.Instance.EmpezarMusicaDeFondo();

        //Animar a Sonic Parado como estado base principal al iniciar
        Animar("Sonic_Parado");
        //Como está parado, el colider ciruclo no es neceesario.
        ColiderCirculo.enabled = false;
        anguloOriginal = transform.rotation.eulerAngles.z;
    }


    private void Start()
    {
        Transicion(Parado);

        Perdiste.enabled = false;

        if (ValoresDeJuego.Vidas <= 0)
        {
            ValoresDeJuego.Vidas = 3;
        }
    }
    void Update()
    {
        if(!Muerto){
            EstadoActual.Update(this);
            ReajusteVelocidad();
            AdherirseAlSuelo();
            RotarSpriteSegunAngulo();
            //Debug.Log("Pisando: " + Pisando() + ", PisandoGirando " + PisandoGirando());
        }
        MostrarVidas(ValoresDeJuego.Vidas);
        MostrarRings(rings);
        MostrarPuntaje(Puntaje);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "RingPerdido")
        {
            rings++;
            Debug.Log("Rings Actuales: " + rings);
        }
        if (collision.gameObject.tag == "Ring")
        {
            rings++;
            Puntaje+=100;
            Debug.Log("Rings Actuales: " + rings);
        }
        else if (collision.gameObject.tag == "BalaEnemigo")
        {
            if (rings > 0)
            {
                PierdeRings();
                Lastimado();
                Transicion(lastimado);
            }
            else
            {
                EvaluarVidas();
            }
        }
        else if (collision.gameObject.tag == "Enemigo")
        {
            if (!esMortal)
            {
                if (rings > 0)
                {
                    PierdeRings();
                    Lastimado();
                    Transicion(lastimado);
                }
                else
                {
                    EvaluarVidas();
                }
            }
            else
            {
                Puntaje+=500;
            }   
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
{
    if (collision.gameObject.tag == "Zona de Muerte")
    {
        // // Si colisiona con la zona de muerte, Sonic muere.
        Muerto = true;
        // // Evalúa las vidas después de la muerte.
        EvaluarVidas();
    }
    else if (collision.gameObject.tag == "Enemigo")
    {
        // Si colisiona con un enemigo y no es mortal
        if (!esMortal)
        {
            // Si tiene anillos, los pierde y se lastima.
            if (rings > 0)
            {
                PierdeRings();
                Lastimado();
                Transicion(lastimado);
            }
            // Si no tiene anillos, evalúa las vidas.
            else
            {
                EvaluarVidas();
            }
        } 
        else
        {
            Puntaje+=500;
        }         
    }
    else if (collision.gameObject.tag == "Puas")
    {
        // Si colisiona con pinchos y tiene anillos, los pierde y se lastima.
        if (rings > 0)
        {
            PierdeRings();
            Lastimado();
            Transicion(lastimado);
        }
        // Si no tiene anillos, evalúa las vidas.
        else
        {
            EvaluarVidas();
        }
    }
    else
    {
        // Esto llama al método OnCollisionEnter2D para el estado actual.
        EstadoActual.OnCollisionEnter2D(this);
    }
}

    //Función para cambiar a Sonic de estado dentro del juego
    public void Transicion(SonicEstadoBase sonicEstado){
        EstadoActual = sonicEstado;
        EstadoActual.EntrarEstado(this);
    }
    //Función para animar a sonic
    public void Animar(string SonicAnimacion){
        Animacion.Play(SonicAnimacion);
    }
    //Función para voltear el Sprite de Sonic
    public void VoltearSprite(bool volteado){
        sprite.flipX = volteado;
    }
    public bool Pisando()
    {
        //Coloca una altura extra para el raycast
        float AlturaExtra = 0.1f;

        //Envía un raycast desde el centro del coliderCapsula de  Sonic para checar si hay otro collider cerca de el.
        RaycastHit2D raycastHit = Physics2D.Raycast(ColiderCapsula.bounds.center, -transform.up, ColiderCapsula.bounds.extents.y + AlturaExtra, Piso);

        // Devuelve verdadero siempre y cuando el rayo esté golpeando una superficie debajo de él.
        return raycastHit.collider != null;
    }
    public bool PisandoGirando()
    {
        float AlturaExtra = 0.1f;

        RaycastHit2D raycastHit = Physics2D.Raycast(coliderCirculo.bounds.center, -transform.up, coliderCirculo.bounds.extents.y + AlturaExtra, Piso);

        return raycastHit.collider != null;
    }
    public bool NoEstaEnTunel()
    {
        // Establece una longitud extra para que el rayo se extienda.
        float AlturaExtra = 0.5f;

        // Envía rayos desde el centro del círculo de colisión de Sonic en todas las direcciones para verificar colisiones cuando está en un túnel.
        RaycastHit2D golpeSuperior = Physics2D.Raycast(ColiderCirculo.bounds.center, Vector2.up, ColiderCirculo.bounds.extents.y + AlturaExtra, Tunel);
        RaycastHit2D golpeInferior = Physics2D.Raycast(ColiderCirculo.bounds.center, Vector2.down, ColiderCirculo.bounds.extents.y + AlturaExtra, Tunel);
        RaycastHit2D golpeDerecha = Physics2D.Raycast(ColiderCirculo.bounds.center, Vector2.right, ColiderCirculo.bounds.extents.x + AlturaExtra, Tunel);
        RaycastHit2D golpeIzquierda = Physics2D.Raycast(ColiderCirculo.bounds.center, Vector2.left, ColiderCirculo.bounds.extents.x + AlturaExtra, Tunel);

        // Retorna verdadero siempre y cuando ninguno de los rayos colisione con otra superficie.
        return  golpeSuperior.collider == null && golpeInferior.collider == null &&
                golpeDerecha.collider == null && golpeIzquierda.collider == null;
    }
    public void ChecarVelocidad()
    {
        //Si sonic corre por encima de 12f de velocidad, su animación pasa a ser correr muy rapido
        if (VelocidadActual > 12f)
        {
            Animar("Sonic_Corre_Rapido");
        }
        //Si sonic corre por debajo de 12f de velocidad, su animación pasa a ser correr normal
        else if (VelocidadActual <= 12f)
        {
            Animar("Sonic_Corre");
        }
    }
    public void EvaluarVidas()
        {
            if (!Muerto)
            {
                Muerto = true;
                ValoresDeJuego.Vidas--;
                Debug.Log("Perdiste una vida");
            }
            
            if (ValoresDeJuego.Vidas > 0)
            {
                Muerto = true;
                SonicMuerto();
            }
            else
            {
                Muerto = true;
                FinJuego();
            }
        }
    private void Lastimado()
    {
        esMortal = true;
        float Direcion;
        if (sprite.flipX == false)
        {
            Direcion = -5f;
        }
        else
        {
            Direcion = 5f;
        }
        EjecutarSonido(SonidoLastimado);
        rb2d.velocity = new Vector2(Direcion, 10f);
    }
    private void PierdeRings()
    {
        for (int i = 0; i < rings; i++)
        {
            int randomSpawnPoint = random.Next(0, 5);
            int randomDirection = random.Next(1, 3) == 1 ? random.Next(1, 46) : random.Next(315, 360);
            Instantiate(RingPerdido, SpawnPointRingsPerdidos[randomSpawnPoint].position, Quaternion.Euler(0, 0, randomDirection));
        }
        rings = 0;
    }
    public void SonicMuerto()
    {
        Animar("Sonic_Muerto");
        coliderCapsula.enabled = false;
        coliderCirculo.enabled = false;
        RigidBody2d.velocity = new Vector2(0f, FuerzaSalto);
        EjecutarSonido(SonidoMuerto);
        Invoke("Reaparecer", 1f);
    }
    public void FinJuego()
    {
        Animar("Sonic_Muerto");
        Perdiste.enabled = true;
        ColiderCapsula.enabled = false;
        ColiderCirculo.enabled = false;
        RigidBody2d.velocity = new Vector2(0f, FuerzaSalto);
        Invoke("MenuFinJuego", 1f);
        
    }
    private void Reaparecer()
    {
        ControladorDeSonidos.Instance.DetenerMusicaDeFondo();
        ControladorDeSonidos.Instance.DetenerBossDeFondo();
        SonicReaparece.ReiniciarNivel();
    }
    
    private void MenuFinJuego()
    {
        ControladorDeSonidos.Instance.DetenerMusicaDeFondo();
        SonicReaparece.MenuFinJuego();
    }

    void MostrarVidas(int Vidas)
    {
        VidasActuales.SetText(Vidas.ToString());
    }

    void MostrarRings(int Rings)
    {
        RingsActuales.SetText(Rings.ToString());
    }
    void MostrarPuntaje(int Puntaje)
    {
        PuntajeActual.SetText(Puntaje.ToString());
        PuntajeFinal.SetText(Puntaje.ToString());
    }
    public void EjecutarSonido(AudioClip sonido)
    {
        ControladorDeSonidos.Instance.EjecutarSonido(sonido);
    }
    public void RotarSpriteSegunAngulo()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, Piso);
        //Debug.Log("Distancia: " + hit.distance);
        if (hit.collider != null && hit.distance < 1f)
        {
            //Se obtiene la normal del suelo
            Vector2 SueloNormal = hit.normal;

            //Calculo el angulo actual del suelo según la normal
            float angulo = Mathf.Atan2(SueloNormal.x, SueloNormal.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(-angulo, Vector3.forward);
        }
        //si no está tocando ningun suelo, regresa a su posición normal con el tiempo      
        else
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0f, 0f, anguloOriginal), velocidadRotacion * Time.deltaTime);
        }
        
    }
    public void AdherirseAlSuelo()
    {
        if (Pisando())
        {
            RaycastHit2D hit = Physics2D.Raycast(coliderCapsula.bounds.center, -transform.up, Mathf.Infinity, Piso);
            if (hit.collider != null)
            {
                // Calcula la distancia al suelo
                float distanciaAlSuelo = hit.distance - coliderCapsula.bounds.extents.y;

                // Mueve el transform de Sonic hacia arriba para que esté en el suelo
                transform.position += new Vector3(0f, -distanciaAlSuelo, 0f);
            }
        }
        else if (PisandoGirando())
        {
            RaycastHit2D hit = Physics2D.Raycast(coliderCirculo.bounds.center, -transform.up, Mathf.Infinity, Piso);
            if (hit.collider != null)
            {
                // Calcula la distancia al suelo
                float distanciaAlSuelo = hit.distance - coliderCirculo.bounds.extents.y;

                // Mueve el transform de Sonic hacia arriba para que esté en el suelo
                transform.position += new Vector3(0f, -distanciaAlSuelo, 0f);
            }
        }
    }

    public bool EstaEmpujando()
    {
        float LongitudExtra = 0.05f;

        //Enviar dos rayos desde el centro de sonic hacia los lados
        if(Input.GetAxisRaw("Horizontal") > 0) 
        {
            RaycastHit2D VerificarDerecha = Physics2D.Raycast(coliderCapsula.bounds.center, Vector2.right, coliderCapsula.bounds.extents.x + LongitudExtra, Piso);
            return VerificarDerecha.collider != null;
        }
        if(Input.GetAxisRaw("Horizontal") < 0)
        {
            RaycastHit2D VerificarIzquierda = Physics2D.Raycast(coliderCapsula.bounds.center, Vector2.left, coliderCapsula.bounds.extents.x + LongitudExtra, Piso);
            return VerificarIzquierda.collider != null;
        }
        return false;
    }
    public void ReajusteVelocidad()
    {
        if (Mathf.Abs(rb2d.velocity.magnitude) < 0.1f) // Compara si la velocidad x es cercana a cero
        {
            VelocidadActual = VelocidadInicial;
        }
    }
}

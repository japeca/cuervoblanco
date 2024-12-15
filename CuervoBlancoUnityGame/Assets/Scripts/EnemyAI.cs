using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Patrullaje")]
    public float velocidadPatrulla = 2f;
    public Transform limiteMin; // Límite inferior (respawn del personaje).
    public Transform limiteMax; // Límite superior (puerta al final de la escena).

    [Header("Detección del jugador")]
    public float rangoDeteccion = 5f;
    public LayerMask capaJugador;

    [Header("Ataque")]
    public float rangoAtaque = 1f;
    public int daño = 10;
    public float tiempoEntreAtaques = 1f;


    [Header("Configuración de bordes y paredes")]
    public Transform detectorSuelo; // Un punto de detección en el borde del enemigo.
    public float rangoDeteccionSuelo = 0.5f; // Distancia para detectar suelo.
    public LayerMask capaSuelo; // Capa que define las superficies sólidas

    private Rigidbody2D rb;
    private Transform jugador;
    private bool moviendoDerecha = true;
    private bool persiguiendo = false;
    private float tiempoUltimoAtaque;

    Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jugador = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Detectar si el jugador está en rango.
        if (jugador != null && Vector2.Distance(transform.position, jugador.position) <= rangoDeteccion)
        {
            persiguiendo = true;
        }
        else
        {
            persiguiendo = false;
        }

        if (persiguiendo)
        {
            PerseguirJugador();
        }
        else
        {
            Patrullar();
        }

        // Comprobar si el enemigo está en rango para atacar.
        if (jugador != null && Vector2.Distance(transform.position, jugador.position) <= rangoAtaque)
        {
            AtacarJugador();
        }
    }



    private void detectarSuelo()
    {
        RaycastHit2D sueloDetectado = Physics2D.Raycast(detectorSuelo.position, Vector2.down, rangoDeteccionSuelo, capaSuelo);
        if (!sueloDetectado.collider)
        {
            Girar(); // Si no hay suelo, girar.
        }
    }

    private void detectarPared()
    {
        RaycastHit2D paredDetectada = Physics2D.Raycast(transform.position, moviendoDerecha ? Vector2.right : Vector2.left, rangoDeteccionSuelo, capaSuelo);
        if (paredDetectada.collider)
        {
            Girar(); // Si hay una pared, girar.
        }
    }



    private void Patrullar()
    {

        animator.SetBool("persiguiendo", false);
        animator.SetBool("atacando", false);
        animator.SetBool("patrullando", true);


        if (moviendoDerecha)
        {
            rb.velocity = new Vector2((moviendoDerecha ? velocidadPatrulla : -velocidadPatrulla), rb.velocity.y);


            detectarPared();
            detectarSuelo();

            if (transform.position.x >= limiteMax.position.x)
            {
                moviendoDerecha = false;
                Girar();
            }
        }
        else
        {
            rb.velocity = new Vector2((moviendoDerecha ? velocidadPatrulla : -velocidadPatrulla), rb.velocity.y);

            detectarPared();
            detectarSuelo();


            if (transform.position.x <= limiteMin.position.x)
            {
                moviendoDerecha = true;
                Girar();
            }
        }
    }

    private void PerseguirJugador()
    {

        animator.SetBool("patrullando", false);
        animator.SetBool("atacando", false);
        animator.SetBool("persiguiendo", true);

        Vector2 direccion = (jugador.position - transform.position).normalized;
        rb.velocity = new Vector2(direccion.x * velocidadPatrulla, rb.velocity.y);

        if ((direccion.x > 0 && !moviendoDerecha) || (direccion.x < 0 && moviendoDerecha))
        {
            Girar();
        }
    }

    private void AtacarJugador()
    {
        animator.SetBool("persiguiendo", false);
        animator.SetBool("atacando", true);

        if (Time.time - tiempoUltimoAtaque >= tiempoEntreAtaques)
        {
            tiempoUltimoAtaque = Time.time;
            Debug.Log("Atacando al jugador con daño: " + daño);

            // Pendiente implementar salud del jugador.
        }
    }

    private void Girar()
    {
        moviendoDerecha = !moviendoDerecha;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class viking : MonoBehaviour
{
    /*
     * Clase que tiene por objetivo controlar el movimiento del personaje (caracter, transform, vikingo).
     */
    [Header("References")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask whatIsGround;

    [Header("Settings")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    public float limiteCaida = -10f; // Límite de altura en el eje Y para detectar caída.

    private Rigidbody2D RigidBody;
    private float xInput;
    private float Horizontal;
    private Animator Animator;
    public bool atacando;
    private PlataformaManager plataformaManager;
    private bool notificacionEnviada = false;
    private AudioSource audioSource; 

    public CameraFollower camara;
    public RespawnManager respawnManager;
    public bool recibiendoDano;
    public float fuerzaRebote = 5f;
    public float vida = 3;
    public bool muerto;
    public bool isRespawned = false;


    void Start()
    {
        RigidBody = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        plataformaManager = FindObjectOfType<PlataformaManager>();
        audioSource = GetComponent<AudioSource>();


        if (plataformaManager == null)
        {
            Debug.Log("No se encontró un PlataformaManager en esta escena. Ignorando la regeneración de plataformas.");
        }



    }

    void Update()
    {
        if (muerto)
        {
            RigidBody.velocity = Vector2.zero;
            return;
        }

        Horizontal = Input.GetAxisRaw("Horizontal");
        //Debug.DrawRay(transform.position, Vector3.down * 0.6f, Color.red);
        Animator.SetBool("running", Horizontal != 0.0f);
        Animator.SetBool("atacando", atacando);
        Animator.SetBool("recibedano", recibiendoDano);
        //Animator.SetBool("muerto", muerto);

        if (transform.position.y < limiteCaida)
        {
            IniciarCaida();
            IniciarRespawn();
        }


        Controles();



    }

    private void Controles()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Atacando();
        }

        Flip();

        if (Input.GetKeyDown(KeyCode.W) && IsGrounded())
        {
            Jump();
        }
    }

    private void Jump()
    {
        //Debug.Log("Saltaaa");
        RigidBody.velocity = new Vector2(RigidBody.velocity.x, 0);

        // El salto
        RigidBody.AddForce(Vector2.up * (jumpForce * Time.fixedDeltaTime), ForceMode2D.Impulse);
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (muerto)
        {
            RigidBody.velocity = Vector2.zero; // Detener cualquier movimiento residual.
            return;
        }
        // Capturamos teclado
        xInput = Input.GetAxisRaw("Horizontal");
        Vector2 move = new Vector2(xInput * speed, RigidBody.velocity.y);
        RigidBody.velocity = move;
    }

    //funcion que devuelve TRUE si está en el suelo
    private bool IsGrounded()
    {
        //version inicial
        //Ray2D ray = new Ray2D(groundCheck.position, Vector2.down);
        // Se lanza el rayo y se guarda la colisión
        //RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 0.3f, whatIsGround);
        //Debug.DrawRay(ray.origin, ray.direction * 0.05f, Color.red);
        // Devuelve "true" si el rayo ha detectado suelo y "false" si no.
        //bool chocado = hit.collider; //util para el debug
        //Debug.Log("hit.collider: " + chocado);
        //return hit.collider is not null;



        //otra aproximacion
        Vector2 posicion = groundCheck.position; // Posición del groundCheck.
        float radio = 0.25f; // Radio del círculo de detección.
        return Physics2D.OverlapCircle(posicion, radio, whatIsGround) != null;

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, 0.2f);
    }

    private void Flip()
    {
        // Derecha
        if (xInput > 0 && transform.localScale.x < 0)
        {
            // Escala en X en positivo. El personaje está escalado por 3.
            transform.localScale = new Vector3(3, 3, 3);
        }
        // Izquierda
        else if (xInput < 0 && transform.localScale.x > 0)
        {
            // Escala X en negativo. El personaje está escalado por 3.
            transform.localScale = new Vector3(-3, 3, 3);
        }
    }

    private void IniciarCaida()
    {
        // Llama al método para detener el seguimiento de la cámara.
        camara.DetenerSeguimiento();
        if (plataformaManager != null)
        {
            plataformaManager.DesactivarPlataformasInmediatamente(); // Desactiva inmediatamente.
        }


        // Aquí se puede añadir lógica adicional, como iniciar animaciones o esperar antes del respawn.
        Debug.Log("El personaje ha caído. La cámara deja de seguirlo.");
    }

    public void Atacando()
    {
        if (atacando) return;
        atacando = true;
        Animator.SetBool("atacando", true);

        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.Play();
        }
    }

    public void DetenAtaque()
    {
        atacando = false;
        //Invoke(nameof(DetenAtaque), 0.5f);
    }

    public void RecibeDano(Vector2 direccion, int cantidadDano)
    {
        if (!recibiendoDano && !muerto)
        {
            recibiendoDano = true;
            vida -= cantidadDano;
            if (vida <= 0)
            {
                muerto = true;
                Animator.SetBool("muerto", true); // Activar animación de muerte.
                RigidBody.velocity = Vector2.zero; // Detener movimiento.
                //Invoke(nameof(ReiniciarNivel), 2f);
                //StartCoroutine(RespawnDespuesDeMuerte() );
                return;
            }

            Vector2 rebote = new Vector2(transform.position.x - direccion.x, 0.2f).normalized;
            RigidBody.AddForce(rebote * fuerzaRebote, ForceMode2D.Impulse);

        }

    }

    public void DesactivaDano()
    {
        recibiendoDano = false;
        RigidBody.velocity = Vector2.zero;
    }



    private void IniciarRespawn()
    {
        if (vida <= 0)
        {
            Animator.SetBool("muerto", true);
            Debug.Log("El personaje está muerto. Notificando al GameManager...");
            Invoke(nameof(NotificarMuerte), 0.5f); // Añade un retraso si quieres ver la animación de muerte.
            return;
        }

        // Lógica de respawn normal
        isRespawned = true;
        Animator.SetBool("muerto", false);
        Animator.Play("Idle");

        if (plataformaManager != null)
        {
            plataformaManager.RegenerarPlataformas();
        }

        muerto = false;
        recibiendoDano = false;
        vida--;

        respawnManager.Respawn();
        camara.ReanudarSeguimiento();
    }


    private void NotificarMuerte()
    {
        if (notificacionEnviada) return;

        notificacionEnviada = true;
        GameManager.instancia.IncrementarMuertes();
        //IniciarRespawn();
    }
}
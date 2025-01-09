using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Patrullaje")]
    public float velocidadPatrulla = 2f;
    /*
    public Transform limiteMin; // Límite inferior (respawn del personaje).
    public Transform limiteMax; // Límite superior (puerta al final de la escena).
    */

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
    public int vida = 3;
    public float fuerzaRebote = 5f;

    private Rigidbody2D rb;
    private Transform jugador;
    private bool moviendoDerecha = true;
    private bool persiguiendo = false;
    private float tiempoUltimoAtaque;
    private bool playerIsAlive;
    private bool enMovimiento;
    private bool muerto;
    private viking playerController;
    private bool recibiendoDano;
    private Vector2 movement;

    Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enMovimiento = true;
        jugador = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        playerIsAlive = true;
        playerController = GameObject.Find("Viking").GetComponent<viking>();
        muerto = false;
    }

    void Update()
    {
        if (playerController != null && !playerController.muerto && !muerto)
        {
            Movimiento();
        }
        else
        {
            if (muerto == true)
            {
                enMovimiento = true;
                movement = Vector2.zero;
                animator.SetBool("muerto", muerto);
            }
        }

    }



    private void Movimiento()
    {
        if (playerIsAlive && !muerto)
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
            enMovimiento = true;
        }
        else
        {
            movement = Vector2.zero;
            enMovimiento = false;
        }

    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector2 direccionDano = new Vector2(transform.position.x, 0);
            viking playerScript = collision.gameObject.GetComponent<viking>();
            playerScript.RecibeDano(direccionDano, 1);

            playerIsAlive = !playerScript.muerto;
            if (!playerIsAlive)
            {
                enMovimiento = false;
            }
        }
    }

    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("axe"))

        {
            viking playerScript = collision.GetComponentInParent<viking>();
            if (playerScript != null && playerScript.atacando)
            {
                Vector2 direccionDanio = new Vector2(collision.gameObject.transform.position.x, 0);
                RecibeDano(direccionDanio, 1);
            }
                

        }
    }

    public void RecibeDano(Vector2 direccion, int cantidadDano)
    {
        if (muerto) return;

        recibiendoDano = true;
        vida -= cantidadDano;
        StartCoroutine(Parpadear());

        if (vida <= 0)
        {
            muerto = true;
            animator.SetBool("muerto", true);
            rb.velocity = Vector2.zero; // Detener movimiento.
            rb.isKinematic = true; // Evitar interacciones físicas.
            Invoke("HandleDeath", animator.GetCurrentAnimatorStateInfo(0).length);
            return;
        }

        Vector2 rebote = new Vector2(transform.position.x - direccion.x, 0.2f).normalized;
        rb.AddForce(rebote * fuerzaRebote, ForceMode2D.Impulse);
    }

    private void HandleDeath()
    {
        gameObject.SetActive(false); // Desactivar el enemigo.
    }



    private IEnumerator Parpadear()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        float duracionParpadeo = 0.3f; // Duración total del parpadeo en segundos.
        float intervaloParpadeo = 0.05f; // Tiempo entre cada parpadeo (apagado y encendido).

        float tiempo = 0f;

        while (tiempo < duracionParpadeo)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled; // Alternar visibilidad.
            yield return new WaitForSeconds(intervaloParpadeo);
            tiempo += intervaloParpadeo;
        }

        spriteRenderer.enabled = true; // Asegurarse de que el sprite esté visible al final.
    }


    private void detectarSuelo()
    {
        /*RaycastHit2D sueloDetectado = Physics2D.Raycast(detectorSuelo.position, Vector2.down, rangoDeteccionSuelo, capaSuelo);
        if (!sueloDetectado.collider)
        {
            Girar(); // Si no hay suelo, girar.
        }*/
        RaycastHit2D infosuelo = Physics2D.Raycast(detectorSuelo.position, Vector2.down, rangoDeteccionSuelo);
        rb.velocity = new Vector2(velocidadPatrulla, rb.velocity.y);

        if (!infosuelo)
        {
            //hay que girar al enemigo
            Girar();

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


        if (!recibiendoDano)
        {
            if (moviendoDerecha)
            {
                rb.velocity = new Vector2((moviendoDerecha ? velocidadPatrulla : -velocidadPatrulla), rb.velocity.y);
                detectarPared();
                detectarSuelo();
            }
            else
            {
                rb.velocity = new Vector2((moviendoDerecha ? velocidadPatrulla : -velocidadPatrulla), rb.velocity.y);
                detectarPared();
                detectarSuelo();
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
        detectarSuelo();
    }

    private void AtacarJugador()
    {
        rb.velocity = Vector2.zero;
        animator.SetBool("persiguiendo", false);
        animator.SetBool("atacando", true);
        

        if (Time.time - tiempoUltimoAtaque >= tiempoEntreAtaques)
        {
            tiempoUltimoAtaque = Time.time;
            Debug.Log("Atacando al jugador con daño: " + daño);
        }
    }

    private void Girar()
    {
        moviendoDerecha = !moviendoDerecha;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180);
        velocidadPatrulla *= -1;
    }




    private void FixedUpdate()
    {
        if (!muerto) detectarSuelo();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(detectorSuelo.transform.position, detectorSuelo.transform.position + Vector3.down * rangoDeteccionSuelo);
        Gizmos.DrawWireSphere(transform.position, rangoDeteccion);
    }

    public void ResetEnemy()
    {
        playerIsAlive = true;
        enMovimiento = true;
        persiguiendo = false;
        rb.velocity = Vector2.zero;
        animator.SetBool("patrullando", true);
        animator.SetBool("persiguiendo", false);
        animator.SetBool("atacando", false);
    }
}
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

    
    private Rigidbody2D RigidBody;
    private float xInput;

    void Start()
    {
        RigidBody = GetComponent<Rigidbody2D>();
       
    }

    void Update()
    {
        Debug.DrawRay(transform.position, Vector3.down * 0.6f, Color.red);
        
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
        // Capturamos teclado
        xInput = Input.GetAxisRaw("Horizontal");
        Vector2 move = new Vector2(xInput * speed, RigidBody.velocity.y);
        RigidBody.velocity = move;
    }

    //funcion que devuelve TRUE si está en el suelo
    private bool IsGrounded()
    {
        Ray2D ray = new Ray2D(groundCheck.position, Vector2.down);

        // Se lanza el rayo y se guarda la colisión
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 0.02f, whatIsGround);
        //Debug.DrawRay(ray.origin, ray.direction * 0.05f, Color.red);

        // Devuelve "true" si el rayo ha detectado suelo y "false" si no.
        //bool chocado = hit.collider; //util para el debug
        //Debug.Log("hit.collider: " + chocado);
        return hit.collider is not null;
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
}

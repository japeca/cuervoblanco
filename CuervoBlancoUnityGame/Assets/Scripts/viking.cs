using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class viking : MonoBehaviour
{
    public float Velocidad;
    public float Salto;
    public LayerMask groundLayer; // Crea una capa para el suelo

    private Rigidbody2D Rigidbody2D;
    private float Horizontal;
    private bool Grounded;
    //private bool isJumping;
    void Start()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Horizontal = Input.GetAxisRaw("Horizontal");

        Debug.DrawRay(transform.position, Vector3.down * 0.1f, Color.red);
        Grounded = Physics2D.OverlapCircle(transform.position - new Vector3(0, 0.5f, 0), 3.0f, groundLayer);

        if (Physics2D.Raycast(transform.position, Vector3.down, 3.0f))
        {
            Grounded = true;
        }
        else
        {
            Grounded = false;
        }



        //if (Input.GetKeyDown(KeyCode.W) && Grounded && !isJumping)
        if (Input.GetKeyDown(KeyCode.W) && Grounded)
        {
            Jump();
        }
    }


    private void Jump()
    {
        //isJumping = true;
        Rigidbody2D.AddForce(Vector2.up*Salto);
        //StartCoroutine(ResetJump());
    }

    /*private IEnumerator ResetJump()
    {
        yield return new WaitForSeconds(0.1f);
        isJumping = false;
    }*/

    private void FixedUpdate()
    {
        Rigidbody2D.velocity = new Vector2(Horizontal, Rigidbody2D.velocity.y);
    }
}

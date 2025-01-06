using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectorSuelo : MonoBehaviour
{
    public float velocidad;
    public Transform controladorSuelo;
    public float distancia;
    public bool movimientoDerecha;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        RaycastHit2D infosuelo = Physics2D.Raycast(controladorSuelo.position, Vector2.down,distancia) ;
        rb.velocity = new Vector2(velocidad, rb.velocity.y);

        if (infosuelo == false)
        {
            //hay que girar al enemigo
            Girar();

        }
    }

    private void Girar()
    {
        movimientoDerecha = !movimientoDerecha;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180);
        velocidad *= -1;
        //transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(controladorSuelo.transform.position, controladorSuelo.transform.position + Vector3.down * distancia);
    }

}

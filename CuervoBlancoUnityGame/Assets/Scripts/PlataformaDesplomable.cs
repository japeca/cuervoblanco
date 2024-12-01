using UnityEngine;

public class PlataformaDesplomable : MonoBehaviour
{
    public float tiempoAntesDeCaer = 1f; // Tiempo que tarda en caer despu�s de ser pisada.
    public float tiempoParaDestruir = 2f; // Tiempo que tarda en destruirse tras caer.

    private Rigidbody2D rb;
    private Vector3 posicionInicial; // Posici�n original de la plataforma.
    private Quaternion rotacionInicial; // Rotaci�n original de la plataforma
    private PlataformaManager plataformaManager;
        

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        posicionInicial = transform.position; // Guardamos la posici�n inicial.
        rotacionInicial = transform.rotation; // Guardamos la rotaci�n inicial.
        plataformaManager = FindObjectOfType<PlataformaManager>(); // Referencia al gestor de plataformas.
        if (plataformaManager != null)
        {
            plataformaManager.RegistrarPlataforma(this);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) // Detecta al jugador.
        {
            Vector2 normalColision = collision.contacts[0].normal;
            if (normalColision.y < -0.5f) // Umbral de pruebas. Normalmente con -1 valdria.
            {
                Invoke("HacerCaer", tiempoAntesDeCaer);
            }
        }
    }

    private void HacerCaer()
    {
        //Debug.Log("La plataforma empieza a caer");
        rb.gravityScale = 2f;
        rb.bodyType = RigidbodyType2D.Dynamic; // Cambia el Rigidbody a din�mico para que caiga.
        Invoke("DesactivarPlataforma", tiempoParaDestruir);
        if (plataformaManager != null)
        {
            plataformaManager.RegistrarDestruccion(this);
        }

    }

    private void DesactivarPlataforma()
    {
        gameObject.SetActive(false); //Desactiva el objeto
    }

    public void Regenerar()
    {
        transform.position = posicionInicial;
        transform.rotation = rotacionInicial; // Restauramos la rotaci�n original.
        rb.bodyType = RigidbodyType2D.Kinematic; // Volvemos a su estado inicial.
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f; // Reseteamos cualquier rotaci�n residual.
        gameObject.SetActive(true); // Reactivamos la plataforma.
    }

    public void DesactivarInmediatamente()
    {
        gameObject.SetActive(false);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    public Transform personaje;     // Referencia al Transform del personaje.
    public Transform puntoRespawn; // Referencia al Transform del punto de respawn.

    public void Respawn()
    {
        // Mueve el personaje al punto de respawn.
        personaje.position = puntoRespawn.position;

        // Opcional: Reinicia la velocidad del Rigidbody si el personaje tiene uno.
        Rigidbody2D rb = personaje.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
        }

        Debug.Log("Personaje respawneado en: " + puntoRespawn.position);
    }
}


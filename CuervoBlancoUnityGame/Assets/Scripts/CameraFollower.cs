using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    /*
     * Clase que tiene por objetivo controlar el seguimiento del personaje mediante la cámara.
     */
    
    public float speedCam = 0.025f;
    public Vector3 desplazamiento;
    public Transform Objetivo;
    public Transform respawnPoint;
    public float limiteCaida = -10f;


    private void LateUpdate()
    {
        if (Objetivo.position.y < limiteCaida)
        {
            RespawnPersonaje();
            return;
        }

        Vector3 posicionDeseada = Objetivo.position + desplazamiento;
        Vector3 posicionSuavizada = Vector3.Lerp(transform.position, posicionDeseada, speedCam);
        transform.position = posicionSuavizada;
    }

    private void RespawnPersonaje()
    {
        // Mueve el personaje al punto de reaparicion.
        Objetivo.position = respawnPoint.position;

       
        Rigidbody2D rb = Objetivo.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
        }

        //Ajustar la camara para posicionarla directamente sobre el personaje.
        transform.position = respawnPoint.position + desplazamiento;
    }

}
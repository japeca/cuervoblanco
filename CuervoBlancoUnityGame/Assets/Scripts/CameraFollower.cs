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


    private bool seguirPersonaje = true;


    private void LateUpdate()
    {
        //Se controla que haya siempre un objetivo a la que la camara debe seguir
        if (Objetivo == null)
        {
            Debug.LogError("No se ha asignado un Objetivo para la camara.");
            return;
        }

        if (!seguirPersonaje) return;

       

        Vector3 posicionDeseada = Objetivo.position + desplazamiento;
        Vector3 posicionSuavizada = Vector3.Lerp(transform.position, posicionDeseada, speedCam);
        transform.position = posicionSuavizada;
    }


    public void DetenerSeguimiento()
    {
        seguirPersonaje = false;
    }

    public void ReanudarSeguimiento()
    {
        seguirPersonaje = true;

    }

}
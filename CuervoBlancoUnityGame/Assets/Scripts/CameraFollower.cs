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


    private void LateUpdate()
    {
        Vector3 posicionDeseada = Objetivo.position + desplazamiento;
        Vector3 posicionSuavizada = Vector3.Lerp(transform.position, posicionDeseada, speedCam);
        transform.position = posicionSuavizada;
    }
}
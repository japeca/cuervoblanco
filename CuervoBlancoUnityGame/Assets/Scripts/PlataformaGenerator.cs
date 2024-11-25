using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformaGenerator : MonoBehaviour
{
    public GameObject plataformaPrefab; // Prefab de la plataforma.
    public int cantidadPlataformas = 5; // N�mero de plataformas a generar.
    public float distanciaMin = 2f; // Distancia m�nima entre plataformas.
    public float distanciaMax = 5f; // Distancia m�xima entre plataformas.
    public float alturaMax = 2f; // Altura m�xima entre plataformas.

    private Vector3 ultimaPosicion;

    void Start()
    {
        ultimaPosicion = transform.position;

        for (int i = 0; i < cantidadPlataformas; i++)
        {
            GenerarPlataforma();
        }
    }

    void GenerarPlataforma()
    {
        float distanciaX = Random.Range(distanciaMin, distanciaMax);
        float distanciaY = Random.Range(-alturaMax, alturaMax);

        Vector3 nuevaPosicion = ultimaPosicion + new Vector3(distanciaX, distanciaY, 0);

        if (nuevaPosicion.y < -4f) // Ajusta el valor seg�n el l�mite de ca�da de la escena.
        {
            nuevaPosicion.y = -4f; // Reajusta la altura m�nima.
        }


        Instantiate(plataformaPrefab, nuevaPosicion, Quaternion.identity);
        ultimaPosicion = nuevaPosicion;
    }
}

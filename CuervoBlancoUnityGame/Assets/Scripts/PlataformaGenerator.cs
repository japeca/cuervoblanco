using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformaGenerator : MonoBehaviour
{
    public GameObject plataformaPrefab; // Prefab de la plataforma.
    public int cantidadPlataformas = 5; // Número de plataformas a generar.
    public float distanciaMin = 2f; // Distancia mínima entre plataformas.
    public float distanciaMax = 5f; // Distancia máxima entre plataformas.
    public float alturaMax = 2f; // Altura máxima entre plataformas.

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

        if (nuevaPosicion.y < -4f) // Ajusta el valor según el límite de caída de la escena.
        {
            nuevaPosicion.y = -4f; // Reajusta la altura mínima.
        }


        Instantiate(plataformaPrefab, nuevaPosicion, Quaternion.identity);
        ultimaPosicion = nuevaPosicion;
    }
}

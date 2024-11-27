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
    public float caida = -1f; //parametro que indica la altura m�nima donde se generan las platformas en el eje y.

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

        if (nuevaPosicion.y < caida)
        {
            nuevaPosicion.y = caida + Random.Range(0.5f, 2f); ; // Reajusta la altura m�nima.
        }


        //se busca evitar el solapamiento entre plataformas. Se requiere saber su ancho.
        float anchoPlataforma = plataformaPrefab.GetComponent<Renderer>().bounds.size.x;

        // Asegurar que la nueva plataforma no se solape horizontalmente con la siguiente.
        if (nuevaPosicion.x < ultimaPosicion.x + anchoPlataforma)
        {
            nuevaPosicion.x = ultimaPosicion.x + anchoPlataforma + distanciaMin;
        }


        Instantiate(plataformaPrefab, nuevaPosicion, Quaternion.identity);
        ultimaPosicion = nuevaPosicion;
    }
}

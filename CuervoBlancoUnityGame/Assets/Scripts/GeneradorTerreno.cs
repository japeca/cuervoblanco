using UnityEngine;
using UnityEngine.Tilemaps;

public class GeneradorTerreno : MonoBehaviour
{
    public Tilemap tilemap; // Referencia al Tilemap donde se colocarán los tiles.
    public Tile baseTile; // Tile base para rellenar.
    public Tile tileSubida; // Tile para subidas.
    public Tile tileBajada; // Tile para bajadas.
    public Tile tileBordeIzquierdo; // Tile para el borde izquierdo.
    public Tile tileBordeDerecho; // Tile para el borde derecho.
    public Tile tileCentro; // Tile para partes intermedias.

    public int anchoTerreno = 50; // Ancho del terreno en tiles.
    public int alturaMaxima = 10; // Altura máxima del terreno en tiles.
    public float escala = 0.1f; // Escala para el Perlin Noise.
    public float seed = -1f; // Semilla para variar el terreno.
    public Transform puntoInicio; // El GameObject vacío que define el inicio del terreno.

    void Start()
    {
        if (seed < 0f)
        {
            seed = Random.Range(0f, 10000f); // Genera una semilla aleatoria.
            Debug.Log($"Semilla generada: {seed}");
        }

        GenerarTerreno();
    }

    void GenerarTerreno()
    {
        Vector3Int inicio = new Vector3Int(Mathf.RoundToInt(puntoInicio.position.x), Mathf.RoundToInt(puntoInicio.position.y), 0);

        // Generar alturas iniciales
        int[] alturas = new int[anchoTerreno];
        for (int x = 0; x < anchoTerreno; x++)
        {
            alturas[x] = Mathf.RoundToInt(Mathf.PerlinNoise((x + seed) * escala, 0) * alturaMaxima);
        }

        // Suavizar alturas
        SuavizarAlturas(alturas);

        int alturaAnterior = alturas[0];

        // Colocar tiles basados en alturas suavizadas
        for (int x = 0; x < anchoTerreno; x++)
        {
            int altura = alturas[x];

            // Determinar el tipo de tile para la primera altura. Interesa que sea subida, bajada o tile base/terreno plano.
            for (int y = 0; y <= altura; y++)
            {
                Vector3Int posicionTile = inicio + new Vector3Int(x, y, 0);
                Tile tileUsar = baseTile;

                if (y == altura) // Primera altura (superficie del jugador)
                {
                    if (x > 0 && altura > alturaAnterior) // Subida
                    {
                        tileUsar = tileSubida;
                    }
                    else if (x > 0 && altura < alturaAnterior) // Bajada
                    {
                        tileUsar = tileBajada;
                    }
                    else
                    {
                        tileUsar = baseTile; // Terreno plano
                    }
                }
                else // Alturas inferiores (relleno)
                {
                    bool tieneVecinoIzquierdo = x > 0 && alturas[x - 1] >= y;
                    bool tieneVecinoDerecho = x < anchoTerreno - 1 && alturas[x + 1] >= y;

                    if (!tieneVecinoIzquierdo && y > 0) // Borde izquierdo
                    {
                        tileUsar = tileBordeIzquierdo;
                    }
                    else if (!tieneVecinoDerecho && y > 0) // Borde derecho
                    {
                        tileUsar = tileBordeDerecho;
                    }
                    else if (y < altura && tilemap.GetTile(posicionTile + Vector3Int.up) == null) // Centro sin base encima
                    {
                        tileUsar = tileCentro;
                    }
                    else
                    {
                        tileUsar = baseTile; // Por defecto, base tile
                    }
                }

                // Colocar el tile calculado
                tilemap.SetTile(posicionTile, tileUsar);
            }

            alturaAnterior = altura; // Actualizar la altura anterior
        }
    }

    void SuavizarAlturas(int[] alturas)
    {
        for (int x = 1; x < alturas.Length - 1; x++)
        {
            // Calcular promedio con vecinos
            alturas[x] = Mathf.RoundToInt((alturas[x - 1] + alturas[x] + alturas[x + 1]) / 4f);
        }
    }
}

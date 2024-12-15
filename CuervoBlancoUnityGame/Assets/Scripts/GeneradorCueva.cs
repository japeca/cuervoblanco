using UnityEngine;
using UnityEngine.Tilemaps;

public class GeneradorSuelo : MonoBehaviour
{
    public Tilemap tilemap; // Tilemap donde se colocarán los tiles.
    public Tile tileSuelo; // Tile sólido para el suelo.
    public Tile tileAire;  // Tile vacío (opcional, puede ser nulo).

    public int ancho = 50; // Ancho del mapa.
    public int altura = 10; // Altura máxima del terreno.
    public float porcentajeLleno = 0.45f; // Probabilidad inicial de que un tile sea sólido.
    public int iteraciones = 5; // Iteraciones para suavizar.
    public Transform puntoInicio; // Punto de inicio para la generación.

    public int alturaMaximaPorSalto = 2; // Diferencia máxima de altura entre bloques adyacentes.
    public int distanciaMaxSalto = 3; // Máxima distancia horizontal alcanzable entre bloques.

    private int[,] mapa;

    void Start()
    {
        GenerarMapa();
        for (int i = 0; i < iteraciones; i++)
        {
            SuavizarMapa();
        }
        AjustarAlturaMaxima();
        AsegurarDistanciaHorizontal();
        DibujarSuelo();
    }

    // Inicializa el mapa.
    void GenerarMapa()
    {
        mapa = new int[ancho, altura];

        for (int x = 0; x < ancho; x++)
        {
            for (int y = 0; y < altura; y++)
            {
                mapa[x, y] = Random.value < porcentajeLleno ? 1 : 0;
            }
        }
    }

    // Aplica reglas para suavizar el mapa.
    void SuavizarMapa()
    {
        int[,] nuevoMapa = new int[ancho, altura];

        for (int x = 0; x < ancho; x++)
        {
            for (int y = 0; y < altura; y++)
            {
                int vecinosSólidos = ContarVecinosSólidos(x, y);

                if (mapa[x, y] == 1) // Si es sólido
                {
                    nuevoMapa[x, y] = (vecinosSólidos >= 4) ? 1 : 0;
                }
                else // Si es vacío
                {
                    nuevoMapa[x, y] = (vecinosSólidos >= 5) ? 1 : 0;
                }
            }
        }

        mapa = nuevoMapa;
    }

    // Ajusta la diferencia de altura entre bloques adyacentes.
    void AjustarAlturaMaxima()
    {
        for (int x = 1; x < ancho; x++)
        {
            int alturaActual = ObtenerAltura(x);
            int alturaAnterior = ObtenerAltura(x - 1);

            if (Mathf.Abs(alturaActual - alturaAnterior) > alturaMaximaPorSalto)
            {
                if (alturaActual > alturaAnterior)
                {
                    mapa[x, alturaAnterior + alturaMaximaPorSalto] = 1;
                }
                else
                {
                    mapa[x, alturaAnterior - alturaMaximaPorSalto] = 1;
                }

                for (int y = Mathf.Min(alturaActual, alturaAnterior) + 1; y <= Mathf.Max(alturaActual, alturaAnterior) - 1; y++)
                {
                    mapa[x, y] = 1;
                }
            }
        }
    }

    // Asegura que la distancia horizontal entre bloques sea alcanzable.
    void AsegurarDistanciaHorizontal()
    {
        for (int x = 1; x < ancho; x++)
        {
            int alturaActual = ObtenerAltura(x);
            int alturaAnterior = ObtenerAltura(x - 1);

            if (Mathf.Abs(x - (x - 1)) > distanciaMaxSalto)
            {
                // Inserta bloques intermedios para cubrir la distancia.
                for (int i = 1; i <= distanciaMaxSalto; i++)
                {
                    mapa[x - i, alturaAnterior] = 1; // Bloque intermedio a la altura del bloque anterior.
                }
            }
        }
    }

    // Obtiene la altura del suelo en una columna específica.
    int ObtenerAltura(int x)
    {
        for (int y = altura - 1; y >= 0; y--)
        {
            if (mapa[x, y] == 1) return y;
        }
        return 0;
    }

    // Cuenta los vecinos sólidos alrededor de una celda.
    int ContarVecinosSólidos(int x, int y)
    {
        int cuenta = 0;

        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                int nx = x + dx;
                int ny = y + dy;

                if (dx == 0 && dy == 0) continue;

                if (nx >= 0 && ny >= 0 && nx < ancho && ny < altura)
                {
                    cuenta += mapa[nx, ny];
                }
            }
        }

        return cuenta;
    }

    // Dibuja el terreno en el Tilemap.
    void DibujarSuelo()
    {
        tilemap.ClearAllTiles();
        Vector3Int posicionInicio = new Vector3Int(Mathf.RoundToInt(puntoInicio.position.x), Mathf.RoundToInt(puntoInicio.position.y), 0);

        for (int x = 0; x < ancho; x++)
        {
            for (int y = 0; y < altura; y++)
            {
                Vector3Int posicionTile = posicionInicio + new Vector3Int(x, y, 0);

                if (mapa[x, y] == 1)
                {
                    tilemap.SetTile(posicionTile, tileSuelo);
                }
                else if (tileAire != null)
                {
                    tilemap.SetTile(posicionTile, tileAire);
                }
            }
        }
    }
}

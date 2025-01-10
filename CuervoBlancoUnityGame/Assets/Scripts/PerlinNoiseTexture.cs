using UnityEngine;

public class PerlinNoiseTexture : MonoBehaviour
{
    [Header("Configuración de la textura")]
    public int width = 256; // Ancho de la textura
    public int height = 256; // Alto de la textura
    public float scale = 20f; // Escala del Perlin Noise

    public Renderer renderer; // Renderer del objeto donde se aplicará la textura

    void Start()
    {
        GenerateTexture();
    }

    void GenerateTexture()
    {
        // Crear una nueva textura
        Texture2D texture = new Texture2D(width, height);

        // Rellenar la textura con valores de Perlin Noise
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float xCoord = (float)x / width * scale;
                float yCoord = (float)y / height * scale;
                float sample = Mathf.PerlinNoise(xCoord, yCoord); // Generar valor de ruido
                texture.SetPixel(x, y, new Color(sample, sample, sample)); // Aplicar a la textura
            }
        }

        // Aplicar los cambios en la textura
        texture.Apply();

        // Asignar la textura al material del renderer
        if (renderer != null)
        {
            renderer.material.mainTexture = texture;
        }
    }
}

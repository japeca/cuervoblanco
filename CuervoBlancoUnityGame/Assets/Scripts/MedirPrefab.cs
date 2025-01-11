using UnityEngine;

public class MedirPrefab : MonoBehaviour
{
    public GameObject prefab;
    public float pixelsPerUnit = 100f; // Ajustar según tus Sprites.

    void Start()
    {
        if (prefab != null)
        {
            // Obtener el Bounds del Collider
            Collider2D collider = prefab.GetComponent<Collider2D>();
            if (collider != null)
            {
                Vector2 size = collider.bounds.size;
                Debug.Log($"Tamaño del prefab '{prefab.name}': X = {size.x} unidades, Y = {size.y} unidades");

                // Convertir a píxeles
                float pixelsX = size.x * pixelsPerUnit;
                float pixelsY = size.y * pixelsPerUnit;
                Debug.Log($"Tamaño en píxeles: X = {pixelsX}px, Y = {pixelsY}px");
            }
            else
            {
                Debug.LogWarning("El prefab no tiene un Collider2D.");
            }
        }
        else
        {
            Debug.LogError("No se asignó ningún prefab.");
        }
    }
}

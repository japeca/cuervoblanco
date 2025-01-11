using UnityEngine;

public class MedirPrefab : MonoBehaviour
{
    public GameObject prefab;
    public float pixelsPerUnit = 100f; // Ajustar seg�n tus Sprites.

    void Start()
    {
        if (prefab != null)
        {
            // Obtener el Bounds del Collider
            Collider2D collider = prefab.GetComponent<Collider2D>();
            if (collider != null)
            {
                Vector2 size = collider.bounds.size;
                Debug.Log($"Tama�o del prefab '{prefab.name}': X = {size.x} unidades, Y = {size.y} unidades");

                // Convertir a p�xeles
                float pixelsX = size.x * pixelsPerUnit;
                float pixelsY = size.y * pixelsPerUnit;
                Debug.Log($"Tama�o en p�xeles: X = {pixelsX}px, Y = {pixelsY}px");
            }
            else
            {
                Debug.LogWarning("El prefab no tiene un Collider2D.");
            }
        }
        else
        {
            Debug.LogError("No se asign� ning�n prefab.");
        }
    }
}

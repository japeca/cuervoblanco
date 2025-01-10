using UnityEngine;

public class GeneradorHabitaciones : MonoBehaviour
{
    [Header("Prefabs de Habitaciones")]
    public GameObject[] habitacionesPrefabs; // Array de prefabs de habitaciones.

    [Header("Configuración de Generación")]
    public Transform puntoInicio; // Punto donde comienza la generación de habitaciones.
    public float espacioEntreHabitaciones = 20f; // Espacio en X entre habitaciones.

    private void Start()
    {
        // Comprobar que hay habitaciones disponibles
        if (habitacionesPrefabs.Length == 0)
        {
            Debug.LogError("No se han asignado prefabs de habitaciones al GeneradorHabitaciones.");
            return;
        }

        // Generar habitaciones aleatorias
        GenerarHabitacionesAleatorias();
    }

    private void GenerarHabitacionesAleatorias()
    {
        // Crear una lista temporal para manejar la selección de habitaciones
        var habitacionesDisponibles = new System.Collections.Generic.List<GameObject>(habitacionesPrefabs);

        // Generar dos habitaciones
        for (int i = 0; i < 2; i++)
        {
            // Seleccionar una habitación aleatoriamente
            int indiceAleatorio = Random.Range(0, habitacionesDisponibles.Count);
            GameObject habitacionSeleccionada = habitacionesDisponibles[indiceAleatorio];

            // Instanciar la habitación en la posición correspondiente
            Vector3 posicion = puntoInicio.position + new Vector3(i * espacioEntreHabitaciones, 0, 0);
            Instantiate(habitacionSeleccionada, posicion, Quaternion.identity);

            // Eliminar la habitación seleccionada de la lista temporal para evitar repeticiones
            habitacionesDisponibles.RemoveAt(indiceAleatorio);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowEmitterController : MonoBehaviour
{
    /*
     * Clase que tiene por objetivo mover el emisor de nieve cuando el vikingo se mueve.
     */
    public Transform character;  // Vikingo
    public ParticleSystem snowParticleSystem;  // El sistema de partículas de nieve
    
    // Distancia en el eje X desde el personaje a la que se quiere que emitir la nieve
    public float distanceFromCharacter = 2f;  // Por defecto 2f, pero se puede ajustar

    private float previousX;

    void Start()
    {
        // Obtener la posición inicial del personaje
        previousX = character.position.x;
    }

    void Update()
    {
        // Mover el emisor con el personaje en el eje X, pero con la distancia configurada
        Vector3 newPosition = transform.position;
        newPosition.x = character.position.x + distanceFromCharacter; // Desplazamos en X en base a la distancia
        transform.position = newPosition;

        // Asegurarnos de que la nieve siempre venga desde la derecha
        AdjustSnowDirection();
    }

    // Método para ajustar la dirección de la nieve
    private void AdjustSnowDirection()
    {
        // Obtiene la velocidad en el eje X del personaje
        float characterSpeed = character.position.x - previousX;

        // Si el personaje se mueve a la derecha, la nieve viene desde la derecha
        if (characterSpeed > 0)
        {
            // Cambiamos la dirección de emisión de las partículas para que vengan desde la derecha
            var mainModule = snowParticleSystem.main;
            mainModule.startRotation = Mathf.Deg2Rad * 90f;  // Rotación de 90 grados (hacia la izquierda)
        }
        else if (characterSpeed < 0)
        {
            // Si el personaje se mueve a la izquierda, la nieve viene desde la izquierda
            var mainModule = snowParticleSystem.main;
            mainModule.startRotation = Mathf.Deg2Rad * -90f;  // Rotación de -90 grados (hacia la derecha)
        }

        // Actualizamos la posición anterior para el siguiente frame
        previousX = character.position.x;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Victory : MonoBehaviour
{
    
    [Header("Referencias")]
    public ParticleSystem[] lanzadoresConfeti; // Array de sistemas de partículas.
    public float duracionConfeti = 1f; // Duración del confeti.

    private bool nivelCompletado = false; // Evitar múltiples activaciones.
    private AudioSource audioSource;

    void Start()
    {
        // Inicializar la referencia
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !nivelCompletado)
        {
            nivelCompletado = true;
            ActivarConfeti();
            Invoke(nameof(FinalizarNivel), duracionConfeti);
            if (audioSource != null && audioSource.clip != null)
            {
                audioSource.Play();
            }
        }
    }

    private void ActivarConfeti()
    {
        StartCoroutine(ActivarConfetiSecuencial());
    }

    private IEnumerator ActivarConfetiSecuencial()
    {
        foreach (var lanzador in lanzadoresConfeti)
        {
            lanzador.Play(); // Activar cada lanzador.
            yield return new WaitForSeconds(0.5f); // Pausa de 0.5 segundos antes de activar el siguiente.
        }
        Debug.Log("¡Confeti activado! ¡Nivel completado!");
    }




    private void FinalizarNivel()
    {

        UnityEngine.SceneManagement.SceneManager.LoadScene("Victory");    
    }
}

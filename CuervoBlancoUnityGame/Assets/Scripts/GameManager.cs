using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instancia;
    

    [Header("Configuraciones del Juego")]
    public int maxMuertes = 3; // Número de muertes permitidas antes del Game Over
    public int muertesActuales = 0; // Contador de muertes
    private bool notificacionEnviada;


    

    private void Awake()
    {
        // Se asegura de que solo exista una instancia de GameManager
        if (instancia == null)
        {
            instancia = this;
            notificacionEnviada = false;
            DontDestroyOnLoad(gameObject); // Persistir entre escenas
            Debug.Log("GameManager inicializado.");
        }
        else
        {
            Debug.Log("GameManager duplicado destruido.");
            Destroy(gameObject); // Eliminar duplicados
        }
    }

    public void IncrementarMuertes()
    {
        muertesActuales++;
        
        Debug.Log("Muertes actuales: " + muertesActuales);


        if (muertesActuales >= maxMuertes)
        {
            MostrarGameOver();
        }
        else
        {
            ReiniciarNivel();
        }
    }

    private void ReiniciarNivel()
    {
        // Reiniciar la escena actual
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void MostrarGameOver()
    {
       


        // Cargar la escena de Game Over
        SceneManager.LoadScene("GameOver");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu")
        {
            ReiniciarEstado();
        }
    }

    public void ReiniciarEstado()
    {
        muertesActuales = 0;
        Debug.Log("Estado reiniciado: muertes actuales = " + muertesActuales);
        Debug.Log("Estado del GameManager reiniciado.");
    }

   

}
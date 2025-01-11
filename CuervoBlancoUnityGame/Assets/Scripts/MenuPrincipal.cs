using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MenuPrincipal : MonoBehaviour
{
    [Header("Referencias de UI")]
    public TMP_Dropdown selectorNivel; // Dropdown para seleccionar el nivel.
    public Button botonIniciar; // Botón para iniciar el juego.
    public Button botonSalir; // Botón para salir del juego.

    private int nivelSeleccionado = 0; // Nivel seleccionado por defecto.

    void Start()
    {
        // Asegúrate de que los listeners estén asignados correctamente.
        botonIniciar.onClick.AddListener(IniciarJuego);
        botonSalir.onClick.AddListener(SalirJuego);

        // Configurar las opciones del selector de niveles.
        ConfigurarSelectorNiveles();
    }

    void ConfigurarSelectorNiveles()
    {
        // Limpiar opciones existentes.
        selectorNivel.ClearOptions();

        // Crear una lista de opciones para los niveles, excluyendo ciertos índices.
        var opciones = new System.Collections.Generic.List<string>();
        int totalNiveles = SceneManager.sceneCountInBuildSettings;

        for (int i = 1; i < totalNiveles; i++) // Empieza desde el índice 1 para omitir el menú principal.
        {
            // Escena 5 es "Game Over" y 6 es "Vitoria" ).
            if (i != 5 && i != 6)
            {
                opciones.Add("Nivel " + (i - 1)); // Ajustar el número mostrado.
            }
        }

        // Añadir opciones al Dropdown.
        selectorNivel.AddOptions(opciones);

        // Configurar el nivel seleccionado.
        selectorNivel.onValueChanged.AddListener(delegate { CambiarNivel(selectorNivel.value); });

        // Configurar valor por defecto.
        selectorNivel.value = 0;
        nivelSeleccionado = 1; // Apunta al primer nivel jugable.
    }


    void CambiarNivel(int indice)
    {
        nivelSeleccionado = indice + 1;
        Debug.Log("Nivel seleccionado: " + nivelSeleccionado);
    }

    void IniciarJuego()
    {
        Debug.Log("Iniciando juego en nivel: " + nivelSeleccionado);
        if (GameManager.instancia != null)
        {
            GameManager.instancia.ReiniciarEstado();
        }
        SceneManager.LoadScene(nivelSeleccionado); //  Cargar la escena seleccionada.
    }

    void SalirJuego()
    {
        Debug.Log("Saliendo del juego... " );
        Application.Quit();
    }
}

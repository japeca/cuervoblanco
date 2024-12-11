using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool JuegoEnPausa = false;

    public GameObject menuPausaUI;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (JuegoEnPausa)
            {
                Reanudar();
            }
            else
            {
                Pausar();
            }
        }
    }

    public void Reanudar()
    {
        menuPausaUI.SetActive(false);
        Time.timeScale = 1f;
        JuegoEnPausa = false;
    }

    void Pausar()
    {
        menuPausaUI.SetActive(true);
        Time.timeScale = 0f;
        JuegoEnPausa = true;
    }

    public void ReiniciarNivel()
    {
        Time.timeScale = 1f; // Asegurarse de que el tiempo se restablece
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void CargarMenuPrincipal()
    {
        Time.timeScale = 1f;
        // Aquí cargaré la escena del menú principal cuando la tenga implementada
        // SceneManager.LoadScene("MenuPrincipal");
    }

    public void SalirDelJuego()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }

}

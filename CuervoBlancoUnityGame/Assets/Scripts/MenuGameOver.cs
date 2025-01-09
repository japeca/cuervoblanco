using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuGameOver : MonoBehaviour
{
    // Start is called before the first frame update
    public void Salir()
    {
        Application.Quit();
    }

    public void CargarMenuPrincipal(string nombre)
    {
        SceneManager.LoadScene(nombre);
    }
}

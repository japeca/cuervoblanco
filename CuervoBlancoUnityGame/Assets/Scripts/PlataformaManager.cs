using System.Collections.Generic;
using UnityEngine;

public class PlataformaManager : MonoBehaviour
{
    private List<PlataformaDesplomable> plataformas = new List<PlataformaDesplomable>();
    private List<PlataformaDesplomable> plataformasDestruidas = new List<PlataformaDesplomable>();

    // Registrar una plataforma al inicio.
    public void RegistrarPlataforma(PlataformaDesplomable plataforma)
    {
        if (!plataformas.Contains(plataforma))
        {
            plataformas.Add(plataforma);
        }
    }

    // Registrar una plataforma destruida.
    public void RegistrarDestruccion(PlataformaDesplomable plataforma)
    {
        if (!plataformasDestruidas.Contains(plataforma))
        {
            plataformasDestruidas.Add(plataforma);
        }
    }


    public void DesactivarPlataformasInmediatamente()
    {
        foreach (PlataformaDesplomable plataforma in plataformasDestruidas)
        {
            if (plataforma != null)
            {
                plataforma.DesactivarInmediatamente();
            }
        }
    }


    public void RegenerarPlataformas()
    {
        foreach (PlataformaDesplomable plataforma in plataformas)
        {
            if (plataforma != null)
            {
                plataforma.Regenerar();
            }
        }

        plataformasDestruidas.Clear(); // Limpiar la lista de plataformas destruidas.
    }
}

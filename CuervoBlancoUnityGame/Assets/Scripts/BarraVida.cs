using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarraVida : MonoBehaviour
{
    public Image rellenoVidaM;
    private viking playerController;
    private float vidaMaxima;

    private void Start()
    {
        playerController = GameObject.Find("Viking").GetComponent<viking>();
        vidaMaxima = playerController.vida;
    }

    private void Update()
    {
        rellenoVidaM.fillAmount = playerController.vida / vidaMaxima;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange_PlayerLogic : MonoBehaviour
{
    private void Awake()
    {
        // Evitar que este GameObject se destruya al cargar una nueva escena
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
            // Recuperar las coordenadas de la puerta guardadas en PlayerPrefs
            float puertaPosX = PlayerPrefs.GetFloat("PuertaPosX", 0f);
            float puertaPosY = PlayerPrefs.GetFloat("PuertaPosY", 0f);
            float puertaPosZ = PlayerPrefs.GetFloat("PuertaPosZ", 0f);

            // Colocar al jugador en la posición de la puerta guardada
            //transform.position = new Vector3(puertaPosX, puertaPosY, puertaPosZ);
    }
}

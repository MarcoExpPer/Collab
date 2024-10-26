using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Trigger_Exit : MonoBehaviour
{
    public string sceneToLoad; // Nombre de la escena exterior de la casa

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Cambiar a la escena exterior de la casa
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}

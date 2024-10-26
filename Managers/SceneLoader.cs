using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using VInspector;

public class SceneLoader : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private string sceneToLoad;
    [SerializeField] private bool hasPointToReturn;
    
    [ShowIf("hasPointToReturn")]
    [SerializeField] private Transform pointToReturn; 
    private GameObject parent; 
    private string currentScene;
    [SerializeField] private DoorIdentifier doorIdentifier;
    [EndIf]

    public static event Action<string, string, List<string>, List<Transform>> SavePointToReturnEvent;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        currentScene = SceneManager.GetActiveScene().name;

        GameObject parentObject = transform.parent != null ? transform.parent.gameObject : null;
        Debug.Log("[Teleport Trigger] " + parentObject);

        if (hasPointToReturn && parentObject != null)
        {
            List<string> childIdentifiers = new List<string>();
            List<Transform> childPositions = new List<Transform>();
            List<Quaternion> childRotations = new List<Quaternion>();
            
            currentScene = SceneManager.GetActiveScene().name;
            int childCount = parentObject.transform.childCount;

            for (int i = 0; i < childCount; i++)
            {
                Transform child = parentObject.transform.GetChild(i);

                SceneLoader childSceneLoader = child.GetComponent<SceneLoader>();
                
                if (childSceneLoader != null)
                {
                    childIdentifiers.Add(childSceneLoader.doorIdentifier.ToString());
                    childPositions.Add(childSceneLoader.pointToReturn);
                }
            }

            SavePointToReturnEvent?.Invoke(currentScene, doorIdentifier.ToString(), childIdentifiers, childPositions);
        }

        //OnSceneWillChange?.Invoke(sceneToLoad);
        
        SceneManager.LoadScene(sceneToLoad);
    }

    public string GetIdDoor()
    {
        return doorIdentifier.ToString();
    }
    public Transform GetPointToReturn()
    {
        return pointToReturn;
    }
}
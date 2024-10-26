using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    //Tags
    public static string PlayerShieldHandTag = "PlayerShieldHand";
    public static string PlayerSwordHandTag = "PlayerSwordHand";
    public static string PlayerTag = "Player";
    public static string HitableTag = "Hitable";
    public static string PressurePlateTag = "PressurePlateTrigger";
    //Layers
    public static int PlayerLayer = 6;
    public static int PlayerDetectorLayer = 8;
    
    
    //Managers
    [HideInInspector] public static GameManager Instance { get; private set; }
    [HideInInspector] public UIManager uiManager;
    [HideInInspector] public InputManager inputManager;

    
    //Player common components
    [FormerlySerializedAs("playerController")] [HideInInspector] public PlayerSMController playerSmController;
    [HideInInspector] public ItemController itemController;
    [HideInInspector] public CameraController cameraController;
    [HideInInspector] public PlayerSpawnController spawnController;
    
    //Scriptable Objects
    public PlayerData playerData;
    
    [Header("Positions and scenes to return")]

    public static Action<Vector3, Vector3> NotifyTransformToPlayer;

    private string lastIdDoor;

    public class TransformData
    {
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }

        public TransformData(Vector3 position, Quaternion rotation)
        {
            Position = position;
            Rotation = rotation;
        }
    }
    private Dictionary<string, Dictionary<string, TransformData>> sceneAndPointToReturn = new Dictionary<string, Dictionary<string, TransformData>>();


    
    void Awake()
    {
        if(Instance == null)
        {
            inputManager = GetComponentInChildren<InputManager>();
            uiManager = GetComponent<UIManager>();
            
            //TODo Temporal fix to dictionaries not being cleared between executions in editor
            playerData.ClearData();
            SetupPlayerOnLevel();
            
            
            Instance = this;
            
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    private void SetupPlayerOnLevel()
    {
        
    }
    private void OnEnable ()
    {
        SceneLoader.SavePointToReturnEvent += SavePointToReturnOnSceneChanged;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable ()
    {
        SceneLoader.SavePointToReturnEvent -= SavePointToReturnOnSceneChanged;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void SavePointToReturnOnSceneChanged(string sceneReceived, string idDoor, List<string> childIdentifiers, List<Transform> childPositions)
    {
        lastIdDoor = idDoor;

        Dictionary<string, TransformData> childTransformsMap = new Dictionary<string, TransformData>();

        for (int i = 0; i < childIdentifiers.Count; i++)
        {
            TransformData transformData = new TransformData(childPositions[i].position, childPositions[i].rotation);

            childTransformsMap[childIdentifiers[i]] = transformData;
        }

        if (!sceneAndPointToReturn.ContainsKey(sceneReceived))
        {
            sceneAndPointToReturn[sceneReceived] = childTransformsMap;
        }
        else
        {
            sceneAndPointToReturn[sceneReceived] = childTransformsMap;
        }
    }

    private void CheckIfThisSceneHasPointToReturn(string sceneToReturn)
    {
        if (!sceneAndPointToReturn.ContainsKey(sceneToReturn))
        {
            FindSpawnPointFromSceneChangeObjects();
        }
        else
        {
            Dictionary<string, TransformData> childTransformsMap = sceneAndPointToReturn[sceneToReturn];

            foreach (var kvp in childTransformsMap)
            {
                string childIdentifier = kvp.Key;
                if(childIdentifier == lastIdDoor)
                {
                    TransformData transformData = kvp.Value;

                    Debug.Log($"Notificando al jugador: ID={childIdentifier}, Posición={transformData.Position}, Rotación={transformData.Rotation}");
                    NotifyTransformToPlayer?.Invoke(transformData.Position, transformData.Rotation.eulerAngles);
                }
            }
        }
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        CheckIfThisSceneHasPointToReturn(scene.name);
    }

    private void FindSpawnPointFromSceneChangeObjects()
    {
        GameObject[] sceneChangeObjects = GameObject.FindGameObjectsWithTag("SceneChanger");

        foreach (var sceneChangeObject in sceneChangeObjects)
        {
            SceneLoader sceneLoader = sceneChangeObject.GetComponent<SceneLoader>();

            if (sceneLoader != null)
            {
                if (sceneLoader.GetIdDoor() == lastIdDoor)
                {
                    Transform aux = sceneLoader.GetPointToReturn();

                    Vector3 spawnPosition = aux.position;
                    Vector3 spawnRotation = aux.rotation.eulerAngles;

                    NotifyTransformToPlayer?.Invoke(spawnPosition, spawnRotation);
                    break;
                }
            }
        }
    }

}

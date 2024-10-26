using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    public bool Detector_X;
    private string posicion = "";
    private string otherCollision = "";
    private GroundMovementController groundMovementController;
    
    private bool blockRight = false;
    private bool blockLeft = false;
    private bool blockBack = false;
    private bool blockFront = false;

    private MovableObject movableObject;
    private bool isHeavy;

    private void Awake()
    {
        MovableObject movableObject = transform.parent.GetComponent<MovableObject>();
        isHeavy = movableObject.IsHeavy;
    }

    private List<GameObject> obstacleObjects = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(!isHeavy){
                StartDragging(other);
            }
            else
            {
                bool hasPulseraFuerza = GameManager.Instance.playerData.HasPassiveItem(EItemId.PulseraFuerza);
                if(hasPulseraFuerza)
                {
                    StartDragging(other);
                }
                else
                {
                    Debug.Log("Pesa Demasiado");
                }
            }
        }
        else
        {
            SaveObstacle(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<GroundMovementController>().PlayerStopDraggingObject();
        }
        else
        {
            if (obstacleObjects.Contains(other.gameObject))
            {
                obstacleObjects.Remove(other.gameObject);

                otherCollision = DetectCollisionSide(other);
                if (otherCollision == "left")
                {
                    blockRight = false;
                }
                else if (otherCollision == "right")
                {
                    blockLeft = false;
                }
                else if (otherCollision == "back")
                {
                    blockFront = false;
                }
                else if (otherCollision == "front")
                {
                    blockBack = false;
                }
            }
        }
    }

    private void StartDragging(Collider other)
    {
        posicion = DetectCollisionSide(other);
        groundMovementController = other.gameObject.GetComponent<GroundMovementController>();
        if (posicion == "left" && !blockLeft)
        {
            groundMovementController.PlayerStartDraggingObject(Detector_X, posicion, transform.parent.gameObject);
        }
        else if (posicion == "right" && !blockRight)
        {
            groundMovementController.PlayerStartDraggingObject(Detector_X, posicion, transform.parent.gameObject);
        }
        else if (posicion == "back" && !blockBack)
        {
            groundMovementController.PlayerStartDraggingObject(Detector_X, posicion, transform.parent.gameObject);
        }
        else if (posicion == "front" && !blockFront)
        {
            groundMovementController.PlayerStartDraggingObject(Detector_X, posicion, transform.parent.gameObject);
        }
    }

    private void SaveObstacle(Collider obstacle)
    {
        string obstacleCollision = DetectCollisionSide(obstacle);
        obstacleObjects.Add(obstacle.gameObject); 
        if(posicion == "left" && obstacleCollision == "right")
        {
            blockLeft = true;
            if(groundMovementController != null)
            {
                groundMovementController.PlayerStopDraggingObject();
            }
        }
        else if(posicion == "right" && obstacleCollision == "left")
        {
            blockRight = true;
            if(groundMovementController != null)
            {
                groundMovementController.PlayerStopDraggingObject();
            }
        }
        else if(posicion == "back" && obstacleCollision == "front")
        {
            blockBack = true;
            if(groundMovementController != null)
            {
                groundMovementController.PlayerStopDraggingObject();
            }
        }
        else if(posicion == "front" && obstacleCollision == "back")
        {
            blockFront = true;
            if(groundMovementController != null)
            {
                groundMovementController.PlayerStopDraggingObject();
            }
        }
    }

    private string DetectCollisionSide(Collider other)
    {
        string posicionCollision = "";
        Vector3 otherPosition = other.transform.position;
        Bounds bounds = GetComponent<BoxCollider>().bounds;
        if(Detector_X)
        {
            if (Mathf.Abs(otherPosition.x - bounds.min.x) < Mathf.Abs(otherPosition.x - bounds.max.x))
            {
                posicionCollision = "left";
            }
            else if (Mathf.Abs(otherPosition.x - bounds.max.x) < Mathf.Abs(otherPosition.x - bounds.min.x))
            {
                posicionCollision = "right";
                
            }
        }
        else
        {
            if (Mathf.Abs(otherPosition.z - bounds.min.z) < Mathf.Abs(otherPosition.z - bounds.max.z))
            {
                posicionCollision = "back";
            }
            else if (Mathf.Abs(otherPosition.z - bounds.max.z) < Mathf.Abs(otherPosition.z - bounds.min.z))
            {
                posicionCollision = "front";
            }
        }
        return posicionCollision;
    }
}

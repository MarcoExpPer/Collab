using System;
using UnityEngine;

/*
 * This class controls player movement while in the ground. Basic walk/run
 */
public class GroundMovementController : MonoBehaviour
{
    public CharacterController Controller { get; private set; }
    private float _turnVelocity;
    private Vector3 _velocity;
    
    public float speedMovement = 6;
    public float speedDragging = 2;
    public float turnTime = 0.2f;


    private bool dragging = false;
    private bool draggingLeft = false; 
    private bool draggingRight = false; 
    private bool draggingFront = false; 
    private bool draggingBack = false; 
    private bool objectOtherSide = false;

    private MovableObject movableObject;

    private void Awake()
    {
        Controller = GetComponent<CharacterController>();
       
    }

    public void Walk(Vector2 move)
    {
        float X = move.x;
        float Z = move.y;

        Vector3 direction = new Vector3(X, 0, Z);

        if (direction != Vector3.zero)
        {
            Vector3 movementDirection = DirectionOfMovement(direction);
            Controller.Move(movementDirection.normalized * (speedMovement * Time.deltaTime));
        }
    }
    
    //Currently used in debug fly functionality. It requires planning if we want to use ti for gameplay flying purpouses
    public void Fly(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            float rotationAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, rotationAngle, ref _turnVelocity, turnTime);
            
            transform.rotation = Quaternion.Euler(0, angle, 0);
            Controller.Move(direction.normalized * (speedMovement * Time.deltaTime));
        }
    }
    
    public void DragObject(Vector2 move)
    {
        Vector3 direction = Vector3.zero;
        float Z = move.y;
        float X = move.x;

        if(draggingFront || draggingBack)
        {
            direction = new Vector3(0, 0, Z);
        }
        else if(draggingLeft || draggingRight)
        {
            direction = new Vector3(X, 0, 0);
        }
        if (direction != Vector3.zero)
        {
            Vector3 movementDirection = DirectionOfMovement(direction);
            Controller.Move(movementDirection.normalized * (speedDragging * Time.deltaTime));

            ComprobarObjectMovement(movementDirection);
        }
    }

    private Vector3 DirectionOfMovement(Vector3 direction)
    {
        float rotationAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, rotationAngle, ref _turnVelocity, turnTime);
            
        transform.rotation = Quaternion.Euler(0, angle, 0);
        Vector3 directionOfMovement = Quaternion.Euler(0, rotationAngle, 0) * Vector3.forward;
        return directionOfMovement;
    }

    private void ComprobarObjectMovement(Vector3 direccionMovimiento)
    {
        if(movableObject != null && !objectOtherSide)
        {
            if(draggingFront)
            {
                if(direccionMovimiento.normalized.z > 0)
                {
                    MoveObject(direccionMovimiento.normalized);
                }
                else if(direccionMovimiento.normalized.z < 0)
                {
                    PlayerStopDraggingObject();
                }
            }
            else if(draggingBack)
            {
                if(direccionMovimiento.normalized.z < 0)
                {
                    MoveObject(direccionMovimiento.normalized);
                }
                else if(direccionMovimiento.normalized.z > 0)
                {
                    PlayerStopDraggingObject();
                }
            }
            else if(draggingRight)
            {
                if(direccionMovimiento.normalized.x > 0)
                {
                    MoveObject(direccionMovimiento.normalized);
                }
                else if(direccionMovimiento.normalized.x < 0)
                {
                    PlayerStopDraggingObject();
                }
            }
            else if(draggingLeft)
            {
                if(direccionMovimiento.normalized.x < 0)
                {
                    MoveObject(direccionMovimiento.normalized);
                }
                else if(direccionMovimiento.normalized.x > 0)
                {
                    PlayerStopDraggingObject();
                }                
            }
        }
    }

    private void MoveObject(Vector3 movementDirection)
    {
        movableObject.MoveObject(movementDirection, speedDragging);
    }

    public void PlayerStartDraggingObject(bool direccion, string posicion, GameObject draggableObject)
    {
        movableObject = draggableObject.GetComponent<MovableObject>();
        if(direccion)
        {
            if(posicion == "left")
            {
                draggingRight = true;
            }
            if(posicion == "right")
            {
                draggingLeft = true;
            }
        }
        else
        {
            if(posicion == "back")
            {
                draggingFront = true;
            }
            if(posicion == "front")
            {
                draggingBack = true;
            }
        }
        dragging = true;
    }
    public void PlayerStopDraggingObject()
    {
        dragging = false;
        draggingLeft = false;
        draggingRight = false;
        draggingBack = false;
        draggingFront = false;
    }

    public bool isDragging()
    {
        return dragging;
    }

    public void collisionWithOtherObject(bool collisionObject)
    {
        objectOtherSide = collisionObject;
    }
}

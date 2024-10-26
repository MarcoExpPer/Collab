using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObject : MonoBehaviour, IMovableObject
{
    [SerializeField] private bool isHeavy;
    public bool IsHeavy => isHeavy;
    public void MoveObject(Vector3 moveDirection, float moveSpeed)
    {
        transform.parent.position += moveDirection * (moveSpeed * Time.deltaTime);
    }

}
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using VInspector;

public static class Utils
{
    public static bool DoesArrayContainT<TClassToFind, TArrayClass>(TArrayClass[] arrayItems)
    {
        foreach (TArrayClass item in arrayItems)
        {
            if (item != null)
            {
                if (item.GetType() == typeof(TClassToFind))
                {
                    return true;
                }
            }

        }
        
        return false;
    }
    
    public static bool DoesArrayContainT<TClassToFind, TArrayClass>(List<TArrayClass> arrayItems)
    {
        foreach (TArrayClass item in arrayItems)
        {
            if (item != null)
            {
                if (item.GetType() == typeof(TClassToFind))
                {
                    return true;
                }
            }

        }
        
        return false;
    }
    
    [System.Serializable]
    public struct ColliderSpawnInformation 
    {
        public bool isSphereCollider;
        [ShowIf("isSphereCollider")]
        public float radius;
        [HideInInspector] public string colliderGameObjectName;
    }
    
    public static PlayerTrigger FindOrCreatePlayerTriggerToGameObject(GameObject targetGameObject,
        ColliderSpawnInformation spawnInfo)
    {
        GameObject parentGameObject = null;
        foreach (Transform child in targetGameObject.transform)
        {
            if (child.gameObject.name == spawnInfo.colliderGameObjectName)
            {
                parentGameObject = child.gameObject;
            }
        }

        if (!parentGameObject)
        {
            Type colliderType = spawnInfo.isSphereCollider ? typeof(SphereCollider) : typeof(BoxCollider);
            Type[] components = { colliderType, typeof(PlayerTrigger) };

            parentGameObject = new GameObject(spawnInfo.colliderGameObjectName, components);
            parentGameObject.transform.SetParent(targetGameObject.transform);
            parentGameObject.transform.localPosition = Vector3.zero;
        }

        if (spawnInfo.isSphereCollider)
        {
            SphereCollider detectorSphere = parentGameObject.GetComponent<SphereCollider>();
            detectorSphere.radius = spawnInfo.radius != 0 ? spawnInfo.radius : 1;
        }
        
        parentGameObject.layer = GameManager.PlayerDetectorLayer;
        
       return parentGameObject.GetComponent<PlayerTrigger>();
    }
    
    public static void DrawDebugBox(Vector3 center, Vector3 halfExtents, Quaternion orientation)
    {
        Vector3[] points = new Vector3[8];

        // Calculate the corners of the box
        Vector3 extents = halfExtents;

        points[0] = center + orientation * new Vector3(extents.x, extents.y, extents.z);
        points[1] = center + orientation * new Vector3(-extents.x, extents.y, extents.z);
        points[2] = center + orientation * new Vector3(extents.x, -extents.y, extents.z);
        points[3] = center + orientation * new Vector3(-extents.x, -extents.y, extents.z);
        points[4] = center + orientation * new Vector3(extents.x, extents.y, -extents.z);
        points[5] = center + orientation * new Vector3(-extents.x, extents.y, -extents.z);
        points[6] = center + orientation * new Vector3(extents.x, -extents.y, -extents.z);
        points[7] = center + orientation * new Vector3(-extents.x, -extents.y, -extents.z);

        Debug.DrawLine(points[0], points[1], Color.green, 1);
        Debug.DrawLine(points[1], points[3], Color.green, 1);
        Debug.DrawLine(points[3], points[2], Color.green, 1);
        Debug.DrawLine(points[2], points[0], Color.green, 1);

        Debug.DrawLine(points[4], points[5], Color.green,1);
        Debug.DrawLine(points[5], points[7], Color.green,1);
        Debug.DrawLine(points[7], points[6], Color.green,1);
        Debug.DrawLine(points[6], points[4], Color.green,1);

        Debug.DrawLine(points[0], points[4], Color.green, 1);
        Debug.DrawLine(points[1], points[5], Color.green, 1);
        Debug.DrawLine(points[2], points[6], Color.green, 1);
        Debug.DrawLine(points[3], points[7], Color.green, 1);
    }
}
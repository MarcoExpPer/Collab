using System.Collections.Generic;
using UnityEngine;

//This script is not intended to be used in game. Only for debug purpouses
public class DebugObtainableItemDatabase : ScriptableObject
{
    [SerializeField] public List<ObtainableItem> obtainableItems = new List<ObtainableItem>();
}
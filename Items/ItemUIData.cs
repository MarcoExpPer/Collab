using UnityEngine;

[CreateAssetMenu(menuName = "Nilsh/Inventory/ItemUIData")]
public class ItemUIData : ScriptableObject
{
    public string itemName;
    public string itemDescription;
    public Texture2D texture;
}
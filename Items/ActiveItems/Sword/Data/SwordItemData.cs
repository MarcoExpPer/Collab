using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Nilsh/Inventory/Items/SwordItemData")]
public class SwordItemData : ActiveItemData
{
    public HitEffect_SO[] swordEffects;
    
    public int damage = 10;
    public GameObject SwordPrefab;
    
    public override void ObtainItem(GameObject ownerGameObject)
    {
        ownerGameObject.AddComponent<ActiveSword>().OnObtained(this);
    }
    
    public override EItemId GetItemId()
    {
        return EItemId.Sword;
    }
    
}
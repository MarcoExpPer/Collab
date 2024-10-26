using UnityEngine;

[CreateAssetMenu(menuName = "Nilsh/Inventory/Items/BombItemData")]
public class BombItemData : ActiveItemData
{
    public HitEffect_SO[] explosionEffects;
    public Bomb_Explosion bombaPrefab;
    public float distanciaSpawn = 2f;
    public HitEffect_SO[] hitEffectsSo;
    public float throwCooldown = 2.0f;
        
    public override void ObtainItem(GameObject ownerGameObject)
    {
        ownerGameObject.AddComponent<ActiveBombs>().OnObtained(this);
    }
    
    public override EItemId GetItemId()
    {
        return EItemId.Bombs;
    }
}
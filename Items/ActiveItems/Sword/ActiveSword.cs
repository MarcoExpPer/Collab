
using UnityEngine;

public class ActiveSword : ActiveItem
{
    private SwordItemData _swordData;
    private SwordController _sword;
    
    public override void OnObtained( ActiveItemData itemData )
    {
        CurrentRigSlot = ERigItemSlot.SwordHand;
        currentEquipedItemSlot = EEquipedItemSlot.Sword;
        
        _swordData = (SwordItemData) itemData;
        
        base.OnObtained(itemData);
        
        Item.EquipItem(_swordData.GetItemId(), EEquipedItemSlot.Sword);
        FindPlayerSlotAndInstantiatePrefab(GameManager.PlayerSwordHandTag, _swordData.SwordPrefab);
        
        //Find the sword collider to add the controller script to it
        foreach (var boxCollider in transform.root.GetComponentsInChildren<BoxCollider>())
        {
            if (boxCollider.CompareTag("PlayerSwordCollider"))
            {
                _sword = boxCollider.gameObject.GetComponent<SwordController>();
                _sword.swordData = _swordData;
                _sword.enabled = true;
                boxCollider.enabled = false;
                _sword.slashCollider = boxCollider;
                break;
            }
        }
    }

    public override bool OnInput(bool inputValue)
    {
        if (!base.OnInput(inputValue)) return false;

        if (!inputValue) return false;
        
        if (!RigGameObject.activeSelf)
        {
            Item.onRigSlotUpdated.Invoke( CurrentRigSlot , this);
            RigGameObject.SetActive(true);
        }
        return _sword.TryStartAttackAnimation();
    }
    
    public void DoAttackDamage()
    {
        _sword.DoAttackDamage();
    }
    public void EndAttackConstraints()
    {
        _sword.EndAttackConstraints();
    }
}

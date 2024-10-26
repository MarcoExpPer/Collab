using System;
using System.Linq;
using UnityEngine;
using VInspector;
using UnityEngine.Serialization;

public class CalderoDisipadorNiebla : InteractionSource
{
    [Header("Caldero")]
    [SerializeField] ParticleSystem fogParticleSystem;
    [SerializeField] float dissipationRate = 0.5f;
    public GameObject CalderoLight;
    
    public override void ExecuteInteraction(InteractionController controller)
    {
        controller.EndInteraction();
        DissipateFog();
        if (CalderoLight != null)
        {
            CalderoLight.SetActive(true);
        }
        Destroy(transform.gameObject);
    }
    
    private void DissipateFog()
    {
        if (fogParticleSystem != null)
        {
            var emission = fogParticleSystem.emission;
            
            emission.rateOverTime = emission.rateOverTime.constant * dissipationRate;
        }
    }

    public override bool CanBeInteractedWith()
    {
        if (base.CanBeInteractedWith())
        {
            bool isEquipped = Utils.DoesArrayContainT<CandleItemData, ActiveItemData>(GameManager.Instance.playerData.EquipedItemsData);;

            if (isEquipped)
            {
                return GameManager.Instance.itemController.GetActiveItem(EItemId.Candle).IsRigObjectActive();
            }
            
        }

        return false;
    }
}

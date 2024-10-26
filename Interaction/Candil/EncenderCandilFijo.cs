using System;
using System.Linq;
using UnityEngine;
using VInspector;
using UnityEngine.Serialization;

public class EncenderCandilFijo : InteractionSource
{
    [Header("Luz")]
    public GameObject candilLight;
    private bool muevePlataforma = false;
    private MoverPlataforma plataformaController;
    
    public override void Start()
    {
        base.Start();
        candilLight.SetActive(false);
    }
    public void SetplataformaController(MoverPlataforma moverPlataforma)
    {
        muevePlataforma = true;
        plataformaController = moverPlataforma;
    }

    public override void ExecuteInteraction(InteractionController controller)
    {
        controller.EndInteraction();
        if (candilLight != null)
        {
            candilLight.SetActive(true);
            if(muevePlataforma)
            {
                NotifyPlatformController();
            }
        }
        Destroy(transform.gameObject);
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

    private void NotifyPlatformController()
    {
        if (plataformaController != null)
        {
            plataformaController.CandilEncendido();
        }
    }
}

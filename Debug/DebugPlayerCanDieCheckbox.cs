using UnityEngine;
using UnityEngine.UI;

public class DebugPlayerCanDieCheckbox : DebugButton
{
    private Image checkboxImage;
    public Color activeColor = Color.green;
    public Color deactiveColor = Color.red;
    
    private PlayerCombatComponent playerCombat;
    private bool immunityActive = false;
    public void Awake()
    {
        checkboxImage = GetComponent<Image>();
    }

    public void Start()
    {
        playerCombat = GameManager.Instance.itemController.gameObject.GetComponent<PlayerCombatComponent>();
        checkboxImage.color = !immunityActive ? deactiveColor : activeColor;
    }

    public override void OnClick()
    {
        immunityActive = !immunityActive;

        for (int i = 0; i < playerCombat.elementalImmunities.Length; i++)
        {
            playerCombat.elementalImmunities[i] = immunityActive;
        }
        
        
        checkboxImage.color = !immunityActive ? deactiveColor : activeColor;
    }
}

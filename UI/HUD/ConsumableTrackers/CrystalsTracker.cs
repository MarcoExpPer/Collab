
using Unity.VisualScripting;
using UnityEngine;


public class CrystalsTracker : ConsumableTracker
{
    
    public Color normalColor = Color.white;
    public Color emptyColor = Color.grey;
    
    public override void SetupPlayerData(PlayerData playerData)
    {
        playerData.OnCrystalsUpdated.AddListener(OnCrystalsUpdated);
        OnCrystalsUpdated(playerData.crystals);
    }
    
    private void OnCrystalsUpdated(int currentAmount)
    {
        if (_hideCoroutine != null)
        {
            StopCoroutine(_hideCoroutine);
        }
        
        _text.text = currentAmount.ToString();
        _text.color = currentAmount == 0 ? emptyColor : normalColor;

        ToggleEnables(true);
        _hideCoroutine = StartCoroutine(HideText());
    }
}

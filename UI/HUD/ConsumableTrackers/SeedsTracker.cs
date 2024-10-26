
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;


public class SeedsTracker : ConsumableTracker
{
    
    public Color normalColor = Color.white;
    public Color fullColor = Color.green;
    public override void SetupPlayerData(PlayerData playerData)
    {
        playerData.OnSeedsUpdated.AddListener(OnSeedsUpdated);
        OnSeedsUpdated(playerData.seeds, playerData.maxSeeds);
    }
    
    private void OnSeedsUpdated(int currentAmount, int maxAmount)
    {
        if (_hideCoroutine != null)
        {
            StopCoroutine(_hideCoroutine);
        }
        
        _text.text = currentAmount.ToString();
        _text.color = currentAmount == maxAmount ? fullColor : normalColor;

        ToggleEnables(true);
        _hideCoroutine = StartCoroutine(HideText());
    }
}

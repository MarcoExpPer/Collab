using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;


public class GoldenKeyTracker : ConsumableTracker
{
    public Color normalColor = Color.white;
    public Color emptyColor = Color.grey;

    public override void SetupPlayerData(PlayerData playerData)
    {
        playerData.OnGoldenKeysUpdated.AddListener(OnGoldenKeysUpdated);
        OnGoldenKeysUpdated(playerData.goldenKeys);
    }

    private void OnGoldenKeysUpdated(int currentAmount)
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

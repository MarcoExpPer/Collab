using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public enum ELifeState
{
    Full,
    Half,
    Empty
}
[RequireComponent(typeof(Image))]
public class SingleLifeIcon : MonoBehaviour
{
    private Image _hearthImage;
    
    public Sprite fullHearth;
    public Sprite midHearth;
    public Sprite emptyHearth;

    public void SetState(ELifeState newState)
    {
        _hearthImage = GetComponent<Image>();
        
        //No ni que esto existia, lol. Me ha salido como sugerencia en el IDE
        _hearthImage.sprite = newState switch
        {
            ELifeState.Full => fullHearth,
            ELifeState.Half => midHearth,
            _ => emptyHearth
        };
    }

    public void OnValidate()
    {
        _hearthImage = GetComponent<Image>();
    }
}

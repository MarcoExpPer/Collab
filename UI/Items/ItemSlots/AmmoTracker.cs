using System;
using Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

[RequireComponent(typeof(TextMeshProUGUI))]
public class AmmoTracker : MonoBehaviour
{
    public Color fullAmmoColor;
    public Color noAmmoColor;
    public Color normalColor;
    
    [SerializeField] private TextMeshProUGUI _text;
    private ActiveItem _activeItem;
    
    public void Awake()
    {
        _text.enabled = false;
    }

    public void TrackAmmo(ActiveItem InActiveItem)
    {
        if (InActiveItem.ItemData.MaxAmmo > 0)
        {
            _activeItem = InActiveItem;
            enabled = true;
            _activeItem.ammoUpdatedEvent.AddListener(UpdateAmmoText);
            UpdateAmmoText(_activeItem.ItemData.AmmoCount, _activeItem.ItemData.MaxAmmo);
            _text.enabled = true;
        }
        else
        {
            enabled = false;
        }
    }

    public void OnEnable()
    {
        if (_activeItem)
        {
            _text.enabled = true;
            UpdateAmmoText(_activeItem.ItemData.AmmoCount, _activeItem.ItemData.MaxAmmo);
        }
    }

    public void OnDisable()
    {
        _text.enabled = false;
    }

    public void UpdateAmmoText(int currentAmount, int maxAmount)
    {
        _text.text = currentAmount + "/" + maxAmount;
        if (currentAmount == maxAmount)
        {
            _text.color = fullAmmoColor;
        }else if (currentAmount == 0)
        {
            _text.color = noAmmoColor;
        }
        else
        {
            _text.color = normalColor;
        }
    }
    private void OnValidate()
    {   
        _text = GetComponent<TextMeshProUGUI>();
    }
    
}

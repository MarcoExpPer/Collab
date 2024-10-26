
using System;
using UnityEngine;

public class ActiveBombs : ActiveItem
{
    private bool readyToThrow;
    private BombItemData _bombData;
    
    private void Start()
    {
        readyToThrow = true;
    }

    public override void OnObtained( ActiveItemData itemData )
    {
        base.OnObtained( itemData );
        _bombData = (BombItemData) itemData;
    }

    public override bool OnInput(bool inputValue)
    {
        if (!base.OnInput(inputValue)) return false;

        if (inputValue && readyToThrow)
        {
            SpawnBomb();
        }
        
        return true;
    }
    
    void SpawnBomb()
    {
        readyToThrow = false;
        ChangeAmmo(-1);
        
        Vector3 posicionSpawn = transform.position + transform.forward * _bombData.distanciaSpawn;

        Bomb_Explosion projectile = Instantiate(_bombData.bombaPrefab, posicionSpawn, Quaternion.identity);
        projectile.hitEffectsSo = _bombData.hitEffectsSo;
        
        Invoke(nameof(ResetThrow), _bombData.throwCooldown);
    }

    private void ResetThrow()
    {
        readyToThrow = true;
    }
}

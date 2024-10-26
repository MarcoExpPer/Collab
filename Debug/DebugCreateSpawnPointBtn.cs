
using System;
using TMPro;
using UnityEngine;

public class DebugCreateSpawnPointBtn : DebugButton
{
    public override void OnClick()
    {
        Transform playerTransform = GameManager.Instance.itemController.transform;
        GameManager.Instance.spawnController.UpdateSpawnLocation(playerTransform.position, playerTransform.rotation);
    }
}

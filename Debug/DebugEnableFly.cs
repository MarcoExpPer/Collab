
using System;
using TMPro;
using UnityEngine;

public class DebugEnableFly : DebugButton
{
    public override void OnClick()
    {
        PlayerSMController ps = GameManager.Instance.playerSmController;
        ps.PlDebugState.PlDebugStateMachine.ChangeState(ps.PlDebugState.PlDebugStateMachine.DebugFlyState);
    }
}

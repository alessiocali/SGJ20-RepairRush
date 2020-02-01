using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "New PowerUp", menuName = "Shield")]
public class Shield : PassivePowerUp
{
    protected override void ActivateInternal()
    {
        m_PickerPlayerComponent.IsInvincible = true;
    }

    protected override void DeactivateInternal()
    {
        m_PickerPlayerComponent.IsInvincible = false;
    }

    protected override PowerUp CreateInternal()
    {
        Shield copy = CreateInstance<Shield>();
        copy.m_Duration = m_Duration;
        return copy;
    }
}
using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "New PowerUp", menuName = "SuperSpeed")]
public class SuperSpeed : PassivePowerUp
{
    public float m_SpeedMultiplier = 1.15f;

    protected override void ActivateInternal()
    {
        SetSpeedMultiplierOnMovementComponent(m_SpeedMultiplier);
    }

    protected override void DeactivateInternal()
    {
        SetSpeedMultiplierOnMovementComponent(1f);
    }

    private void SetSpeedMultiplierOnMovementComponent(float value)
    {
        MovementComponent movementCmp = m_Picker.GetComponent<MovementComponent>();
        if (movementCmp != null)
        {
            movementCmp.SpeedMultiplier = value;
        }
    }

    protected override PowerUp CreateInternal()
    {
        SuperSpeed copy = CreateInstance<SuperSpeed>();
        copy.m_Duration = m_Duration;
        copy.m_SpeedMultiplier = m_SpeedMultiplier;
        return copy;
    }
}
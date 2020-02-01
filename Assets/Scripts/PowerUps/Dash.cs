using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "New PowerUp", menuName = "Dash")]
public class Dash : PowerUp
{
    public float m_DashImpulseForce = 10f;
    public float m_DashCooldown = 2f;
    public float m_LockedDirectionDuration = 0.7f;

    private bool m_IsActionLocked = false;

    public override bool ShouldBeDestroyed() { return false; } // rbaldi: it is the base PowerUp
    public override bool CanBeActivated() { return !m_IsActionLocked; }
    public override bool IsPassivePowerUp() { return false; }

    public override void PickedUp(GameObject picker)
    {
        base.PickedUp(picker);
        m_IsActionLocked = false;
    }

    public override void Activate()
    {
        Rigidbody rigidbody = m_Picker.GetComponent<Rigidbody>();
        if (rigidbody != null)
        {
            Vector3 direction = rigidbody.velocity.normalized;
            rigidbody.AddForce(m_DashImpulseForce * direction, ForceMode.Impulse);
        }

        m_PickerPlayerComponent.IsDashing = true;

        m_PickerPlayerComponent.StartCoroutineWithAction(m_LockedDirectionDuration, () =>
        {
            m_PickerPlayerComponent.IsDashing = false;
        });

        m_IsActionLocked = true;
        m_PickerPlayerComponent.StartCoroutineWithAction(m_DashCooldown, () =>
        {
            m_IsActionLocked = false;
        });

        SoundManager.Instance.PlayDashSound();
    }

    protected override PowerUp CreateInternal()
    {
        Dash copy = CreateInstance<Dash>();
        copy.m_DashImpulseForce = m_DashImpulseForce;
        copy.m_DashCooldown = m_DashCooldown;
        copy.m_LockedDirectionDuration = m_LockedDirectionDuration;
        return copy;
    }
}
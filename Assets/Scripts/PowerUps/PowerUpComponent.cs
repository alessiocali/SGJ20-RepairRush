using System;
using UnityEngine;

public class PowerUpComponent : MonoBehaviour
{
    [SerializeField]
    public PowerUp m_PowerUp;
    public PowerUp GetCreatedPowerUp() { return m_PowerUp.Create(); }

    private void OnTriggerEnter(Collider other)
    {
        PlayerComponent playerComponent = other.GetComponent<PlayerComponent>();
        if (playerComponent != null)
        {
            playerComponent.OnPickedPowerUp(this);
            Debug.Log(String.Format("Player {0} got {1}", playerComponent.PlayerNumber, m_PowerUp.name));
            GameObject.Destroy(gameObject.transform.parent.gameObject); // Hackety hack
        }
    }
}

[Serializable]
public abstract class PowerUp : ScriptableObject
{
    public Sprite m_PowerUpImage;

    protected GameObject m_Picker;
    protected PlayerComponent m_PickerPlayerComponent;
    
    public virtual bool ShouldBeDestroyed() { return false; }
    public virtual bool CanBeActivated() { return true; }

    public virtual bool IsPassivePowerUp() { return true; }
    public bool IsActivePowerUp() { return !IsPassivePowerUp(); }

    public virtual void PickedUp(GameObject picker)
    {
        m_Picker = picker;
        Debug.Assert(m_Picker != null);
        m_PickerPlayerComponent = m_Picker.GetComponent<PlayerComponent>();
        Debug.Assert(m_PickerPlayerComponent != null);
    }

    public abstract void Activate();
    public virtual void Deactivate() { }

    public PowerUp Create()
    {
        PowerUp copy = CreateInternal();
        copy.m_PowerUpImage = m_PowerUpImage;
        return copy;
    }

    protected abstract PowerUp CreateInternal();
}

[Serializable]
public abstract class PassivePowerUp : PowerUp
{
    public float m_Duration;
    bool m_IsExpired = false;
    bool m_Deactivated = false;

    public override bool ShouldBeDestroyed() { return m_IsExpired; }
    public override bool CanBeActivated() { return false; }

    public override void PickedUp(GameObject picker)
    {
        base.PickedUp(picker);
        Activate();

        m_IsExpired = false;
        m_PickerPlayerComponent.StartCoroutineWithAction(m_Duration, () =>
        {
            Deactivate();
            m_IsExpired = true;
        });
    }

    public override void Activate()
    {
        ActivateInternal();
    }

    public override void Deactivate()
    {
        if (!m_Deactivated)
        {
            DeactivateInternal();
            m_Deactivated = true;
        }
    }

    protected abstract void ActivateInternal();
    protected abstract void DeactivateInternal();
}
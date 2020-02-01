using System;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerStage
{
    [SerializeField]
    public uint m_PartsNeeded;

    public PlayerStage(uint partsNeeded)
    {
        m_PartsNeeded = partsNeeded;
    }
}

public class PlayerComponent : MovementComponent
{
    enum PlayerState
    {
        Regular,
        Dashing,
        Staggering,
        Invincible
    }

    [SerializeField]
    private List<PlayerStage> m_Stages = new List<PlayerStage>();

    public String PlayerName { get; set; } = "Player";

    public PlayerStage m_CurrentStage { get { return m_Stages[m_CurrentStageIdx]; } }
    public int m_CurrentStageIdx;
    public uint m_PartsCollected;

    public Dash m_BasePowerUpData;

    private PowerUp m_BasePowerUp;
    private PowerUp m_PickedPowerUp;
    private PowerUp m_ActivePowerUp;

    public bool FixStationIsNearby { private get; set; } = false;

    private PlayerState m_PlayerState;
    
    private void SetPlayerState(PlayerState playerState) { m_PlayerState = playerState; }

    public bool IsDashing
    {
        get { return m_PlayerState == PlayerState.Dashing; }
        set 
        {
            SetPlayerState(value ? PlayerState.Dashing : PlayerState.Regular);
            IsDirectionLocked = value;
        }
    }

    public bool IsStaggering
    {
        get { return m_PlayerState == PlayerState.Staggering; }
    }

    public bool IsInvincible
    {
        get { return m_PlayerState == PlayerState.Invincible; }
        set { SetPlayerState(value ? PlayerState.Invincible: PlayerState.Regular); }
    }

    protected override void Start()
    {
        base.Start();
        m_CurrentStageIdx = 0;
        m_PartsCollected = 0;

        if (m_BasePowerUpData != null)
        {
            m_BasePowerUp = m_BasePowerUpData.Create();
            m_BasePowerUp.PickedUp(/*this.*/gameObject);
            m_ActivePowerUp = m_BasePowerUp;
        }

        m_PlayerState = PlayerState.Regular;
    }

    protected override void Update()
    {
        base.Update();

        if (GameHelpers.GetInputManager().IsActionButtonPressed(PlayerNumber))
        {
            if (m_ActivePowerUp != null && m_ActivePowerUp.IsPassivePowerUp())
            {
                m_ActivePowerUp.Deactivate();
                m_ActivePowerUp = m_BasePowerUp;
            }

            if (m_ActivePowerUp != null && m_ActivePowerUp.IsActivePowerUp() && m_ActivePowerUp.CanBeActivated())
            {
                m_ActivePowerUp.Activate();
                if (m_ActivePowerUp.ShouldBeDestroyed())
                {
                    m_ActivePowerUp = m_BasePowerUp;
                }
            }
        }
        else if (GameHelpers.GetInputManager().IsInteractButtonPressed(PlayerNumber))
        {
            if (FixStationIsNearby && m_PartsCollected == m_CurrentStage.m_PartsNeeded)
            {
                OnStageCompleted();
            }
        }
    }

    private void OnValidate()
    {
        if (m_Stages.Count == 0)
        {
            Debug.LogWarning("Empty player stages, you must provide at least one.");
            m_Stages.Add(new PlayerStage(0));
        }

        m_Stages.Sort((s1, s2) => { return checked((int)s1.m_PartsNeeded) - checked((int)s2.m_PartsNeeded); });
    }

    public bool OnPartsCollected(uint partsCollectedCount = 1)
    {
        if (IsStaggering)
        {
            return false;
        }

        if (m_PartsCollected < m_CurrentStage.m_PartsNeeded)
        {
            m_PartsCollected = Math.Min(m_PartsCollected + partsCollectedCount, m_CurrentStage.m_PartsNeeded);
            Debug.Log(String.Format("{0} Collected {1} parts", PlayerName, m_PartsCollected));
            SoundManager.Instance.PlayPickPartSound();
            return true;
        }

        return false;
    }

    public void OnPickedPowerUp(PowerUpComponent powerUpCmp)
    {
        if (m_ActivePowerUp != null)
        {
            m_ActivePowerUp.Deactivate();
        }

        m_PickedPowerUp = powerUpCmp.GetCreatedPowerUp();
        m_PickedPowerUp.PickedUp(/*this.*/gameObject);
        m_ActivePowerUp = m_PickedPowerUp;

        SoundManager.Instance.PlayPickPowerUpSound();
    }

    public void StartCoroutineWithAction(float timer, Action action)
    {
        StartCoroutine(CoroutineWithAction(timer, action));
    }

    private IEnumerator CoroutineWithAction(float timer, Action action)
    {
        yield return new WaitForSeconds(timer);
        action();
    }

    public void HardStagger(Vector3 impulse)
    {
        if (!IsInvincible)
        {
            while (DropPart()) ;
            DecreaseStage();
            Stagger(impulse);
        }
    }

    public void SoftStagger(Vector3 impulse)
    {
        if (!IsInvincible)
        {
            DropPart();
            Stagger(impulse);
        }
    }

    private void Stagger(Vector3 impulse)
    {
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.AddForce(impulse, ForceMode.Impulse);
        SetPlayerState(PlayerState.Staggering);

        StartCoroutineWithAction(
            GameHelpers.GetGameManager().StaggerTime, 
            () => SetPlayerState(PlayerState.Regular)
        );
    }

    private bool DropPart()
    {
        if (m_PartsCollected == 0)
        {
            return false;
        }

        m_PartsCollected--;
        GameManagerComponent gameManager = GameHelpers.GetGameManager();
        SpawnManagerComponent spawnManager = GameHelpers.GetSpawnManager();
        GameObject partDropped = spawnManager.RequestSpawnRobotPart(
            gameManager.GetRandomRobotPartPrefab(), 
            transform.position + Vector3.up * 2.5f, Quaternion.identity,
            true
        );

        if (partDropped)
        {
            partDropped.GetComponentInChildren<RobotPartComponent>().SetUnpickableFor(gameManager.DroppedPartUnpickableTime);
            SoundManager.Instance.PlayDropPartSound();
            return true;
        }

        return false;
    }

    private void OnStageCompleted()
    {
        Debug.Assert(m_PartsCollected == m_CurrentStage.m_PartsNeeded);
        Debug.Log(String.Format("{0} Completed Stage {1}", PlayerName, m_CurrentStageIdx));
        if (m_CurrentStageIdx == m_Stages.Count - 1)
        {
            OnGameWon();
        }
        else
        {
            m_CurrentStageIdx++;
            m_PartsCollected = 0;
            SoundManager.Instance.PlayStageUpSound();
        }
    }

    private void DecreaseStage()
    {
        m_CurrentStageIdx -= m_CurrentStageIdx > 0 ? 1 : 0;
        Debug.Log(String.Format("{0} Decrease to Stage {1}", PlayerName, m_CurrentStageIdx));
    }

    private void OnGameWon()
    {
        Debug.Log(String.Format("{0} Won!", PlayerName));
        SoundManager.Instance.PlayGameOverSound();
        GameHelpers.GetGameManager().OnGameWon(this);
    }

    public Sprite GetPowerUpImage()
    {
        return m_ActivePowerUp ? m_ActivePowerUp.m_PowerUpImage : null;
    }
}

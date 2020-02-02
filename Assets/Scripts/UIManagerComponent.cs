using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManagerComponent : MonoBehaviour
{
    public List<GameObject> m_PlayerHUDs = new List<GameObject>();
    public List<GameObject> m_PlayerPowerUpImageSlots = new List<GameObject>();
    public List<GameObject> m_PlayerRepairHUDs = new List<GameObject>();

    public GameObject m_GameHUD;
    public GameObject m_VictoryHUD;
    public GameObject m_VictoryText;

    private List<PlayerComponent> m_Players = new List<PlayerComponent>();

    private void Start()
    {
        m_GameHUD.SetActive(true);

        if (m_VictoryHUD != null)
        {
            m_VictoryHUD.SetActive(false);
        }
    }

    private void Update()
    {
        foreach (PlayerComponent playerComponent in GameHelpers.GetPlayers())
        {
            SetPlayerState(playerComponent);
            GetPowerUpImageSlotForPlayer(playerComponent).sprite = playerComponent.GetPowerUpImage();

            m_PlayerRepairHUDs[playerComponent.PlayerNumber].SetActive(playerComponent.FixStationIsNearby
                && playerComponent.m_PartsCollected >= playerComponent.m_CurrentStage.m_PartsNeeded);
        }
    }

    public void SetPlayerState(PlayerComponent playerComponent)
    {
        Text playerHUD = m_PlayerHUDs[playerComponent.PlayerNumber].GetComponent<Text>();
        playerHUD.text = String.Format("{0} : Stage {1} | {2}/{3}",
            playerComponent.PlayerName,
            playerComponent.m_CurrentStageIdx,
            playerComponent.m_PartsCollected,
            playerComponent.m_CurrentStage.m_PartsNeeded
        );
    }

    public Image GetPowerUpImageSlotForPlayer(PlayerComponent playerComponent)
    {
        return m_PlayerPowerUpImageSlots[playerComponent.PlayerNumber].GetComponent<Image>();
    }

    public void OnGameWon(PlayerComponent winner)
    {
        m_GameHUD.SetActive(false);

        if (m_VictoryHUD != null && m_VictoryText != null)
        {
            m_VictoryHUD.SetActive(true);
            m_VictoryText.GetComponent<Text>().text = String.Format("{0} Won!\nPress Start to play again.", winner.PlayerName);
        }
    }
}

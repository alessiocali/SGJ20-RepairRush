using System;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnerComponent : MonitoringSpawnerComponent
{
    public int m_PlayerNumberToSet = 0;

    protected override void OnInstanceSpawned(GameObject spawnedInstance)
    {
        base.OnInstanceSpawned(spawnedInstance);
        PlayerComponent playerComponent = spawnedInstance.GetComponent<PlayerComponent>();
        if (playerComponent)
        {
            playerComponent.PlayerNumber = m_PlayerNumberToSet;
            playerComponent.PlayerName = String.Format("Player {0}", m_PlayerNumberToSet + 1);
        }
    }
}

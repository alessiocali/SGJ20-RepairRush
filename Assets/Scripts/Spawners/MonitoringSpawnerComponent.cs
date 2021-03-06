﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonitoringSpawnerComponent : AbstractSpawnerComponent
{
    [Min(0)]
    public int m_MaxEntitiesToSpawn;

    public bool m_ShouldTestFallOnTerrain = false;

    private readonly List<GameObject> m_SpawnedEntities = new List<GameObject>();

    protected override void Update()
    {
        CleanupDeletedInstances();
        base.Update();
    }

    void CleanupDeletedInstances()
    {
        m_SpawnedEntities.RemoveAll(instance => instance == null);
    }

    protected override GameObject SpawnRandomEntityFromList()
    {
        if (m_SpawnedEntities.Count >= m_MaxEntitiesToSpawn)
        {
            return null;
        }

        GameObject prefab = GetRandomEntityFromList();
        Vector3 spawnPoint = GetRandomSpawnPoint();

        if (m_ShouldTestFallOnTerrain && !SpawnManagerComponent.WouldFallOnTerrain(prefab, spawnPoint))
        {
            return null;
        }

        var instance = GameObject.Instantiate(prefab, spawnPoint, Quaternion.identity);
        m_SpawnedEntities.Add(instance);
        return instance;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartSpawnerComponent : AbstractSpawnerComponent
{
    [Min(0)]
    public int m_MaxTries = 5;

    protected override GameObject SpawnRandomEntityFromList()
    {
        int timesTried = 0;
        GameObject spawnedEntity = null;

        while (spawnedEntity == null && timesTried++ < m_MaxTries)
        {
            spawnedEntity = GameHelpers.GetSpawnManager().RequestSpawnRobotPart(
                GetRandomEntityFromList(), GetRandomSpawnPoint(), Quaternion.identity
            );
        }

        return spawnedEntity;
    }
}

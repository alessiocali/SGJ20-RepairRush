using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartSpawnerComponent : AbstractSpawnerComponent
{
    protected override GameObject SpawnRandomEntityFromList()
    {
        return GameHelpers.GetSpawnManager()
            .RequestSpawnRobotPart(GetRandomEntityFromList(), GetRandomSpawnPoint(), Quaternion.identity);
    }
}

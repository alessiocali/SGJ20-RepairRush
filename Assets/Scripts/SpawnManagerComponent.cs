using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;
using System;

public class SpawnManagerComponent : MonoBehaviour
{
    private List<GameObject> m_SpawnedRobotParts = new List<GameObject>();
    private List<GameObject> m_SpawnedProjectiles = new List<GameObject>();
    private List<GameObject> m_SpawnedPowerUps = new List<GameObject>();

    private void Update()
    {
        CleanupDeletedInstances();
    }

    private void CleanupDeletedInstances()
    {
        System.Predicate<GameObject> IsGameObjectDeleted = (GameObject gameObject) => { return gameObject == null; };
        m_SpawnedRobotParts.RemoveAll(IsGameObjectDeleted);
        m_SpawnedProjectiles.RemoveAll(IsGameObjectDeleted);
        m_SpawnedPowerUps.RemoveAll(IsGameObjectDeleted);
    }

    public GameObject RequestSpawnRobotPart(GameObject template, Vector3 position, Quaternion rotation, bool forceSpawn = false)
    {
        Debug.Assert(template.GetComponentInChildren<RobotPartComponent>());

        if (!forceSpawn && m_SpawnedRobotParts.Count >= GetMaxSpawnableRobotParts())
        {
            return null;
        }

        if (!forceSpawn && !WouldFallOnTerrain(template, position))
        {
            return null;
        }

        GameObject instance = GameObject.Instantiate(template, position, rotation);
        m_SpawnedRobotParts.Add(instance);
        return instance;
    }

    private uint GetMaxSpawnableRobotParts()
    {
        var query =
            from player in GameHelpers.GetPlayers()
            orderby player.m_CurrentStage.m_PartsNeeded descending
            select player.m_CurrentStage.m_PartsNeeded;

        return query.FirstOrDefault();
    }

    public static bool WouldFallOnTerrain(GameObject gameObject, Vector3 position)
    {
        const float MaxRayCastDistance = 40f;
        const string TerrainTag = "Terrain";

        Physics.Raycast(position, Vector3.down * MaxRayCastDistance, out RaycastHit hitInfo);
        if (!hitInfo.collider || !hitInfo.collider.CompareTag(TerrainTag))
        {
            return false;
        }

        bool wouldOverlapWithGeometry = false;
        Collider collider = gameObject.GetComponent<Collider>();
        if (collider && !collider.isTrigger)
        {
            Func<Collider[]> colliderFunction = null;
            Vector3 hitPoint = hitInfo.point;

            if (collider is SphereCollider sphereCollider)
            {
                colliderFunction = () => { return Physics.OverlapSphere(hitPoint, sphereCollider.radius); };
            }
            else if (collider is BoxCollider boxCollider)
            {
                colliderFunction = () => { return Physics.OverlapBox(hitPoint + boxCollider.center, boxCollider.size * 0.5f); };
            }

            if (colliderFunction != null)
            {
                var notTerrainCollisionQuery =
                    from collision in colliderFunction()
                    where !collision.CompareTag(TerrainTag)
                    select collision;

                wouldOverlapWithGeometry = notTerrainCollisionQuery.Any();
            }
        }

        if (wouldOverlapWithGeometry)
        {
            return false;
        }

        return true;
    }
}

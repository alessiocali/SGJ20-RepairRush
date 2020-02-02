using System;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractSpawnerComponent : MonoBehaviour
{
    public List<Rect> m_SpawnAreas = new List<Rect>();

    [Min(0)]
    [Tooltip("This spawner will generate a random entities every this number of seconds. If 0 will spawn every frame.")]
    public float m_SpawnTime = 1f;

    [SerializeField]
    private List<GameObject> m_PrefabsToSpawn = new List<GameObject>();

    private bool m_IsSpawnTimerElapsed = false;

    protected virtual void Start()
    {
        StartSpawnTimer();
    }

    protected virtual void Update()
    {
        if (CanSpawnNext())
        {
            GameObject spawnedEntity = SpawnRandomEntityFromList();
            if (spawnedEntity)
            {
                OnInstanceSpawned(spawnedEntity);
            }

            m_IsSpawnTimerElapsed = false;
            StartSpawnTimer();
        }
    }

    private bool CanSpawnNext()
    {
        return !GameHelpers.GetGameManager().IsGameOver
            && m_SpawnAreas.Count > 0
            && m_PrefabsToSpawn.Count > 0
            && m_IsSpawnTimerElapsed;
    }

    private void StartSpawnTimer()
    {
        StartCoroutine(WaitSpawnTime());
    }

    private IEnumerator WaitSpawnTime()
    {
        yield return new WaitForSeconds(m_SpawnTime);
        m_IsSpawnTimerElapsed = true;
    }

    protected GameObject GetRandomEntityFromList()
    {
        int randomPrefabIdx = UnityEngine.Random.Range(0, m_PrefabsToSpawn.Count);
        return m_PrefabsToSpawn[randomPrefabIdx];
    }

    protected Vector3 GetRandomSpawnPoint()
    {
        int randomSpawnAreaIdx = UnityEngine.Random.Range(0, m_SpawnAreas.Count);
        Rect randomArea = m_SpawnAreas[randomSpawnAreaIdx];

        GetCenterAndHalfExtents(randomArea, out Vector3 spawnCenter, out Vector3 rectHalfExtents);
        Vector3 min = spawnCenter - rectHalfExtents;
        Vector3 max = spawnCenter + rectHalfExtents;
        return new Vector3(UnityEngine.Random.Range(min.x, max.x), transform.position.y, UnityEngine.Random.Range(min.z, max.z));
    }

    protected abstract GameObject SpawnRandomEntityFromList();

    protected virtual void OnInstanceSpawned(GameObject spawnedInstance)
    { }

    private void GetCenterAndHalfExtents(Rect spawnArea, out Vector3 center, out Vector3 halfExtents)
    {
        Vector3 rectPosition = new Vector3(spawnArea.x, 0f, spawnArea.y);
        halfExtents = new Vector3(spawnArea.width, 0f, spawnArea.height) * 0.5f;
        center = transform.position + rectPosition;
    }

    void OnDrawGizmos()
    {
        foreach (Rect spawnArea in m_SpawnAreas)
        {
            GetCenterAndHalfExtents(spawnArea, out Vector3 center, out Vector3 halfExtents);
            halfExtents.y = 0.1f;

            Color color = Color.green;
            color.a = 0.5f;

            Gizmos.color = color;
            Gizmos.DrawCube(center, halfExtents * 2);
        }
    }
}

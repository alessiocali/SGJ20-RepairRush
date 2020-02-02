using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerComponent : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> SpawnableParts = new List<GameObject>();
    public float StaggerTime = 2.5f;
    public float DroppedPartUnpickableTime = 3f;

    public bool IsGameOver { get; private set; }

    public GameObject GetRandomRobotPartPrefab()
    {
        int randomIdx = UnityEngine.Random.Range(0, SpawnableParts.Count);
        return SpawnableParts[randomIdx];
    }

    public void OnGameWon(PlayerComponent winner)
    {
        IsGameOver = true;
        GameHelpers.GetInputManager().OnGameWon();
        GameHelpers.GetUIManager().OnGameWon(winner);
    }

    public void OnGameRestarted()
    {
        IsGameOver = false;
    }
}

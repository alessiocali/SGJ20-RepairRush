using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHelpers
{
    public static GameManagerComponent GetGameManager()
    {
        return Object.FindObjectOfType<GameManagerComponent>();
    }

    public static UIManagerComponent GetUIManager()
    {
        return Object.FindObjectOfType<UIManagerComponent>();
    }

    public static InputManagerComponent GetInputManager()
    {
        return Object.FindObjectOfType<InputManagerComponent>();
    }

    public static SpawnManagerComponent GetSpawnManager()
    {
        return Object.FindObjectOfType<SpawnManagerComponent>();
    }

    public static PlayerComponent[] GetPlayers()
    {
        return Object.FindObjectsOfType<PlayerComponent>();
    }
}

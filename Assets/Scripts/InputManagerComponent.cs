using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputManagerComponent : MonoBehaviour
{
    public bool m_IsActive = true;

    private void Update()
    {
        if (GameHelpers.GetGameManager().IsGameOver && Input.GetButtonDown("Start"))
        {
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name, LoadSceneMode.Single);
        }
    }

    public Vector3 GetDesiredDirection(int playerNumber = 0)
    {
        if (!m_IsActive) { return Vector3.zero; }

        Vector3 inputDirection = Vector3.zero;
        switch (playerNumber)
        {
            case 0:
                inputDirection = new Vector3(Input.GetAxis("Horizontal1"), 0f, Input.GetAxis("Vertical1"));
                break;
            case 1:
                inputDirection = new Vector3(Input.GetAxis("Horizontal2"), 0f, Input.GetAxis("Vertical2"));
                break;
        }
        return inputDirection.normalized;
    }

    public bool IsActionButtonPressed(int playerNumber = 0)
    {
        if (!m_IsActive) { return false; }

        switch (playerNumber)
        {
            case 0:
                return Input.GetButtonDown("Fire1");
            case 1:
                return Input.GetButtonDown("Fire2");
        }
        return false;
    }

    public bool IsInteractButtonPressed(int playerNumber = 0)
    {
        if (!m_IsActive) { return false; }

        switch (playerNumber)
        {
            case 0:
                return Input.GetButtonDown("Interact1");
            case 1:
                return Input.GetButtonDown("Interact2");
        }
        return false;
    }

    public void OnGameWon()
    {
        m_IsActive = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixStationTriggerComponent : MonoBehaviour
{
    public GameObject m_InteractButtonPrompt;

    private List<PlayerComponent> m_PlayersInside = new List<PlayerComponent>();

    private void Update()
    {
        if (m_InteractButtonPrompt)
        {
            Animator buttonAnimator = m_InteractButtonPrompt.GetComponent<Animator>();
            if (buttonAnimator)
            {
                bool anyReadyPlayerInside = m_PlayersInside.Find(player => { return player.CanCompleteStage(); });
                buttonAnimator.SetTrigger(anyReadyPlayerInside ? "Activate" : "Deactivate");
                buttonAnimator.ResetTrigger(anyReadyPlayerInside ? "Deactivate" : "Activate");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        SetFixStationNearby(other, true);
    }

    private void OnTriggerExit(Collider other)
    {
        SetFixStationNearby(other, false);
    }

    private void SetFixStationNearby(Collider target, bool fixStationNearby)
    {
        PlayerComponent playerComponent = target.GetComponent<PlayerComponent>();
        if (playerComponent)
        {
            if (fixStationNearby)
            {
                m_PlayersInside.Add(playerComponent);
            }
            else
            {
                m_PlayersInside.Remove(playerComponent);
            }

            playerComponent.FixStationIsNearby = fixStationNearby;
        }
    }
}

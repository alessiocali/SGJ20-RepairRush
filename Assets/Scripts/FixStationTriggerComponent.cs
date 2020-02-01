using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixStationTriggerComponent : MonoBehaviour
{
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
            playerComponent.FixStationIsNearby = fixStationNearby;
        }
    }
}

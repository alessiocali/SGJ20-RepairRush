using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : Projectile
{
    private void Start()
    {
        SoundManager.Instance.PlayRocketSound();
    }

    protected override void Hit(PlayerComponent playerCmp, Vector3 direction)
    {
        playerCmp.HardStagger(direction * m_StaggerIntensity);
    }
}

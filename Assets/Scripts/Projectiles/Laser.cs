using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    public float m_StaggerIntensity = 5f;

    protected abstract void Hit(PlayerComponent playerCmp, Vector3 direction);
    
    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rbCmp = GetComponent<Rigidbody>();
        PlayerComponent playerCmp = other.GetComponent<PlayerComponent>();
        if (playerCmp != null && rbCmp != null)
        {
            Hit(playerCmp, rbCmp.velocity.normalized);
        }

        if (other.GetComponentInChildren<PowerUpComponent>() == null
            && other.GetComponentInChildren<StaggerComponent>() == null
            && other.GetComponentInChildren<RobotPartComponent>() == null
            && other.GetComponentInChildren<FixStationTriggerComponent>() == null)
        {
            GameObject.Destroy(gameObject);
        }
    }
}

public class Laser : Projectile
{
    private void Start()
    {
        SoundManager.Instance.PlayLaserSound();
    }

    protected override void Hit(PlayerComponent playerCmp, Vector3 direction)
    {
        playerCmp.SoftStagger(direction * m_StaggerIntensity);
    }
}

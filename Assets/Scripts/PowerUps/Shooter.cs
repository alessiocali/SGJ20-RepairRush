using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "New PowerUp", menuName = "Shooter")]
public class Shooter : PowerUp
{
    public GameObject m_Projectile;
    public float m_ShootSpeed = 15f;

    public uint m_AvailableNumberOfShoots = 1;
    private uint m_ShotedShoot = 0;

    public override bool ShouldBeDestroyed() { return m_ShotedShoot >= m_AvailableNumberOfShoots; }
    public override bool CanBeActivated() { return !ShouldBeDestroyed(); }
    public override bool IsPassivePowerUp() { return false; }

    public override void Activate()
    {
        Rigidbody rbCmp = m_Picker.GetComponent<Rigidbody>();
        Vector3 direction = rbCmp != null ? rbCmp.velocity.normalized : m_Picker.transform.forward;
        Vector3 spawnPosition = m_Picker.transform.position + direction * 2;
        
        Quaternion spawnRotaton = Quaternion.Euler(Vector3.Cross(Vector3.up, direction) * 90f) * Quaternion.FromToRotation(Vector3.forward, direction);
        GameObject go = GameObject.Instantiate(m_Projectile, spawnPosition + Vector3.up * 2, spawnRotaton);
        Rigidbody rb = go != null ? go.GetComponent<Rigidbody>() : null;
        if (rb != null)
        {
            rb.velocity = direction * m_ShootSpeed;
        }

        m_ShotedShoot++;
    }

    protected override PowerUp CreateInternal()
    {
        Shooter copy = CreateInstance<Shooter>();
        copy.m_ShootSpeed = m_ShootSpeed;
        copy.m_Projectile = m_Projectile;
        copy.m_AvailableNumberOfShoots = m_AvailableNumberOfShoots;
        return copy;
    }
}
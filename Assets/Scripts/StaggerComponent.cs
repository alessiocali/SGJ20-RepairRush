using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaggerComponent : MonoBehaviour
{
    public GameObject m_PlayerParent;
    public float m_StaggerImpulse = 10f;

    private PlayerComponent m_ParentPlayerComponent;

    private void Start()
    {
        m_ParentPlayerComponent = m_PlayerParent.GetComponent<PlayerComponent>();
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerComponent playerComponent = other.GetComponent<PlayerComponent>();
        if (playerComponent && playerComponent.IsDashing)
        {
            Vector3 direction = m_PlayerParent.transform.position - other.transform.position;
            direction.y = 0;
            m_ParentPlayerComponent.SoftStagger(direction.normalized * m_StaggerImpulse);
        }
    }
}

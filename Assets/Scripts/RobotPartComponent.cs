using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotPartComponent : MonoBehaviour
{
    public GameObject m_ParentToDelete;
    
    private bool m_IsPickable = true;

    private void OnTriggerEnter(Collider other)
    {
        if (!m_IsPickable)
        {
            return;
        }

        PlayerComponent playerComponent = other.GetComponent<PlayerComponent>();
        if (playerComponent && playerComponent.OnPartsCollected())
        {
            Destroy(m_ParentToDelete ? m_ParentToDelete : gameObject);
        }
    }

    public void SetUnpickableFor(float unpickableSeconds)
    {
        m_IsPickable = false;
        StartCoroutine(SetPickableAfter(unpickableSeconds));
    }

    private IEnumerator SetPickableAfter(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        m_IsPickable = true;
    }
}

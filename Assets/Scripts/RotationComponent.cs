using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationComponent : MonoBehaviour
{
    public float m_RotationSpeed = 15f;
    public Vector3 m_RotationAxis = Vector3.up;
    
    void Update()
    {
        Vector3 rot = transform.eulerAngles;
        rot += m_RotationSpeed * m_RotationAxis * Time.deltaTime;
        transform.eulerAngles = rot;
    }
}

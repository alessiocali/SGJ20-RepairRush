using System.Timers;
using System;
using UnityEngine;

public class MovementComponent : MonoBehaviour
{
    public int PlayerNumber { get; set; } = 0;

    public float m_BaseSpeed = 7f;
    public float SpeedMultiplier { get; set; } = 1f;
    public float m_SpeedGainForSecond = 2f;

    public float m_MaxRotationSpeed = 10f;

    public bool IsDirectionLocked { get; set; } = false;

    private Rigidbody m_Rigidbody;
    private Vector3 m_DesiredDirection = Vector3.zero;


    protected virtual void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    protected virtual void Update()
    {
        m_DesiredDirection = IsDirectionLocked ? m_DesiredDirection : GameHelpers.GetInputManager().GetDesiredDirection(PlayerNumber);
        m_Rigidbody.velocity = Vector3.Lerp(m_Rigidbody.velocity, m_BaseSpeed * SpeedMultiplier * m_DesiredDirection, m_SpeedGainForSecond * Time.deltaTime);

        Transform transform = gameObject.transform;
        float desiredRotationAngle = Vector3.Angle(-transform.forward, m_DesiredDirection);

        Vector3 axis = Vector3.Cross(-transform.forward, m_DesiredDirection);
        m_Rigidbody.angularVelocity = (axis.sqrMagnitude > 0 ? axis : transform.up)
            * Mathf.Clamp(desiredRotationAngle - 0.3f, 0f, m_MaxRotationSpeed);

        UpdateAnimatorSpeed(m_Rigidbody.velocity.magnitude);
    }

    private void UpdateAnimatorSpeed(float speed)
    {
        Animator animator = gameObject.GetComponent<Animator>();
        if (animator)
        {
            animator.SetFloat("Speed", speed);
        }
    }
}

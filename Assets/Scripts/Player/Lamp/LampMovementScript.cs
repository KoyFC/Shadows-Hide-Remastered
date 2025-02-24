using System;
using UnityEngine;

public class LampMovementScript : MonoBehaviour
{
    #region Variables
    private bool m_GoingRight = true;

    [Header("References")]
    private LampController m_LampController = null;
    private Transform m_Hinge = null;

    [Header("Lamp Settings")]
    [SerializeField] private float m_RotationSpeed = 10.0f;
    [HideInInspector] public Vector3 m_LastDirection = Vector3.zero;
    private Vector3 m_OriginalScale = Vector3.one;
    #endregion

    #region Main Methods
    private void OnEnable()
    {
        if (m_LampController != null && m_LampController.m_LampInputScript != null)
        {
            m_LampController.m_LampInputScript.m_AimInput = m_LastDirection;
        }
    }

    void Start()
    {
        m_LampController = GetComponent<LampController>();
        m_Hinge = transform.parent;

        m_OriginalScale = transform.localScale;
    }

    void Update()
    {
        HandleLampAim();
    }
    #endregion

    #region Handling Methods
    private void HandleLampAim()
    {
        if (!m_LampController.m_PlayerController.m_LampActive) return;

        m_GoingRight = m_LampController.m_PlayerController.m_PlayerMovementScript.GoingRight;

        Vector3 direction = m_LastDirection;

        if (m_LampController.m_LampInputScript.IsGamepad)
        {
            RotateWithGamepad(ref direction);
        }
        else
        {
            RotateWithMouse(ref direction);
        }

        UpdateLampRotation(direction);
    }
    #endregion

    #region Helper Methods
    private void UpdateLampRotation(Vector3 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Adjust the angle while facing left
        // Doing these operations in reverse order may cause issues
        if (!m_GoingRight)
        {
            angle *= -1;
            angle += 180;
        }

        // Rotate the hinge
        float target = Mathf.MoveTowardsAngle(m_Hinge.localEulerAngles.z, angle, m_RotationSpeed * Time.deltaTime);

        // Normalize the angle to be within -180 to 180 degrees
        target = NormalizeAngle(target);

        Vector3 targetAngle = new Vector3(0, 0, target);
        m_Hinge.localRotation = Quaternion.Euler(targetAngle);

        CheckLanternFlip(target);
    }

    private float NormalizeAngle(float angle)
    {
        while (angle > 180) angle -= 360;
        while (angle < -180) angle += 360;
        return angle;
    }

    private void CheckLanternFlip(float target)
    {
        bool shouldFlipScale = Mathf.Abs(target) > 90;

        transform.localScale = new Vector3(
            m_OriginalScale.x,
            shouldFlipScale ? -m_OriginalScale.y : m_OriginalScale.y,
            m_OriginalScale.z
        );
    }

    private void RotateWithGamepad(ref Vector3 direction)
    {
        Vector2 aimInput = m_LampController.m_LampInputScript.m_AimInput;

        if (aimInput.sqrMagnitude > 0f)
        {
            direction = new Vector3(aimInput.x, aimInput.y, 0).normalized;
            m_LastDirection = direction;
        }
        else
        {
            direction = m_LastDirection;
        }
    }

    private void RotateWithMouse(ref Vector3 direction)
    {
        Vector3 aimPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        aimPosition.z = transform.position.z;
        direction = aimPosition - transform.position;
        m_LastDirection = direction;
    }
    #endregion
}

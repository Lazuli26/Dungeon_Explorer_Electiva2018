using UnityEngine;
using System.Collections.Generic;

public class CharacterMovement : MonoBehaviour
{
    public FixedJoystick leftJoystick;
    public FixedButton button;
    public FixedTouchField touchField;

    private enum ControlMode
    {
        Tank,
        Direct
    }

    [SerializeField] private float m_moveSpeed = 2;
    [SerializeField] private float m_turnSpeed = 200;
    [SerializeField] private float m_jumpForce = 4;
    [SerializeField] private Animator m_animator;
    [SerializeField] private Rigidbody m_rigidBody;
    

    private float m_currentV = 0;
    private float m_currentH = 0;

    private readonly float m_interpolation = 10;
    private readonly float m_walkScale = 0.33f;
    private readonly float m_backwardsWalkScale = 0.16f;
    private readonly float m_backwardRunScale = 0.66f;

    private bool m_wasGrounded;
    private Vector3 m_currentDirection = Vector3.zero;

    private float m_jumpTimeStamp = 0;
    private float m_minJumpInterval = 0.25f;

    private bool m_isGrounded;
    private List<Collider> m_collisions = new List<Collider>();

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint[] contactPoints = collision.contacts;
        for (int i = 0; i < contactPoints.Length; i++)
        {
            if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
            {
                if (!m_collisions.Contains(collision.collider))
                {
                    m_collisions.Add(collision.collider);
                }
                m_isGrounded = true;
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        ContactPoint[] contactPoints = collision.contacts;
        bool validSurfaceNormal = false;
        for (int i = 0; i < contactPoints.Length; i++)
        {
            if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
            {
                validSurfaceNormal = true; break;
            }
        }

        if (validSurfaceNormal)
        {
            m_isGrounded = true;
            if (!m_collisions.Contains(collision.collider))
            {
                m_collisions.Add(collision.collider);
            }
        }
        else
        {
            if (m_collisions.Contains(collision.collider))
            {
                m_collisions.Remove(collision.collider);
            }
            if (m_collisions.Count == 0) { m_isGrounded = false; }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (m_collisions.Contains(collision.collider))
        {
            m_collisions.Remove(collision.collider);
        }
        if (m_collisions.Count == 0) { m_isGrounded = false; }
    }

    void Update()
    {
        m_animator.SetBool("Grounded", m_isGrounded);
        DirectUpdate();

        m_wasGrounded = m_isGrounded;
    }


    Vector3 targetPoint = new Vector3(-1,-1,-1);
    private void DirectUpdate()
    {
        //Debug.Log(
        //    "Target point: " + targetPoint + ", Jugador: " + transform.position + 
        //    ", InputVector: " + leftJoystick.inputVector);

        if (targetPoint.x == -1)
            targetPoint = transform.position;
        Transform camera = Camera.main.transform;
        Vector3 tForward = camera.forward;
        float cx = -Mathf.Sign(tForward.x);
        float cz = -Mathf.Sign(tForward.z);

        float h = Mathf.Round(leftJoystick.inputVector.x);
        float v = Mathf.Round(leftJoystick.inputVector.y);

        if (targetPoint == transform.position)
        {
            float factor = v != 0 && h != 0 ? 2 : 1;
            targetPoint.x += h * -cz/factor;
            targetPoint.z += h * cx/factor;
            targetPoint.x -= v * cx/factor;
            targetPoint.z -= v * cz/factor;
            targetPoint.x += -(targetPoint.x % 1) + 0.5f * Mathf.Sign(targetPoint.x);
            targetPoint.z += -(targetPoint.z % 1) + 0.5f * Mathf.Sign(targetPoint.z);
        }
        targetPoint.y = 0;
        transform.LookAt(targetPoint);
        float xDiff = Mathf.Abs(transform.position.x - targetPoint.x);
        float zDiff = Mathf.Abs(transform.position.z - targetPoint.z);
        float step = m_moveSpeed * Time.deltaTime;
        float animationSpeed = -1;
        if ((xDiff == 1 || xDiff == 0) && (zDiff == 1 || zDiff == 0) && !(xDiff == 0 && zDiff == 0))
        {
            if (Physics.CheckSphere(new Vector3(targetPoint.x, 0.6f, targetPoint.z), 0.5f))
            {
                Debug.Log("Colisión directa:" + targetPoint + ", Jugador" + transform.position);
                targetPoint = transform.position;
                animationSpeed = 0;
            }
            else if (Physics.CheckSphere(new Vector3(targetPoint.x, 0.6f, transform.position.z), 0.5f) && Physics.CheckSphere(new Vector3(transform.position.x, 0.6f, targetPoint.z), 0.5f))
            {
                Debug.Log("Colisión indirecta:" + targetPoint + ", Jugador" + transform.position);
                targetPoint = transform.position;
                animationSpeed = 0;
            }
        }  
        transform.position = Vector3.MoveTowards(transform.position, targetPoint, step);
        m_animator.SetFloat("MoveSpeed", animationSpeed == -1 ? (Vector3.Distance(transform.position, targetPoint) == 0 ? Mathf.Abs(h) + Mathf.Abs(v) : 1) : animationSpeed);
    }
}

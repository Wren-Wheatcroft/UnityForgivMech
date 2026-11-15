using System.Data.Common;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
	private Rigidbody2D m_RB;
    private Animator m_Animator;
	[SerializeField] private float m_MoveSpeed;
    [SerializeField] private float m_LastMoveDirection;
    [SerializeField] private float m_FacingDirection;
    [SerializeField] private bool m_IsTurning;
    [SerializeField] private SpriteRenderer m_SpriteRenderer;
    [SerializeField] private bool m_IsRunning;
	[SerializeField] private float m_JumpStrength;
	[SerializeField] private Transform m_RaycastPosition;
	[SerializeField] private LayerMask m_GroundLayer;
	private float m_InMove;
    private PlayerControls m_ActionMap;
    private bool m_IsGrounded;

    private void Awake()
    {
        m_ActionMap = new PlayerControls();
		m_RB = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_SpriteRenderer = transform.Find("Sprite").GetComponentInChildren<SpriteRenderer>();

        
    }

    #region Bindings
    private void OnEnable()
    {
        m_ActionMap.Enable();

        m_ActionMap.Default.MoveHoriz.performed += Handle_MovePerformed;
        m_ActionMap.Default.MoveHoriz.canceled += Handle_MoveCancelled;
        m_ActionMap.Default.Jump.performed += Handle_JumpPerformed;
    }

    private void OnDisable()
    {
        m_ActionMap.Disable();

        m_ActionMap.Default.MoveHoriz.performed -= Handle_MovePerformed;
        m_ActionMap.Default.MoveHoriz.canceled -= Handle_MoveCancelled;
        m_ActionMap.Default.Jump.performed -= Handle_JumpPerformed;
    }
    #endregion

    #region InputFunctions

    private void Handle_MovePerformed(InputAction.CallbackContext context)
    {
        m_InMove = context.ReadValue<float>();
    }
    private void Handle_MoveCancelled(InputAction.CallbackContext context)
    {
        m_InMove = 0f;
    }

    private void Handle_JumpPerformed(InputAction.CallbackContext context)
    {
        if (m_IsGrounded)
        {
            m_RB.AddForce(Vector2.up * m_JumpStrength, ForceMode2D.Impulse);
        }
    }
    #endregion


	private void FixedUpdate()
	{
		m_RB.linearVelocityX = m_MoveSpeed * m_InMove;

        m_IsGrounded = Physics2D.Raycast(m_RaycastPosition.position, Vector2.down, 0.1f, m_GroundLayer);

        if (Input.GetKey("left shift") || Input.GetKey("right shift"))
        {
            m_IsRunning = true;
             m_Animator.SetBool("IsRunning", m_IsRunning);
             m_MoveSpeed = 10;
        }
        else 
        {
            m_IsRunning = false;
             m_Animator.SetBool("IsRunning", m_IsRunning);
             m_MoveSpeed = 5;

        }

        if (m_InMove !=0 && !m_IsTurning)
        {
            if (m_LastMoveDirection != 0 && Mathf.Sign(m_InMove) !=m_LastMoveDirection)
            {
                m_Animator.SetTrigger("Turn");
                m_IsTurning = true;
                m_FacingDirection = Mathf.Sign(m_InMove);
            }
  
        }
    
    if (Input.GetKey("d"))
        {
            Debug.Log ("D");
            m_SpriteRenderer.flipX = false; // face right
        }
        else if (Input.GetKey("a"))
        {
            Debug.Log ("A");
            m_SpriteRenderer.flipX = true; // face left
        }
    // animator paraneters
    m_Animator.SetFloat("Speed", Mathf.Abs(m_InMove));
    m_Animator.SetFloat("YVelocity", m_RB.linearVelocityY);
    m_Animator.SetBool("IsGrounded", m_IsGrounded);

    }

  
}

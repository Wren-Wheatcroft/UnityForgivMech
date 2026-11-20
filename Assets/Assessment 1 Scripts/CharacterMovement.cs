using System.Data.Common;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using System.Collections;

public class CharacterMovement : MonoBehaviour
{
	private Rigidbody2D m_RB;
    public Animator m_Animator;
	[SerializeField] public float m_MoveSpeed;
    [SerializeField] private float m_LastMoveDirection;
    [SerializeField] private float m_FacingDirection;
    [SerializeField] private bool m_IsTurning;
    [SerializeField] private float m_CoyoteTime = 0.2f;
    [SerializeField] private float m_CoyoteTimeCounter;
    [SerializeField] private float m_JumpBufferTime;
    [SerializeField] private float m_JumpBufferCounter;
    [SerializeField] private SpriteRenderer m_SpriteRenderer;
    [SerializeField] public bool m_IsRunning;
	[SerializeField] private float m_JumpStrength;
	[SerializeField] private Transform m_RaycastPosition;
	[SerializeField] private LayerMask m_GroundLayer;
    [SerializeField] private bool m_HasJumped;
    [SerializeField] private bool m_JumpedPressed;
    [SerializeField] public float m_JumpCounter;
    private Coroutine m_CoyoteTimeCoroutine;
    public bool m_wasGrounded;
    public bool m_IsGrounded;
    public bool m_SlideActivate;

    public Vector2 m_CrouchSize;
    public Vector2 m_RegSize;


	private float m_InMove;
    private PlayerControls m_ActionMap;


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
         m_ActionMap.Default.Sprint.performed += Handle_SprintPerformed;
         m_ActionMap.Default.Sprint.canceled += Handle_SprintCancelled;
         m_ActionMap.Default.Jump.performed += Handle_JumpPerformed;
         m_ActionMap.Default.Jump.canceled += Handle_JumpCancelled;


       
    }

    private void OnDisable()
    {
        m_ActionMap.Disable();

        m_ActionMap.Default.MoveHoriz.performed -= Handle_MovePerformed;
        m_ActionMap.Default.MoveHoriz.canceled -= Handle_MoveCancelled;
        m_ActionMap.Default.Sprint.performed -= Handle_SprintPerformed;
        m_ActionMap.Default.Sprint.canceled -= Handle_SprintCancelled;
        m_ActionMap.Default.Jump.performed -= Handle_JumpPerformed;
        m_ActionMap.Default.Jump.canceled -= Handle_JumpCancelled;
   
    }
    #endregion

    #region InputFunctions
    private void Jump()
    {
         m_RB.AddForce(Vector2.up * m_JumpStrength, ForceMode2D.Impulse);
         m_CoyoteTimeCounter = 0f;
         m_JumpBufferCounter = 0f;
         
         if (m_CoyoteTimeCoroutine != null)
         {
            StopCoroutine(m_CoyoteTimeCoroutine);
            m_CoyoteTimeCoroutine = null;
         }
         m_IsGrounded = false ;

    }
    private void Handle_MovePerformed(InputAction.CallbackContext context)
    {
        m_InMove = context.ReadValue<float>();

        if(m_InMove > 0)
        {
             m_SpriteRenderer.flipX = false; 
        }

        if(m_InMove < 0)
        {
             m_SpriteRenderer.flipX = true; 
        }
    }
    private void Handle_MoveCancelled(InputAction.CallbackContext context)
    {
        m_InMove = 0f;
    }

    private void Handle_JumpPerformed(InputAction.CallbackContext context)
    {
        Debug.Log("Jump");

        m_JumpedPressed = true;
        m_HasJumped = true;
        m_JumpCounter = 1;
       

      
        m_JumpBufferCounter = m_JumpBufferTime;
        
        if (m_IsGrounded || m_CoyoteTimeCounter > 0f && m_JumpCounter <= 0f) 
        {
            Jump();
        }
        
    
    }

    private void Handle_JumpCancelled(InputAction.CallbackContext context)
    {
     m_JumpedPressed = false;

    }

    private void Handle_SprintPerformed(InputAction.CallbackContext context)
    {
            m_IsRunning = true;
             m_Animator.SetBool("IsRunning", m_IsRunning);
             m_MoveSpeed = 10;
    }

    private void Handle_SprintCancelled(InputAction.CallbackContext context)
    {
            m_IsRunning = false;
             m_Animator.SetBool("IsRunning", m_IsRunning);
             m_MoveSpeed = 5;
    }

    #endregion
    private IEnumerator CoyoteTime()
    {
        m_CoyoteTimeCounter = m_CoyoteTime;
        yield return new WaitForSeconds(m_CoyoteTime);
        m_CoyoteTimeCounter =0f;
        m_CoyoteTimeCoroutine = null;
        m_JumpCounter = 0f;
    }

	private void FixedUpdate()
	{
        m_IsGrounded = Physics2D.Raycast(m_RaycastPosition.position, Vector2.down, 0.1f, m_GroundLayer);

        if(m_RB.linearVelocityY < 1 && m_RB.linearVelocityY > -1 || !m_JumpedPressed && m_HasJumped)
        {
            m_RB.gravityScale = 3;
        }

        bool m_wasGrounded = m_IsGrounded;
        
        // Coyote TIme
        if (m_wasGrounded && !m_IsGrounded && m_CoyoteTimeCoroutine ==null)
        {
            m_CoyoteTimeCoroutine = StartCoroutine(CoyoteTime());

        }
        if (m_IsGrounded)
        {
            if (m_CoyoteTimeCoroutine != null)
            {
                StopCoroutine(m_CoyoteTimeCoroutine);
                m_CoyoteTimeCoroutine = null;
            }
            m_CoyoteTimeCounter = m_CoyoteTime;

        }
        //jump buffer
        if (m_JumpBufferCounter > 0f)
        {
            m_JumpBufferCounter -= Time.deltaTime;
        }
    
        if (m_JumpBufferCounter > 0f && m_CoyoteTimeCounter > 0f)
        {
           Jump();
        }

        if (m_IsGrounded)
        {
          m_RB.gravityScale = 1;
        }
        if (!m_IsGrounded)
        {
            m_CoyoteTimeCounter -= Time.deltaTime;
        }
		m_RB.linearVelocityX = m_MoveSpeed * m_InMove;

        


        if (m_InMove !=0 && !m_IsTurning)
        {
            if (m_LastMoveDirection != 0 && Mathf.Sign(m_InMove) !=m_LastMoveDirection)
            {
                m_Animator.SetTrigger("Turn");
                m_IsTurning = true;
                m_FacingDirection = Mathf.Sign(m_InMove);
            }
  
        }

    // animator paraneters
    m_Animator.SetFloat("Speed", Mathf.Abs(m_InMove));
    m_Animator.SetFloat("YVelocity", m_RB.linearVelocityY);
    m_Animator.SetBool("IsGrounded", m_IsGrounded);


    }

  
}

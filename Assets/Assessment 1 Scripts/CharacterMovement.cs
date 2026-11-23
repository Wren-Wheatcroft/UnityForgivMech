using System;
using System.Data.Common;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using System.Collections;
using System.Collections.Generic;
//using System.Numerics;

public class CharacterMovement : MonoBehaviour
{
    #region Variables!
[Header("Components")]
	private Rigidbody2D m_RB;
    public Animator m_Animator;
    [SerializeField] private Transform m_RaycastPosition;


    [Header("Movement")]
	[SerializeField] public float m_MoveSpeed;
    [SerializeField] private float m_LastMoveDirection;
    [SerializeField] private float m_FacingDirection;
    [SerializeField] private bool m_IsTurning;
    private float m_InMove;

    [Header("Jump")]
    [SerializeField] private float m_CoyoteTime = 0.2f;
    [SerializeField] private float m_CoyoteTimeCounter;
    [SerializeField] private float m_JumpBufferTime;
    [SerializeField] private float m_JumpBufferCounter = 0.5f;
    [SerializeField] private SpriteRenderer m_SpriteRenderer;
    [SerializeField] public bool m_IsRunning;
	[SerializeField] private float m_JumpStrength;
    [SerializeField] private bool m_HasJumped;
    [SerializeField] private bool m_JumpedPressed;
    [SerializeField] public float m_JumpCounter;
    [SerializeField] private AudioClip[] m_JumpAudioClips;
    private Coroutine m_CoyoteTimeCoroutine;

    [Header("Particles")]
    [SerializeField] private ParticleSystem  m_LandingParticles;
    [SerializeField] private ParticleSystem  m_LandingParticlesOnLand;
    
    [Header("Ground Detection")]
    public bool m_wasGrounded;
    public bool m_IsGrounded;
	[SerializeField] private LayerMask m_GroundLayer;
   [SerializeField] private Vector2 m_GroundCheckSize = new Vector2(0.5f,0.1f);
    [SerializeField]private float m_GroundCheckDistance =0.2f;
    [SerializeField] private AudioClip m_GroundedAudioClip;


    [Header("Crouch & Slide")]
    public bool m_SlideActivate;
    public Vector2 m_CrouchSize;
    public Vector2 m_RegSize;


[Header("Dash")]
    [SerializeField] private bool m_IsDashing;
    [SerializeField] private bool m_CanDash = true;
    [SerializeField] private float m_OgGravity;       
    [SerializeField] private float m_DashDirection; 
    [SerializeField] private float m_DashSpeed;
    [SerializeField] public float m_DashDuration;
    [SerializeField] public float m_DashCooldown;
    private Coroutine m_DashCoroutine;
    [SerializeField] private AudioClip[] m_DashAudioClips;

    [Header("Dash Ghost")]
    [SerializeField] private GameObject m_DashGhost;
    [SerializeField] private float m_GhostSpawnRate = 0.05f;
    [SerializeField] private Color m_GhostColor = new Color(1f, 1f, 1f, 0.5f);
    private Coroutine m_GhostCoroutine;


[Header("Corner Correction")]
[SerializeField] private Collider2D m_PlayerCollider;
[SerializeField] public float m_CornerCorrectionAmount = 0.075f;
[SerializeField] private int m_CornerCorrectionSteps =5;

[Header("Gizmos")]
[SerializeField] public bool m_ShowGizmos = true;
private RaycastHit2D[] m_CornerHits = new RaycastHit2D[5];
 private PlayerControls m_ActionMap;

 #endregion
 #region Awake
    private void Awake()
    {
        m_ActionMap = new PlayerControls();
		m_RB = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_SpriteRenderer = transform.Find("Sprite").GetComponentInChildren<SpriteRenderer>();
        m_PlayerCollider = GetComponent<CapsuleCollider2D>();
    }
    #endregion

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
         m_ActionMap.Default.Dash.performed += Handle_DashPerformed;
         m_ActionMap.Default.Dash.canceled += Handle_DashCancelled;




       
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
        m_ActionMap.Default.Dash.performed -= Handle_DashPerformed;
        m_ActionMap.Default.Dash.canceled -= Handle_DashCancelled;

   
    }
    #endregion
#region Jump
    private void Jump()
    {
         m_RB.AddForce(Vector2.up * m_JumpStrength, ForceMode2D.Impulse);
         m_CoyoteTimeCounter = 0f;
         m_JumpBufferCounter = 0f;
         SoundManager.instance.PlayRandomSound(m_JumpAudioClips, transform, 1f);

         
         if (m_CoyoteTimeCoroutine != null)
         {
            StopCoroutine(m_CoyoteTimeCoroutine);
            m_CoyoteTimeCoroutine = null;
         }
         m_IsGrounded = false ;
         m_HasJumped = true;
         m_LandingParticles.Play();
         CornerCorrection(Vector2.right * m_InMove);


#endregion
    }
    #region Corner Correction
    private void CornerCorrection(Vector2 direction)
    {
        for (int i = 1; i<= m_CornerCorrectionSteps; i++)
        {
            float offset = m_CornerCorrectionAmount*i;
            Vector2 correctionOffset = direction.normalized * offset;

            int hitCount = m_PlayerCollider.Cast(direction.normalized, m_CornerHits, offset + 0.01f);
            if (hitCount == 0)
            {
                m_RB.position += correctionOffset;
                break;
            }
        }
    }


    #endregion
        #region InputFunctions

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
        
            Debug.Log("Jump Buffer Counting:" + m_JumpBufferCounter); 

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

    private void Handle_DashPerformed(InputAction.CallbackContext context)
    {
        if (m_CanDash && !m_IsDashing)
        {
            m_DashCoroutine = StartCoroutine(Dash());
            //m_DashParticles.Play();

        }

    }
    private void Handle_DashCancelled(InputAction.CallbackContext context)
    {


    }

        
        
        #endregion

#region Dash
    private IEnumerator Dash()
    {
        m_IsDashing = true;
        m_CanDash = false;
        m_GhostCoroutine = StartCoroutine(Ghost());
        m_OgGravity = m_RB.gravityScale;
        m_RB.gravityScale = 0;
        SoundManager.instance.PlayRandomSound(m_DashAudioClips, transform, 1f);


        if (m_SpriteRenderer.flipX == true)
        {
            m_DashDirection = -1;
        }
        else
        {
            m_DashDirection = 1;
        }

        m_RB.linearVelocity = new Vector2(m_DashDirection * m_DashSpeed, 0);
        yield return new WaitForSeconds (m_DashDuration);

        m_RB.gravityScale = m_OgGravity;
        m_IsDashing = false;

        yield return new WaitForSeconds (m_DashCooldown);
        m_CanDash = true;
        m_DashCoroutine = null;
        CornerCorrection(Vector2.up);

    }
    #endregion
    #region Coyote Time
    private IEnumerator CoyoteTime()
    {
        m_CoyoteTimeCounter = m_CoyoteTime;
        yield return new WaitForSeconds(m_CoyoteTime);
        m_CoyoteTimeCounter =0f;
        m_CoyoteTimeCoroutine = null;
        m_JumpCounter = 0f;
    }
    #endregion
    #region Ghost
    private IEnumerator Ghost()
    {
     while (m_IsDashing)
        {
            GameObject Ghost = Instantiate(m_DashGhost, transform.position, transform.rotation);
            GhostTrail GhostTrailScript = Ghost.GetComponent<GhostTrail>();
            GhostTrailScript.GhostSpawn(m_SpriteRenderer.sprite, m_SpriteRenderer.flipX, m_GhostColor);
            yield return new WaitForSeconds (m_GhostSpawnRate);

        }   
        m_GhostCoroutine = null;
    }

    #endregion
#region land Particles
    private void OnCollisionEnter2D(Collision2D other)
    {

        if (Mathf.RoundToInt(other.GetContact(0).normal.y) == 1)
        {
            m_LandingParticlesOnLand.Play();
            SoundManager.instance.PlaySound(m_GroundedAudioClip, transform, 1f);


        }
    }
    #endregion
#region Update
#endregion
    private void FixedUpdate()
	{


        m_wasGrounded = m_IsGrounded;

        //m_IsGrounded = Physics2D.Raycast(m_RaycastPosition.position, Vector2.down, 0.1f, m_GroundLayer);
        m_IsGrounded = Physics2D.BoxCast(m_RaycastPosition.position, m_GroundCheckSize, 0f, Vector2.down, m_GroundCheckDistance, m_GroundLayer);

  
        if(m_RB.linearVelocityY < 1 && m_RB.linearVelocityY > -1 || !m_JumpedPressed && m_HasJumped)
        {
            m_RB.gravityScale = 3;
        }
        
#region CotyoteTimeActivate
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
#endregion
#region Jump Buffer Activate
        if (m_JumpBufferCounter > 0f)
        {
            m_JumpBufferCounter -= Time.deltaTime;
        }
    
        if (m_JumpBufferCounter > 0f && ( m_IsGrounded || m_CoyoteTimeCounter > 0f))
        {
           Jump();
        }
#endregion
        if (m_IsGrounded)
        {
          m_RB.gravityScale = 1;
        }
        if (!m_IsGrounded)
        {
            m_CoyoteTimeCounter -= Time.deltaTime;
        }
		if (!m_IsDashing)
        {
             m_RB.linearVelocityX = m_MoveSpeed * m_InMove;

        }
        
#region Turning 
        if (m_InMove !=0 && !m_IsTurning)
        {
            if (m_LastMoveDirection != 0 && Mathf.Sign(m_InMove) !=m_LastMoveDirection)
            {
                m_Animator.SetTrigger("Turn");
                m_IsTurning = true;
                m_FacingDirection = Mathf.Sign(m_InMove);
            }
  
        }
#endregion
#region Animator Parameters
    m_Animator.SetFloat("Speed", Mathf.Abs(m_InMove));
    m_Animator.SetFloat("YVelocity", m_RB.linearVelocityY);
    m_Animator.SetBool("IsGrounded", m_IsGrounded);
#endregion

    }

    #region Gizmo
    private void OnDrawGizmos()
    {
       if(!m_ShowGizmos) return;



       // Ground Detect
        if (m_RaycastPosition == null) return;
        Gizmos.color = m_IsGrounded? Color.seaGreen : Color.red;
        Gizmos.DrawWireCube(m_RaycastPosition.position + Vector3.down *m_GroundCheckDistance, m_GroundCheckSize);

        //coyote time
        Gizmos.color = Color.softYellow;
        Gizmos.DrawWireSphere(m_RaycastPosition.position, 0.5f);

        //jump buf
        if (m_JumpBufferCounter > 0)
        {
            Gizmos.color = Color.paleGreen;
            Gizmos.DrawWireSphere (transform.position,0.35f);
        }

        //cornercor
        if(m_PlayerCollider!=null)
        {
            Gizmos.color = Color.darkMagenta;
            Gizmos.DrawWireSphere(m_PlayerCollider.bounds.center, m_CornerCorrectionAmount *m_CornerCorrectionSteps);
        }

        //Player Colid
        if( m_PlayerCollider !=null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube (m_PlayerCollider.bounds.center, m_PlayerCollider.bounds.size);

        }
        
        //Dash
        if (m_IsDashing && m_PlayerCollider != null)
        {
            Gizmos.color = Color.aliceBlue;
            Gizmos.DrawRay ( m_PlayerCollider.bounds.center,Vector3.right *m_DashDirection *1f );

        }
        //movment 
        if (m_InMove !=0)
        {
            Gizmos.color = Color.floralWhite;
            Gizmos.DrawRay(transform.position, Vector3.right* m_InMove *0.5f);
        }

        }
    #endregion
}

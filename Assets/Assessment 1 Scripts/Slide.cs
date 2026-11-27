using UnityEngine;
using System.Data.Common;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using System.Collections;


public class Slide : MonoBehaviour
{
    [SerializeField] private AudioClip m_SlideAudioClip;
    [SerializeField] private float m_SlideDuration = 0.5f;
    private Coroutine m_SlideCoroutine;

    public CharacterMovement m_PlayerMoveVars;
    PlayerControls m_ActionMap2;
    public Animator m_Animator;

     public CapsuleCollider2D capsule;

     private void Awake()
     {
        m_ActionMap2 = new PlayerControls();
       
     }

    private void OnEnable()
    {
        m_ActionMap2.Enable();
        m_ActionMap2.Default.Crouch.performed += Handle_Slide;
        m_ActionMap2.Default.Crouch.canceled += Handle_SlideCancel;

    }
    private void OnDisable()
    {
        m_ActionMap2.Disable();
        m_ActionMap2.Default.Crouch.performed -= Handle_Slide;
        m_ActionMap2.Default.Crouch.canceled -= Handle_SlideCancel;
    }

    private void Start()
    {
        m_PlayerMoveVars.m_RegSize = new Vector2(capsule.size.x, capsule.size.y);
        m_PlayerMoveVars.m_CrouchSize = new Vector2(capsule.size.x, capsule.size.y / 2f);
        m_Animator = GetComponent<Animator>();

    }

    private void Handle_Slide(InputAction.CallbackContext context)
    {
        Debug.Log("crouch pressed");
        if (m_PlayerMoveVars.m_IsRunning && m_PlayerMoveVars.m_IsGrounded)
        {
            m_PlayerMoveVars.m_SlideActivate = true;
            m_PlayerMoveVars.m_MoveSpeed = 15f;
            capsule.size = m_PlayerMoveVars.m_CrouchSize;
            SoundManager.instance.PlaySound(m_SlideAudioClip, transform, 0.5f);
            m_SlideCoroutine = StartCoroutine(m_SlideDurationAnimation());

        }

    }
    private void Handle_SlideCancel(InputAction.CallbackContext context)
    {
    }
    private IEnumerator m_SlideDurationAnimation()
    {
        yield return new WaitForSeconds(m_SlideDuration);
        m_PlayerMoveVars.m_SlideActivate = false;
        capsule.size = m_PlayerMoveVars.m_RegSize;    
        m_SlideCoroutine = null;    
    }
    void Update()
    {
        m_Animator.SetBool("SlideActivate", m_PlayerMoveVars.m_SlideActivate);
    }


}

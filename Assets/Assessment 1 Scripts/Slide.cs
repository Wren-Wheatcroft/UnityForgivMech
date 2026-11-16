using UnityEngine;
using System.Data.Common;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
#include CharacterMovement.cs


public class Slide : MonoBehaviour
{

    private void Handle_Slide(InputAction.CallbackContext context)
    {
     m_SlideActivate = true;
        if (IsRunning = true)
        {
            m_MoveSpeed += 5f;
        }

    }
    private void Handle_SlideCancel(InputAction.CallbackContext context)
    {
     m_SlideActivate = false;

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using UnityEngine;

public class LaunchPadMushroom : MonoBehaviour
{
    public CharacterMovement m_PlayerMoveVars;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Rigidbody2D rb2d))
        {
           // rb2d.AddForce(transform.up * m_PlayerMoveVars.m_JumpStrength, ForceMode2D.Impulse);
        }
    }
}

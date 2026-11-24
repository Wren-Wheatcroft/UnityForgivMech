using UnityEngine;

public class LaunchPadMushroom : MonoBehaviour
{
    public float jumpForce;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Rigidbody2D rb2d))
        {
            rb2d.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        }
    }
}

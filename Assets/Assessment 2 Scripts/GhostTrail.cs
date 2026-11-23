using UnityEngine;
using System.Collections;

public class GhostTrail : MonoBehaviour
{
private SpriteRenderer m_SpriteRenderer;
[SerializeField] private float m_FadeDuration = 0.5f;
float timer;
float alpha;

    void Awake()
    {
        m_SpriteRenderer =GetComponent<SpriteRenderer>();
    }
#region GhostSpawn
public void GhostSpawn(Sprite sprite, bool flipx, Color color)
    {
        m_SpriteRenderer.sprite = sprite;
        m_SpriteRenderer.flipX = flipx;
        m_SpriteRenderer.color = color;
        
        StartCoroutine(DashFade());

    }
    #endregion
#region Dashfade
    private IEnumerator DashFade()
    {
        timer = 0f;
        Color startColor = m_SpriteRenderer.color;

        while (timer <m_FadeDuration)
        {
            while (timer <m_FadeDuration)
            {
                timer += Time.deltaTime;
                
                alpha = Mathf.Lerp(startColor.a, 0f, timer/m_FadeDuration);

                m_SpriteRenderer.color = new Color (startColor.r, startColor.g, startColor.b, alpha);
                yield return null;
            }
            Destroy (gameObject);
        }
    }
}
#endregion 

using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
        [SerializeField] private float m_ShakeDuration = 0.2f;
        [SerializeField] private float m_ShakeMagnitude = 0.15f;
        private Vector3 m_OgPosition;
        private Coroutine m_ShakeCoroutine;
        float m_ElapsedTime =0f;


    private void Awake()
    {
        m_OgPosition = transform.localPosition;

    }
    public void ShakeCamera()
    {
        if (m_ShakeCoroutine != null)
        {
            StopCoroutine(m_ShakeCoroutine);
        }
        m_ElapsedTime = 0f;
        m_ShakeCoroutine = StartCoroutine(Shake());
        

    }
    private IEnumerator Shake()
    {
        while (m_ElapsedTime <m_ShakeDuration)
        {
            float x = Random.Range(-1f, 1f) * m_ShakeMagnitude;
            float y = Random.Range(-1f, 1f) * m_ShakeMagnitude;

            transform.localPosition = m_OgPosition + new Vector3 (x, y, 0f);
            m_ElapsedTime += Time.deltaTime;
            yield return null;
            m_ShakeCoroutine = null;
        }
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class Collectible : MonoBehaviour
{
    public int Score;
    public int Value = 1;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))

        {
            Score = +Value;
            Debug.Log("+" + Value + "Collected - " + "Score Now - " + Score);
            Destroy(gameObject);
        }
    }
}

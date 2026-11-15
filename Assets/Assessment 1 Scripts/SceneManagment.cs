using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using Unity.VisualScripting;

public class SceneManagment : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(C_LoadSceneAsync(1, LoadSceneMode.Additive));
    }

  IEnumerator C_LoadSceneAsync(int sceneIndex, LoadSceneMode sceneMode)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex, sceneMode);

        while (!asyncLoad.isDone)
        {
            yield return null;
            Debug.Log($"Progress: {asyncLoad.progress * 100}");
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}


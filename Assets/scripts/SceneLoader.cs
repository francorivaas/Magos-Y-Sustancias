using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public float timeOne;

    private void Update()
    {
        timeOne -= Time.deltaTime;
        if (timeOne <= 0)
        {
            SceneManager.LoadScene(2);
        }
    }
}

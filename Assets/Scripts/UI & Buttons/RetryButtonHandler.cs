using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RetryButtonHandler : MonoBehaviour
{
    public void RetryLevel(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }

    public void GoToWorkshop (string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }
}

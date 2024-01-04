using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public int sceneIndexNo;
  public void StartToPlay()
    {
        Debug.Log("scene loaded");
        SceneManager.LoadScene(sceneIndexNo);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CoverButton : MonoBehaviour
{
    [SerializeField] public string sceneName;
    public void onClick()
    {
        SceneManager.LoadScene(sceneName);
    }
}

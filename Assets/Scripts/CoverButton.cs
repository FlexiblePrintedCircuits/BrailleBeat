using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CoverButton : MonoBehaviour
{
    [SerializeField] public string sceneName;
    public void onClick()
    {
        GameObject name = this.transform.parent.Find("name").gameObject;
        ScoreData.Instance.title = name.GetComponent<Text>().text;
        ScoreData.Instance.img = this.GetComponent<Image>().sprite;
        SceneManager.LoadScene(sceneName);
    }
}

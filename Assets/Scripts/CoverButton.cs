using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CoverButton : MonoBehaviour
{
	[SerializeField] public string sceneName;
	[SerializeField] public string musicPath;

	public void onClick()
	{
		GameObject name = this.transform.parent.Find("name").gameObject;

		ScoreData.Instance.title = name.GetComponent<Text>().text;
		ScoreData.Instance.img = this.GetComponent<Image>().sprite;
		ScoreData.Instance.musicPath = musicPath;
		FadeManager.Instance.LoadScene(sceneName, 1f);
	}
}

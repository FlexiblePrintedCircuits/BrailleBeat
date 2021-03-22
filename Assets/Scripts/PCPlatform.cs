using UnityEngine;

public class PCPlatform
{
	static bool isPC
	{
		get => Application.platform == RuntimePlatform.OSXEditor
			|| Application.platform == RuntimePlatform.OSXPlayer
			|| Application.platform == RuntimePlatform.WindowsEditor
			|| Application.platform == RuntimePlatform.WindowsPlayer
			|| Application.platform == RuntimePlatform.LinuxEditor
			|| Application.platform == RuntimePlatform.LinuxPlayer
			|| Application.platform == RuntimePlatform.WebGLPlayer;
	}
}
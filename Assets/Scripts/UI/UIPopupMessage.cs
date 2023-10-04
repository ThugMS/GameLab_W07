using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPopupMessage : MonoBehaviour
{
	#region PublicVariables
	public static UIPopupMessage instance;
	#endregion

	#region PrivateVariables
	private TextMeshProUGUI text;
	#endregion

	#region PublicMethod
	public void Activate()
	{
		gameObject.SetActive(true);
	}
	public void Deactivate()
	{
		gameObject.SetActive(false);
	}
	public void PrintText(string _text)
	{
		Deactivate();
		text.text = _text;
		Activate();
	}
	#endregion

	#region PrivateMethod
	private void Awake()
	{
		if (instance == null)
			instance = this;
		transform.Find("Text").TryGetComponent(out text);
		Deactivate();
	}
	#endregion
}

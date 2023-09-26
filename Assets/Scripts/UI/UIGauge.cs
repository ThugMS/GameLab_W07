using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIGauge : MonoBehaviour
{
	#region PublicVariables
	#endregion

	#region PrivateVariables
	private TextMeshProUGUI text;
	private Image fill;
	#endregion

	#region PublicMethod
	public void UpdateValue(float percentage01)
	{
		text.text = Mathf.Floor(percentage01 * 100).ToString() + "%";
		fill.material.SetFloat("_ClipUvRight", 1 - percentage01);
	}
	#endregion

	#region PrivateMethod
	private void Awake()
	{
		transform.Find("Text").TryGetComponent(out text);
		transform.Find("Fill").TryGetComponent(out fill);
	}
	#endregion
}

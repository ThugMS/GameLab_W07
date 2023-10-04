using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class UIGauge : MonoBehaviour
{
	#region PublicVariables
	#endregion

	#region PrivateVariables
	private TextMeshProUGUI text;
	private Image fill;
	private float oldValue;
	#endregion

	#region PublicMethod
	public void Initialize()
	{
		transform.Find("Text").TryGetComponent(out text);
		transform.Find("Fill").TryGetComponent(out fill);
		oldValue = 1 - fill.material.GetFloat("_ClipUvRight");
	}
	public void UpdateValue(float percentage01)
	{
		if(percentage01 > oldValue + 0.03f)
		{
			text.DOScale(1f, 0.2f).From(1.6f);
			text.DOColor(Color.white, 0.2f).From(Color.green);
		}
		oldValue = percentage01;
		text.text = Mathf.Floor(percentage01 * 100).ToString() + "%";
		fill.material.SetFloat("_ClipUvRight", 1 - percentage01);
	}
	#endregion

	#region PrivateMethod
	#endregion
}

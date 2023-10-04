using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecallGauge : MonoBehaviour
{
	#region PublicVariables
	#endregion

	#region PrivateVariables
	private SpriteRenderer fill;
	#endregion

	#region PublicMethod
	public void Initialize()
	{
		transform.Find("Fill").TryGetComponent(out fill);
		UpdateValue(0);
		gameObject.SetActive(false);
	}
	public void UpdateValue(float percentage01)
	{
		percentage01 = Mathf.Clamp01(percentage01);
		fill.material.SetFloat("_ClipUvRight", 1f - percentage01);
	}
	#endregion

	#region PrivateMethod
	#endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using DG.Tweening;

public class UIVIgnette : MonoBehaviour
{
	#region PublicVariables
	#endregion

	#region PrivateVariables
	private Volume volume;
	private UnityEngine.Rendering.Universal.Vignette vignette;
	#endregion

	#region PublicMethod
	public void Initialize()
	{
		TryGetComponent(out volume);
		volume.profile.TryGet(out vignette);
	}
	public void UpdateIntensity(float _value)
	{
		vignette.intensity.Override(_value);
	}
	#endregion

	#region PrivateMethod
	#endregion
}

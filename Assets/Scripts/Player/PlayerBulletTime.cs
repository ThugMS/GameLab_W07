using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerBulletTime : MonoBehaviour
{
	#region PublicVariables
	#endregion

	#region PrivateVariables
	private RippleEffect rippleEffect;
	[SerializeField] private Volume volume;
	private UnityEngine.Rendering.Universal.ColorAdjustments colorAdjustments;

	[SerializeField][ReadOnly] private float currentSaturation;
	private const float SATURATION_IDLE = 0f;
	private const float SATURATION_GREY = -100f;
	[SerializeField] private float saturateSpeed = 1;

	private bool isCalled;
	#endregion

	#region PublicMethod
	public void Initialize()
	{
		transform.Find("Ripple Effect").TryGetComponent(out rippleEffect);
		volume.profile.TryGet(out colorAdjustments);
	}
	public void OnActionPerformed()
	{
		rippleEffect.gameObject.SetActive(true);
		isCalled = true;
		Time.timeScale = 0.1f;
	}
	public void OnActionCanceled()
	{
		rippleEffect.gameObject.SetActive(false);
		isCalled = false;
		currentSaturation = SATURATION_IDLE;
		colorAdjustments.saturation.Override(currentSaturation);
		Time.timeScale = 1f;
	}
	public void HandleInput()
	{
		if(isCalled == true)
		{
			currentSaturation = Mathf.Lerp(currentSaturation, SATURATION_GREY, saturateSpeed * Time.unscaledDeltaTime);
			colorAdjustments.saturation.Override(currentSaturation);
		}
	}
	public void ForceQuit()
	{

	}
	#endregion

	#region PrivateMethod
	#endregion
}

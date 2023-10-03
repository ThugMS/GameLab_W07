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
	private PlayerDeadEye deadEye;
	private PlayerMove move;
	private RippleEffect rippleEffect;
	private Volume volume;
	private UnityEngine.Rendering.Universal.ColorAdjustments colorAdjustments;
	private ParticleSystem dustParticle;
	private ParticleSystem dashParticle;

	private float currentSaturation;
	private const float SATURATION_IDLE = 0f;
	private const float SATURATION_GREY = -100f;
	[SerializeField] private float saturateSpeed = 1;
	[SerializeField] private float deadEyeConsumeMult;
	[SerializeField] private float speedMult;
	private float deadEyeCooldown = 1f;

	private bool isCalled;
	private bool isReady;
	#endregion

	#region PublicMethod
	public void Initialize()
	{
		isReady = true;
		TryGetComponent(out deadEye);
		TryGetComponent(out move);
		transform.Find("Ripple Effect").TryGetComponent(out rippleEffect);
		GameObject.Find("Global Volume").TryGetComponent(out volume);
		volume.profile.TryGet(out colorAdjustments);
		transform.Find("Dust Trail").TryGetComponent(out dustParticle);
		transform.Find("Dash Trail").TryGetComponent(out dashParticle);
	}
	public void OnActionPerformed()
	{
		if (isReady == false)
			return;
		isReady = false;
		rippleEffect.gameObject.SetActive(true);
		isCalled = true;
		Time.timeScale = 0.1f;
		dustParticle.Stop();
		int scaleX = transform.position.x - Utils.MousePosition.x > 0 ? 1 : -1;
		dashParticle.transform.localScale = new Vector3(scaleX, 1, 1);
		dashParticle.Play();
		move.SetSpeedMult(speedMult);
	}
	public void OnActionCanceled()
	{
		ForceQuit();
	}
	public void HandleInput()
	{
		if(isCalled == true)
		{
			deadEye.ChangeValue(-deadEyeConsumeMult * Time.unscaledDeltaTime);
			if(deadEye.Value <= 0)
			{
				ForceQuit();
			}
			currentSaturation = Mathf.Lerp(currentSaturation, SATURATION_GREY, saturateSpeed * Time.unscaledDeltaTime);
			colorAdjustments.saturation.Override(currentSaturation);
		}
	}
	public void ForceQuit()
	{
		Invoke(nameof(Ready), deadEyeCooldown);
		rippleEffect.gameObject.SetActive(false);
		isCalled = false;
		currentSaturation = SATURATION_IDLE;
		colorAdjustments.saturation.Override(currentSaturation);
		Time.timeScale = 1f;
		dustParticle.Play();
		dashParticle.Stop();
		move.SetSpeedMult(1f);
	}
	#endregion

	#region PrivateMethod
	private void Ready()
	{
		isReady = true;
	}
	#endregion
}

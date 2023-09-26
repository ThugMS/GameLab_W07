using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RippleEffect : MonoBehaviour
{
	#region PublicVariables
	#endregion

	#region PrivateVariables
	private SpriteRenderer sr;
	[SerializeField] private float waveDistanceStart;
	[SerializeField] private float waveDistanceEnd;
	[SerializeField] private float duration;
	#endregion

	#region PublicMethod
	public void Initialize()
	{
		transform.position = Player.instance.transform.position;
		sr.material.SetFloat("_WaveDistanceFromCenter", waveDistanceStart);
	}
	#endregion

	#region PrivateMethod
	private void OnEnable()
	{
		TryGetComponent(out sr);
		Initialize();
		EffectStart();
	}
	private void OnDisable()
	{
		sr.material.DOKill();
	}
	private void EffectStart()
	{
		sr.material.DOFloat(waveDistanceEnd, "_WaveDistanceFromCenter", duration).SetUpdate(true);
		Invoke(nameof(DeactiveSelf), duration);
	}
	private void DeactiveSelf()
	{
		gameObject.SetActive(false);
	}
	#endregion
}

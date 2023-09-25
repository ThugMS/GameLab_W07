using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
	#region PublicVariables
	#endregion

	#region PrivateVariables
	private Player main;
	private Rigidbody2D rb;
	private ParticleSystem dustParticle;
	private ParticleSystem dashParticle;
	private Bow bow;

	[SerializeField] private float dashSpeed;
	[SerializeField] private float dashDuration;
	[SerializeField] private float dashCooldown;

	private bool isReady = true;
	#endregion

	#region PublicMethod
	public void Initialize()
	{
		TryGetComponent(out rb);
		TryGetComponent(out main);
		transform.Find("Bow").TryGetComponent(out bow);
		transform.Find("Dust Trail").TryGetComponent(out dustParticle);
		transform.Find("Dash Trail").TryGetComponent(out dashParticle);
	}
	public void Dash()
	{
		if (isReady == false)
		{
			return;
		}
		isReady = false;
		//anim.SetBool("dash", true);
		bow.SetRendererVisibility(false);
		rb.velocity = (Utils.MousePosition - (Vector2)transform.position).normalized * dashSpeed;
		main.SetActive(false);
		main.SetInvincibility(true);
		dustParticle.Stop();
		int scaleX = transform.position.x - Utils.MousePosition.x > 0 ? 1 : -1;
		dashParticle.transform.localScale = new Vector3(scaleX, 1, 1);
		dashParticle.Play();

		Invoke(nameof(DashReady), dashCooldown);
		Invoke(nameof(DashEnd), dashDuration);
	}
	public void ForceDodgeEnd()
	{
		CancelInvoke(nameof(DashEnd));
		DashEnd();
	}
	#endregion

	#region PrivateMethod
	private void DashEnd()
	{
		rb.velocity = Vector2.zero;
		main.SetActive(true);
		main.SetInvincibility(false);
		bow.SetRendererVisibility(true);
		dustParticle.Play();
		dashParticle.Stop();
	}
	private void DashReady()
	{
		isReady = true;
	}
	#endregion
}

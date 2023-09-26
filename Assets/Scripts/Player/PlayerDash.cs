using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
	#region PublicVariables
	#endregion

	#region PrivateVariables
	private Player main;
	private PlayerMove move;
	private Rigidbody2D rb;
	private ParticleSystem dustParticle;
	private ParticleSystem dashParticle;
	private Bow bow;

	[SerializeField] private float dashSpeed;
	[SerializeField] private float dashDuration;
	[SerializeField] private float dashCooldown;

	[SerializeField][ReadOnly] private bool isReady = true;
	#endregion

	#region PublicMethod
	public void Initialize()
	{
		TryGetComponent(out rb);
		TryGetComponent(out main);
		TryGetComponent(out move);
		transform.Find("Bow").TryGetComponent(out bow);
		transform.Find("Dust Trail").TryGetComponent(out dustParticle);
		transform.Find("Dash Trail").TryGetComponent(out dashParticle);
	}
	public void Dash()
	{
		if (isReady == false || move.Direction == Vector2.zero)
		{
			return;
		}
		isReady = false;
		//anim.SetBool("dash", true);
		bow.SetRendererVisibility(false);
		rb.velocity = move.Direction * dashSpeed;
		main.SetActive(false);
		main.SetInvincibility(true);
		dustParticle.Stop();
		int scaleX = transform.position.x - Utils.MousePosition.x > 0 ? 1 : -1;
		dashParticle.transform.localScale = new Vector3(scaleX, 1, 1);
		dashParticle.Play();

		Invoke(nameof(DashReady), dashCooldown);
		Invoke(nameof(DashEnd), dashDuration);
	}
	public void ForceQuit()
	{
		CancelInvoke(nameof(DashEnd));
		DashEnd();
	}
	#endregion

	#region PrivateMethod
	private void DashEnd()
	{
		Debug.Log("End");
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

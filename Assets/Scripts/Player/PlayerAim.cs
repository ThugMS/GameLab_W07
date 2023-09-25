using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
	#region PublicVariables
	#endregion

	#region PrivateVariables
	private Bow bow;
	private Body body;

	[SerializeField] private float shotCooldown = 0.5f;
	private float cooldownTimer = 0f;
	private bool isCalled;
	#endregion

	#region PublicMethod
	public void Initialize()
	{
		transform.Find("Bow").TryGetComponent(out bow);
		bow.Initialize();
		transform.Find("Renderer").TryGetComponent(out body);
		body.Initialize();
	}
	public void OpenFire()
	{
		isCalled = true;
		cooldownTimer = 0.5f;
	}
	public void HoldFire()
	{
		isCalled = false;
		cooldownTimer = 0f;
	}
	public void HandleInput()
	{
		bow.Look(Utils.MousePosition);
		body.SetSpriteDirection(Utils.MousePosition);
		Fire();
	}
	#endregion

	#region PrivateMethod
	private void Fire()
	{
		if(isCalled == true)
		{
			cooldownTimer += Time.deltaTime;
			if(cooldownTimer > shotCooldown)
			{
				cooldownTimer = 0f;
				bow.Fire();
			}
		}
	}
	#endregion
}

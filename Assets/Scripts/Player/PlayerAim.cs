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
		cooldownTimer = shotCooldown;
	}
	public void OpenFire()
	{
		isCalled = true;
	}
	public void HoldFire()
	{
		isCalled = false;
	}
	public void Recall()
	{
		bow.Recall();
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
		cooldownTimer += Time.unscaledDeltaTime;
		if (isCalled == true)
		{
			if(cooldownTimer > shotCooldown)
			{
				cooldownTimer = 0f;
				if(bow.CheckForExtraArrows())
				{
					bow.Fire();
				}
				else
				{
					bow.Recall();
				}
			}
		}
	}
	#endregion
}

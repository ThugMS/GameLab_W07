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

	}
	public void HoldFire()
	{

	}
	public void HandleInput()
	{
		bow.Look(Utils.MousePosition);
		body.SetSpriteDirection(Utils.MousePosition);
	}
	#endregion

	#region PrivateMethod
	#endregion
}

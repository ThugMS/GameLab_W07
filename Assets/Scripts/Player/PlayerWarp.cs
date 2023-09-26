using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWarp : MonoBehaviour
{
	#region PublicVariables
	#endregion

	#region PrivateVariables
	private LineRenderer lr;
	private Player main;
	[SerializeField] private GameObject warpFeather;
	[SerializeField] private float chargingSpeed;
	[SerializeField] private float chargingInitDistance;
	[SerializeField] private float chargingMaxDistance;
	[SerializeField] private float warpCooldown;

	private float chargingCurrentDistance;
	private bool isCalled = false;
	[SerializeField][ReadOnly] private bool isReady = true;
	private Vector2 offset = new Vector2(0.7f, 0.2f);
	#endregion

	#region PublicMethod
	public void Initialize()
	{
		isCalled = false;
		isReady = true;
		TryGetComponent(out main);
		chargingCurrentDistance = chargingInitDistance;
		transform.Find("Line").TryGetComponent(out lr);
	}
	public void OnActionPerformed()
	{
		if (isReady == false)
			return;

		if(warpFeather.activeSelf == true)
		{
			Warp();
		}
		else
		{
			isCalled = true;
			warpFeather.SetActive(true);
		}
	}
	public void OnActionCanceled()
	{
		isCalled = false;
		if(warpFeather.activeSelf == true)
		{
			Shot();
		}
	}
	public void HandleInput()
	{
		if(isCalled == true)
		{
			Charging();
		}
	}
	public void ForceQuit()
	{
		chargingCurrentDistance = chargingInitDistance;
	}
	#endregion

	#region PrivateMethod
	private void Charging()
	{
		warpFeather.transform.position = (Vector2)main.transform.position + offset;
		chargingCurrentDistance = Mathf.Clamp(chargingCurrentDistance + chargingSpeed * Time.deltaTime, 0, chargingMaxDistance);
		Vector2 targetDirection = (Utils.MousePosition - (Vector2)transform.position).normalized;
		lr.SetPosition(0, targetDirection * chargingInitDistance);
		RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDirection, chargingCurrentDistance, 1 << LayerMask.NameToLayer("Wall"));
		if(hit.collider == null)
		{
			lr.SetPosition(1, targetDirection * chargingCurrentDistance);
		}
		else
		{
			lr.SetPosition(1, hit.point - (Vector2)transform.position);
		}

	}
	private void Shot()
	{
		chargingCurrentDistance = chargingInitDistance;
		warpFeather.transform.position = transform.position + lr.GetPosition(1);
		lr.SetPosition(1, Vector2.zero);
	}
	private void Warp()
	{
		isReady = false;
		main.SetPosition(warpFeather.transform.position);
		warpFeather.SetActive(false);
		Invoke(nameof(Ready), warpCooldown);
	}
	private void Ready()
	{
		isReady = true;
	}
	#endregion
}
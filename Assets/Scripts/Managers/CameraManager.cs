using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
	#region PublicVariables
	public static CameraManager instance;
	#endregion

	#region PrivateVariables
	[SerializeField] private float camSpeed;
	[SerializeField] private float maxMouseMult;
	[SerializeField] private float maxMouseMultUpdateDistance;
	#endregion

	#region PublicMethod
	#endregion

	#region PrivateMethod
	private void Awake()
	{
		if(instance == null)
		{
			instance = this;
		}
	}
	private void Update()
	{
		FollowPlayer();
	}
	private void FollowPlayer()
	{
		Vector2 targetPosition = Player.instance.transform.position;
		float mouseMult = Mathf.Lerp(0, maxMouseMult, Vector2.Distance(Player.instance.transform.position, Utils.MousePosition) / maxMouseMultUpdateDistance);
		Vector2 mouseDirection = (Utils.MousePosition - (Vector2)Player.instance.transform.position).normalized;
		targetPosition += mouseDirection * mouseMult;
		transform.position = Vector2.Lerp(transform.position, targetPosition, camSpeed * Time.unscaledDeltaTime);
	}
	#endregion
}

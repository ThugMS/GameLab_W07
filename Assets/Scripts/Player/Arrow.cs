using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
	#region PublicVariables
	#endregion

	#region PrivateVariables
	private float speed;
	private Vector2 direction;
	#endregion

	#region PublicMethod
	[Button]
	public void SetDirection(Vector2 _direction)
	{
		direction = _direction;
		Quaternion.LookRotation(_direction, Vector2.up);
	}
	public void SetSpeed(float _speed)
	{
		speed = _speed;
	}
	#endregion

	#region PrivateMethod
	private void Update()
	{
		//transform.position +=
	}
	#endregion
}

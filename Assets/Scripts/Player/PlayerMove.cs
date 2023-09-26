using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
	#region PublicVariables
	public Vector2 Direction { get { return direction; } }
	#endregion

	#region PrivateVariables
	private PlayerDeadEye deadEye;
	private Animator anim;

	private Vector2 direction;
	[SerializeField] private float speed;
	[SerializeField] private float deadEyeAdditive;
	#endregion

	#region PublicMethod
	public void Initialize()
	{
		TryGetComponent(out deadEye);
		transform.Find("Renderer").TryGetComponent(out anim);
	}
	public void Move(Vector2 _direction)
	{
		direction = _direction;
		anim.SetBool("move", true);
	}
	public void Stop()
	{
		direction = Vector2.zero;
		anim.SetBool("move", false);
	}
	public void HandleInput()
	{
		if (direction != Vector2.zero)
		{
			deadEye.Add(deadEyeAdditive * Time.deltaTime);
		}
		transform.Translate(direction * speed * Time.deltaTime);
	}
	#endregion

	#region PrivateMethod
	#endregion
}

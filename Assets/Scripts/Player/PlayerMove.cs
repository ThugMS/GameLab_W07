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
	private float speedMult = 1f;
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
			Debug.DrawRay(transform.position, direction * 0.5f, Color.red);
			RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 0.5f, 1 << LayerMask.NameToLayer("Wall"));
			if(hit.collider == null)
			{
				deadEye.ChangeValue(deadEyeAdditive * Time.unscaledDeltaTime);
				transform.Translate(direction * speed * speedMult * Time.unscaledDeltaTime);
			}
		}
	}
	public void SetSpeedMult(float _value)
	{
		speedMult = _value;
	}
	#endregion

	#region PrivateMethod
	#endregion
}

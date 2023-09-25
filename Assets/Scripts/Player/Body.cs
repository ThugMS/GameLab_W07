using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : MonoBehaviour
{
	#region PublicVariables
	public enum EBodyDirection
	{
		down = 0,
		up = 1
	}
	#endregion

	#region PrivateVariables
	private SpriteRenderer sr;
	[SerializeField] private Sprite[] sprites;
	#endregion

	#region PublicMethod
	public void Initialize()
	{
		TryGetComponent(out sr);
	}
	public void SetSpriteDirection(Vector2 _mousePosition)
	{
		Vector2 direction = _mousePosition - (Vector2)transform.position;
		if (direction.x > 0)
		{
			sr.flipX = true;
		}
		else
		{
			sr.flipX = false;
		}

		if (direction.y <= 0)
		{
			sr.sprite = sprites[(int)EBodyDirection.down];
		}
		else if (direction.y > 0)
		{
			sr.sprite = sprites[(int)EBodyDirection.up];
		}
	}
	#endregion

	#region PrivateMethod
	#endregion
}

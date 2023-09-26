using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpFeather : MonoBehaviour
{
	#region PublicVariables
	#endregion

	#region PrivateVariables
	[SerializeField] private float distanceThreshold;
	[SerializeField] private float speed;
	private bool isStuck;
	#endregion

	#region PublicMethod
	public void SetStuck(bool b)
	{
		isStuck = b;
	}
	#endregion

	#region PrivateMethod
	private void Update()
	{
        if(isStuck == false)
        {
			if(Vector2.Distance((Vector2)transform.position, (Vector2)Player.instance.transform.position) > distanceThreshold)
			{
				transform.position = Vector2.Lerp(transform.position, (Vector2)Player.instance.transform.position, speed * Time.deltaTime);
			}
		}
	}
	#endregion
}

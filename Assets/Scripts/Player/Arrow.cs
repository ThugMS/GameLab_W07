using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Arrow : MonoBehaviour
{
	#region PublicVariables
	#endregion

	#region PrivateVariables
	private float speed;
	private Vector2 direction;
	#endregion

	#region PublicMethod
	public void SetDirection(Vector3 _rotation)
	{
		transform.eulerAngles = _rotation;
		//TEMP
		Invoke(nameof(Deactive), 5f);
		//TEMPEND
	}
	public void SetSpeed(float _speed)
	{
		speed = _speed;
	}
	public void Deactive()
	{
		gameObject.SetActive(false);
	}
	#endregion

	#region PrivateMethod
	private void Update()
	{
		transform.position += transform.up * speed * Time.deltaTime;
	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
		// if stuck stop this.
	}
	private void Stop()
	{
		speed = 0f;
	}
	#endregion
}

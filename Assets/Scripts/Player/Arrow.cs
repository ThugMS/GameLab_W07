using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem.Processors;

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
		Invoke(nameof(Deactive), 10f);
		//TEMPEND
	}
    public float GetSpeed()
    {
        return speed;
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
		if(collision != null)
		{
			if(collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
			{
				Stop();
			}

			if(collision.gameObject.layer == LayerMask.NameToLayer("Shield"))
			{
				Deactive();
			}

			if(collision.gameObject.layer == LayerMask.NameToLayer("Monster"))
			{
				MonsterBase monster = collision.gameObject.GetComponent<MonsterBase>();
				monster.GetDamage();
				Deactive();
			}
		}
	}
	private void Stop()
	{
		Debug.Log("stop");
		speed = 0f;
	}
	#endregion
}

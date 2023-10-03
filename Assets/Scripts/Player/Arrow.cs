using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem.Processors;
using UnityEngine.Rendering.Universal;

public class Arrow : MonoBehaviour
{
	#region PublicVariables
	#endregion

	#region PrivateVariables
	private SpriteRenderer sr;
	private ParticleSystem ps;
	private Vector2 direction;
	#endregion

	#region PublicMethod
	public void Initialize()
	{
		TryGetComponent(out sr);
		transform.Find("Trail").TryGetComponent(out ps);
	}
	public void SetDirection(Vector3 _rotation)
	{
		transform.eulerAngles = _rotation;
	}
    public void Shot()
	{
		RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, transform.up, float.MaxValue
			, 1 << LayerMask.NameToLayer("Monster") | 1 << LayerMask.NameToLayer("Wall"))
			.OrderBy(hit => hit.distance)
			.ToArray();

		for(int i = 0; i < hits.Length; ++i)
		{
			if (hits[i].collider.gameObject.layer == LayerMask.NameToLayer("Wall"))
			{
				transform.DOMove(hits[i].point, 0.1f);
				break;
			}
			else
			{
				MonsterBase monster = hits[i].collider.gameObject.GetComponent<MonsterBase>();
				monster.GetDamage();
			}
		}
	}
	public void Recall()
	{
		RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, (Player.instance.transform.position - transform.position).normalized, float.MaxValue
			, 1 << LayerMask.NameToLayer("Monster"))
			.OrderBy(hit => hit.distance)
			.ToArray();

		for (int i = 0; i < hits.Length; ++i)
		{
			MonsterBase monster = hits[i].collider.gameObject.GetComponent<MonsterBase>();
			monster.GetDamage();
		}
		ForceRecall();
	}
	public void ForceRecall()
	{
		sr.enabled = false;
		transform.position = Player.instance.transform.position;
		Invoke(nameof(DestroySelf), 2f);
	}
	#endregion

	#region PrivateMethod
	private void DestroySelf()
	{
		Destroy(gameObject);
	}
	#endregion
}

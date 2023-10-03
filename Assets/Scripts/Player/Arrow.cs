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
	private Collider2D col;
	private Vector2 direction;

	private Tweener moveTween;
	[SerializeField] private float speed;
	private float speedMult = 1;

	private bool isStuck;
	private bool isRecalled;
	#endregion

	#region PublicMethod
	public void Initialize()
	{
		TryGetComponent(out sr);
		TryGetComponent(out col);
		transform.Find("Halo").TryGetComponent(out ps);
		moveTween = transform.DOMove(Player.instance.transform.position, speed).SetAutoKill(false).SetSpeedBased().SetUpdate(true).Pause();
		isStuck = false;
		isRecalled = false;
	}
	public void SetSpeed(float _speed)
	{
		speed = _speed;
	}
	public void SetDirection(Vector3 _rotation)
	{
		transform.eulerAngles = _rotation;
	}
	public void Shot()
	{
		RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, float.MaxValue
			, 1 << LayerMask.NameToLayer("Wall"));
		if (hit.collider != null)
		{
			transform.DOMove(hit.point, speed).SetSpeedBased().OnComplete(() => isStuck = true);
		}
	}
	public void Recall()
	{
		isRecalled = true;
	}
	#endregion

	#region PrivateMethod
	private void Update()
	{
		if (isStuck == true && isRecalled == true && moveTween != null)
		{
			//speedMult += Time.unscaledDeltaTime * 0.4f;
			moveTween.ChangeEndValue(Player.instance.transform.position, speed * speedMult, true).Restart();
			if (Vector2.Distance(transform.position, Player.instance.transform.position) < 0.2f)
			{
				isStuck = false;
				moveTween.Kill();
				moveTween = null;
				sr.enabled = false;
				col.enabled = false;
				ps.gameObject.SetActive(false);
				Invoke(nameof(DestroySelf), 2f);
			}
		}
	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision != null)
		{
			if (collision.gameObject.layer == LayerMask.NameToLayer("Monster"))
			{
				MonsterBase monster = collision.gameObject.GetComponent<MonsterBase>();
				monster.GetDamage();
			}
		}
	}
	private void DestroySelf()
	{
		Destroy(gameObject);
	}
	#endregion
}

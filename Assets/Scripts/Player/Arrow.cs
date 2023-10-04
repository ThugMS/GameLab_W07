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
	private PlayerDeadEye deadEye;

	private SpriteRenderer sr;
	private ParticleSystem psHalo;
	private ParticleSystem psTrail;
	private Collider2D col;

	private Vector3 destination;

	private Tweener recallTween;
	private Tweener shotTween;
	[SerializeField] private float speed;

	private bool isStuck;
	private bool isRecalled;
	private bool isHitEnemy = false;
	#endregion

	#region PublicMethod
	public void Initialize()
	{
		TryGetComponent(out sr);
		TryGetComponent(out col);
		transform.Find("Halo").TryGetComponent(out psHalo);
		transform.Find("Trail").TryGetComponent(out psTrail);
		recallTween = transform.DOMove(Player.instance.transform.position, speed).SetAutoKill(false).SetEase(Ease.Linear).SetSpeedBased().SetUpdate(true).Pause();
		isStuck = false;
		isRecalled = false;
		destination = Vector2.zero;
		Player.instance.TryGetComponent(out deadEye);
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
			destination = hit.point;
			shotTween = transform.DOMove(destination, speed).SetEase(Ease.Linear).SetAutoKill(false).SetSpeedBased().SetUpdate(true).Play();
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
		if(isStuck == false && isHitEnemy == false && destination != Vector3.zero)
		{
			if(transform.position != destination)
				shotTween.ChangeEndValue(destination, speed, true).Restart();
			if (Vector2.Distance(transform.position, destination) < 0.1f)
			{
				isStuck = true;
			}
		}
		if (isStuck == true && isRecalled == true && recallTween != null && isHitEnemy == false)
		{
			if(transform.position != Player.instance.transform.position)
				recallTween.ChangeEndValue(Player.instance.transform.position, speed, true).Restart();
			if (Vector2.Distance(transform.position, Player.instance.transform.position) < 0.2f)
			{
				sr.enabled = false;
				col.enabled = false;
				psHalo.gameObject.SetActive(false);
				psTrail.Stop();
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
				if(isRecalled == false)
				{
					deadEye.ChangeValue(1);
				}
				else
				{
					deadEye.ChangeValue(3);
				}
				MonsterBase monster = collision.gameObject.GetComponent<MonsterBase>();
				monster.GetDamage();
				StartCoroutine(nameof(TimeStuck), 0.1f);
			}
		}
	}
	private void DestroySelf()
	{
		recallTween.Kill();
		shotTween.Kill();
		Destroy(gameObject);
	}

	private IEnumerator TimeStuck(float _time)
	{
		StopArrow();
		yield return new WaitForSeconds(_time);
		MoveArrow();
	}
	private void MoveArrow()
	{
		if(isRecalled == false)
		{
			isHitEnemy = false;
			shotTween.Play();
		}
		else
		{
			isHitEnemy = false;
			recallTween.Restart();
		}
	}
	private void StopArrow()
	{
		if (isRecalled == false)
		{
			isHitEnemy = true;
			shotTween.Pause();
		}
		else
		{
			isHitEnemy = true;
			recallTween.Pause();
		}
	}
	#endregion
}

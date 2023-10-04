using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour
{
	#region PublicVariables
	#endregion

	#region PrivateVariables
	private SpriteRenderer sr;
	private Animator anim;
	private RecallGauge recallGauge;
	[SerializeField] private GameObject arrowPrefab;
	private List<Arrow> arrowList = new List<Arrow>();
	[SerializeField] private float angularSpeed = 12f;

	[SerializeField] private float speed = 30f;
	[SerializeField] private int maxArrowCount = 5;

	[SerializeField] private float recallPostDelay;
	private float recallDelayTimer;
	private bool isReadyToShot = true;
	#endregion

	#region PublicMethod
	public void Initialize()
	{
		transform.Find("Renderer").TryGetComponent(out sr);
		transform.Find("Renderer").TryGetComponent(out anim);
		transform.parent.Find("Gauge").TryGetComponent(out recallGauge);
		recallGauge.Initialize();
	}
	public void Look(Vector2 mousePosition)
	{
		Vector2 direction = (mousePosition - (Vector2)transform.position).normalized;
		Quaternion targetRot = Quaternion.FromToRotation(Vector3.up, direction);
		transform.rotation = Quaternion.Euler(0, 0, Mathf.LerpAngle(transform.rotation.eulerAngles.z, targetRot.eulerAngles.z, angularSpeed * Time.unscaledDeltaTime));
		sr.sortingOrder = GetSortingOrderByAngle();
		sr.flipX = GetFlipXByAngle();
	}
	public bool CheckForExtraArrows()
	{
		return maxArrowCount > arrowList.Count;
	}
	public void Fire()
	{
		if (isReadyToShot == false)
			return;

		anim.SetTrigger("shot");
		Arrow current = Instantiate(arrowPrefab).GetComponent<Arrow>();
		arrowList.Add(current);
		current.Initialize();
		current.transform.position = transform.position;
		current.SetSpeed(speed);
		current.SetDirection(transform.eulerAngles);
		current.Shot();
	}
	public void Recall()
	{
		if (arrowList.Count == 0)
			return;

		for(int i = 0; i < arrowList.Count; ++i)
		{
			arrowList[i].Recall();
		}
		arrowList.Clear();
		StartRecallDelay();
	}
	public void SetRendererVisibility(bool b)
	{
		sr.enabled = b;
	}
	public void StartFlickering(float _time)
	{
		sr.material.EnableKeyword("FLICKER_ON");
		Invoke(nameof(EndFlickering), _time);
	}
	public void ForceQuit()
	{
		for (int i = 0; i < arrowList.Count; ++i)
		{
			arrowList[i].DestroySelf();
		}
		arrowList.Clear();
	}
	#endregion

	#region PrivateMethod
	private void Update()
	{
		if(isReadyToShot == false)
		{
			recallDelayTimer += Time.unscaledDeltaTime;
			recallGauge.UpdateValue(recallDelayTimer / recallPostDelay);
			if (recallDelayTimer > recallPostDelay)
			{
				isReadyToShot = true;
				recallGauge.gameObject.SetActive(false);
			}
		}
	}
	private int GetSortingOrderByAngle()
	{
		return transform.rotation.eulerAngles.z > 90 && transform.rotation.eulerAngles.z < 270 ? 1 : -1;
	}
	private bool GetFlipXByAngle()
	{
		return transform.rotation.eulerAngles.z > 180 && transform.rotation.eulerAngles.z < 360 ? true : false;
	}
	private void EndFlickering()
	{
		sr.material.DisableKeyword("FLICKER_ON");
	}
	private void StartRecallDelay()
	{
		isReadyToShot = false;
		recallDelayTimer = 0;
		recallGauge.gameObject.SetActive(true);
	}
	#endregion
}

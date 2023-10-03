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
	[SerializeField] private GameObject arrowPrefab;
	private List<Arrow> arrowList = new List<Arrow>();
	[SerializeField] private float angularSpeed = 12f;

	[SerializeField] private float speed = 30f;
	[SerializeField] private int maxArrowCount = 5;
	#endregion

	#region PublicMethod
	public void Initialize()
	{
		transform.Find("Renderer").TryGetComponent(out sr);
		transform.Find("Renderer").TryGetComponent(out anim);
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
		for(int i = 0; i < arrowList.Count; ++i)
		{
			arrowList[i].Recall();
		}
		arrowList.Clear();
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
	#endregion

	#region PrivateMethod
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
	#endregion
}

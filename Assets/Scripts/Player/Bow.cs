using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour
{
	#region PublicVariables
	#endregion

	#region PrivateVariables
	private SpriteRenderer sr;
	[SerializeField] private GameObject normalArrowPrefab;
	private List<GameObject> arrowList = new List<GameObject>();
	[SerializeField] private GameObject warpArrowPrefab;
	[SerializeField] private float angularSpeed = 12f;
	[SerializeField] private float shotSpeed = 10f;
	#endregion

	#region PublicMethod
	public void Initialize()
	{
		transform.Find("Renderer").TryGetComponent(out sr);
	}
	public void Look(Vector2 mousePosition)
	{
		Vector2 direction = (mousePosition - (Vector2)transform.position).normalized;
		Quaternion targetRot = Quaternion.FromToRotation(Vector3.up, direction);
		transform.rotation = Quaternion.Euler(0, 0, Mathf.LerpAngle(transform.rotation.eulerAngles.z, targetRot.eulerAngles.z, angularSpeed * Time.deltaTime));
		sr.sortingOrder = GetSortingOrderByAngle();
		sr.flipX = GetFlipXByAngle();
	}
	public void FireNormalArrow()
	{
		GameObject current = InstantiateArrow();
		// 화살 방향과 벡터 결정
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
	private GameObject InstantiateArrow()
	{
		GameObject current = GetSurplusArrow();
		if (current == null)
		{
			current = Instantiate(current, transform);
			arrowList.Add(current);
		}
		else
		{
			current.SetActive(true);
		}
		return current;
	}
	private GameObject GetSurplusArrow()
	{
		for(int i = 0; i < arrowList.Count; ++i)
		{
			if (arrowList[i].activeSelf == false)
			{
				return arrowList[i];
			}
		}
		return null;
	}
	#endregion
}

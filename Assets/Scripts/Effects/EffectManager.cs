using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
	#region PublicVariables
	public static EffectManager instance;
	#endregion

	#region PrivateVariables
	[SerializeField] private GameObject recallHitEffectPrefab;
	#endregion

	#region PublicMethod
	public void InstantiateRecallHitEffect(Vector2 _position)
	{
		Instantiate(recallHitEffectPrefab, _position, Quaternion.identity, transform);
	}
	#endregion

	#region PrivateMethod
	private void Awake()
	{
		if(instance == null)
		{
			instance = this;
		}
	}
	#endregion
}

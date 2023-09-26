using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHeartContainer : MonoBehaviour
{
	#region PublicVariables
	#endregion

	#region PrivateVariables
	[SerializeField] private List<UIHeart> hearts = new List<UIHeart>();
	#endregion

	#region PublicMethod
	public void Initialize()
	{
		foreach(UIHeart heart in hearts)
		{
			heart.Initialize();
			heart.Live();
		}
	}
	public void UpdateHeartCount(int _amount)
	{
		_amount = Mathf.Clamp(_amount, 0, hearts.Count);
		for (int i = 0; i < hearts.Count; ++i)
		{
			hearts[i].Die();
		}
		for(int i = 0; i < _amount; ++i)
		{
			hearts[i].Live();
		}
	}
	#endregion

	#region PrivateMethod
	#endregion
}

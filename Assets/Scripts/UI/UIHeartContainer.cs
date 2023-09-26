using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHeartContainer : MonoBehaviour
{
	#region PublicVariables
	#endregion

	#region PrivateVariables
	[SerializeField] private List<UIHeart> hearts = new List<UIHeart>();
	private int index = 0;
	#endregion

	#region PublicMethod
	public void Initialize()
	{
		index = 0;
		foreach(UIHeart heart in hearts)
		{
			heart.Live();
		}
	}
	public void UpdateHeartCount(int _amount)
	{
		_amount = Mathf.Clamp(_amount, 0, hearts.Count);
		foreach (UIHeart heart in hearts)
		{
			heart.Die();
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

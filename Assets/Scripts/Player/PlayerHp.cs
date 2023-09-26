using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHp : PlayerStat
{
	#region PublicVariables
	#endregion

	#region PrivateVariables
	private Player main;
	#endregion

	#region PublicMethod
	public override void Initialize()
	{
		currentValue = maxValue;
		TryGetComponent(out main);
	}
	public override void Add(float _value)
	{
		base.Add(_value);
		if(currentValue == minValue)
		{
			main.Die();
		}
	}
	#endregion

	#region PrivateMethod
	#endregion
}

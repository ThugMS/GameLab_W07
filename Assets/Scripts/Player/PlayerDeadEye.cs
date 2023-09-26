using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadEye : PlayerStat
{
	#region PublicVariables
	#endregion

	#region PrivateVariables
	private UIGauge deadEyeUIGuage;
	#endregion

	#region PublicMethod
	public override void Initialize()
	{
		GameObject.Find("Canvas/UIDeadEyeGauge").TryGetComponent(out deadEyeUIGuage);
		deadEyeUIGuage.UpdateValue(currentValue / maxValue);
	}
	public override void Add(float _value)
	{
		base.Add(_value);
		deadEyeUIGuage.UpdateValue(currentValue / maxValue);
	}
	#endregion

	#region PrivateMethod
	#endregion
}

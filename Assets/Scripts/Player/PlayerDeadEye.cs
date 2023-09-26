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
		GameObject.Find("Canvas/UI DeadEye Gauge").TryGetComponent(out deadEyeUIGuage);
		deadEyeUIGuage.UpdateValue(currentValue / maxValue);
	}
	public override void ChangeValue(float _value)
	{
		base.ChangeValue(_value);
		deadEyeUIGuage.UpdateValue(currentValue / maxValue);
	}
	#endregion

	#region PrivateMethod
	#endregion
}

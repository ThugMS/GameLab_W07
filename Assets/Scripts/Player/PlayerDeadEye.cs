using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadEye : PlayerStat
{
	#region PublicVariables
	#endregion

	#region PrivateVariables
	private PlayerBulletTime bulletTime;
	private UIGauge deadEyeUIGuage;
	private float initValue = 50f;
	#endregion

	#region PublicMethod
	public override void Initialize()
	{
		GameObject.Find("Canvas/UI DeadEye Gauge").TryGetComponent(out deadEyeUIGuage);
		currentValue = initValue;
		deadEyeUIGuage.Initialize();
		deadEyeUIGuage.UpdateValue(currentValue / maxValue);
		TryGetComponent(out bulletTime);
	}
	public override void ChangeValue(float _value)
	{
		if(_value > 0)
		{
			if(bulletTime.IsCalled == false)
			{
				base.ChangeValue(_value);
			}
		}
		else
		{
			base.ChangeValue(_value);
		}
		deadEyeUIGuage.UpdateValue(currentValue / maxValue);
	}
	#endregion

	#region PrivateMethod
	#endregion
}

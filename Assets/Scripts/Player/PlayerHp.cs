using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHp : PlayerStat
{
	#region PublicVariables
	#endregion

	#region PrivateVariables
	private Player main;

	private UIVIgnette vignette;
	private UIHeartContainer hearts;
	#endregion

	#region PublicMethod
	public override void Initialize()
	{
		currentValue = maxValue;
		TryGetComponent(out main);
		GameObject.Find("Global Volume").TryGetComponent(out vignette);
		vignette.Initialize();
		GameObject.Find("Canvas/UI Heart Container").TryGetComponent(out hearts);
		hearts.Initialize();
	}
	public override void ChangeValue(float _value)
	{
		base.ChangeValue(_value);
		UpdateVignetteByHp();
		if(currentValue == minValue)
		{
			main.Die();
		}
	}
	#endregion

	#region PrivateMethod
	private void UpdateVignetteByHp()
	{
		switch(currentValue)
		{
			case 3:
				vignette.UpdateIntensity(0);
				hearts.UpdateHeartCount(3);
				break;
			case 2:
				vignette.UpdateIntensity(0.15f);
				hearts.UpdateHeartCount(2);
				break;
			case 1:
				vignette.UpdateIntensity(0.3f);
				hearts.UpdateHeartCount(1);
				break;
			default:
				vignette.UpdateIntensity(0.5f);
				hearts.UpdateHeartCount(0);
				break;
		}
	}
	#endregion
}

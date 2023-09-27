using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerStat : MonoBehaviour
{
	#region PublicVariables
	public float Value { get { return Mathf.Clamp(currentValue, minValue, maxValue); } }
	#endregion

	#region PrivateVariables
	[SerializeField] protected float minValue;
	[SerializeField] protected float maxValue;
	[SerializeField][ReadOnly] protected float currentValue;
	#endregion

	#region PublicMethod
	public abstract void Initialize();
	public virtual void ChangeValue(float _value)
	{
		currentValue = Mathf.Clamp(currentValue + _value, minValue, maxValue);
	}
	#endregion

	#region PrivateMethod
	#endregion
}

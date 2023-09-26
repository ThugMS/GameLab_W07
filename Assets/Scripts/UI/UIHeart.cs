using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHeart : MonoBehaviour
{
	#region PublicVariables
	#endregion

	#region PrivateVariables
	private Animator anim;
	#endregion

	#region PublicMethod
	public void Live()
	{
		anim.SetBool("die", false);
	}
	public void Die()
	{
		anim.SetBool("die", true);
	}
	#endregion

	#region PrivateMethod
	private void Awake()
	{
		TryGetComponent(out anim);
	}
	#endregion
}

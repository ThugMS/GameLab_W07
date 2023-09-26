using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
	#region PublicVariables
	public static CameraManager instance;
	#endregion

	#region PrivateVariables
	#endregion

	#region PublicMethod
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

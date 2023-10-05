using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
	#region PublicVariables
	public static EffectManager instance;
	public Texture2D cursurIcon;
	#endregion

	#region PrivateVariables
	[SerializeField] private GameObject recallHitEffectPrefab;
	#endregion

	#region PublicMethod
	public void InstantiateRecallHitEffect(Vector2 _position)
	{
		Instantiate(recallHitEffectPrefab, _position, Quaternion.identity, transform);
	}
	#endregion

	#region PrivateMethod
	private void Awake()
	{
		if(instance == null)
		{
			instance = this;
		}
	}

    private void Start()
    {
		Cursor.SetCursor(cursurIcon, new Vector2(cursurIcon.width /2, cursurIcon.height / 2), CursorMode.Auto);
		
    }
    #endregion
}

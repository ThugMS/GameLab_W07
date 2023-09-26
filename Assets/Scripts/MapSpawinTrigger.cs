using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSpawinTrigger : MonoBehaviour
{
    #region PublicVariables
    public Transform m_spawnPointTransform;


    public Vector2 m_spawnPos;
    #endregion

    #region PrivateVariables
    #endregion

    #region PublicMethod
    private void Start()
    {
        m_spawnPos = new Vector2(m_spawnPointTransform.position.x, m_spawnPointTransform.position.y);
    }
    #endregion

    #region PrivateMethod

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Player.instance.SetPosition(m_spawnPos);
            
        }
    }
    #endregion
}

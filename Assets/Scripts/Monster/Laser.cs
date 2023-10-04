using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    #region PublicVariables
    #endregion

    #region PrivateVariables
    private Collider2D m_collider;
    #endregion

    #region PublicMethod
    private void Start()
    {
        TryGetComponent<Collider2D>(out m_collider);
        m_collider.enabled = false;
    }

    public void OnCollider()
    {
        m_collider.enabled = true;
    }

    public void OffCollider()
    {
        m_collider.enabled = false;
    }

    #endregion

    #region PrivateMethod
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Player.instance.Hit(-1);
        }
    }
    #endregion
}

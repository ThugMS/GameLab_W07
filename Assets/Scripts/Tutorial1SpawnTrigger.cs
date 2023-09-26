using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial1SpawnTrigger : MonoBehaviour
{
    #region PublicVariables
    public Transform m_spawnTransform;
    #endregion

    #region PrivateVariables
    #endregion

    #region PublicMethod
    #endregion

    #region PrivateMethod
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            MapSpawinTrigger.instance.SetSpawnPos(m_spawnTransform);
        }
    }
    #endregion
}

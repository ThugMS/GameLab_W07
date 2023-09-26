using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    #region PublicVariables

    #endregion
    #region PrivateVariables
    #endregion
    #region PublicMethod
    #endregion
    #region PrivateMethod
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Player.instance.Hit(-1);
        }

        
    }
    #endregion
}
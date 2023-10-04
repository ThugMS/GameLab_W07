using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideShackle : MonoBehaviour
{
    #region PublicVariables
    public GameObject m_shackle;
    public GameObject m_lock;
    #endregion

    #region PrivateVariables
    #endregion

    #region PublicMethod
    public void Hide()
    {
        m_lock.SetActive(false);
        m_shackle.SetActive(false);
    }
    #endregion

    #region PrivateMethod
    #endregion
}

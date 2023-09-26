using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DefensiveBossHP : MonoBehaviour
{
    #region PublicVariables
    public Image m_HP;
    #endregion

    #region PrivateVariables
    #endregion

    #region PublicMethod
    private void Update()
    {
        float value = DefensiveBoss.instance.m_health / DefensiveBoss.instance.m_maxHealth;
        m_HP.fillAmount = value;
    }
    #endregion

    #region PrivateMethod
    #endregion
}

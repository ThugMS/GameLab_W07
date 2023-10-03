using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThugBoss : MonsterBase
{
    #region PublicVariables
    public int m_phase = 1;
    #endregion
    #region PrivateVariables
    #endregion
    #region PublicMethod


    protected override void Move()
    {
    }

    public void SelectState()
    {
        if (m_phase == 1)
        {
            //페이즈1
        }
        else if (m_phase == 2)
        {
            //페이즈2
        }
    }

    #endregion
    #region PrivateMethod
    


    #endregion
}
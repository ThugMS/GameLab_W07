using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectProjectile : MonoBehaviour
{
    #region PublicVariables
    private float m_speed = 10f;
    #endregion

    #region PrivateVariables

    #endregion

    #region PublicMethod
    private void Update()
    {
        transform.position += transform.up * m_speed * Time.deltaTime;
    }

    public void InitSetting(float _speed)
    {
        m_speed = _speed;
    }
    #endregion

    #region PrivateMethod
    #endregion
}

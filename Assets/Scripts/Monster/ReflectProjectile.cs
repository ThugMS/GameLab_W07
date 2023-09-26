using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectProjectile : MonoBehaviour
{
    #region PublicVariables
    private float m_speed = 0f;
    #endregion

    #region PrivateVariables

    #endregion

    #region PublicMethod
    private void Update()
    {
        if(m_speed != 0f)
        {
            gameObject.transform.SetParent(null);
        }

        Vector3 dir = Player.instance.transform.position - transform.position;
        float angle = Vector2.SignedAngle(Vector3.up, dir);
        Quaternion rotate = Quaternion.Euler(0, 0, angle);
        transform.rotation = rotate;

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

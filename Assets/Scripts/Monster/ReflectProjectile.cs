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
            transform.position += transform.up * m_speed * Time.deltaTime;
        }
        else
        {
            Vector3 dir = Player.instance.transform.position - transform.position;
            float angle = Vector2.SignedAngle(Vector3.up, dir);
            Quaternion rotate = Quaternion.Euler(0, 0, angle);
            transform.rotation = rotate;
        }
    }

    public void InitSetting(float _speed)
    {
        m_speed = _speed;
    }
    #endregion

    #region PrivateMethod 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision != null)
        {
            if(collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                Player.instance.Hit(-1);
                Destroy(gameObject);
            }

            if(collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
            {
                Destroy(gameObject);
            }
        }
    }
    #endregion
}

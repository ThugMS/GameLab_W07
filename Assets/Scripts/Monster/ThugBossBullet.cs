using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThugBossBullet : MonoBehaviour
{
    #region PublicVariables
    #endregion
    #region PrivateVariables
    #endregion
    #region PublicMethod
    public void Init(float _bulletSpeed, Vector3 _bossPos)
    {
        GetComponent<Rigidbody2D>().velocity = (transform.position - _bossPos) * _bulletSpeed;
    }

    #endregion
    #region PrivateMethod
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                Player.instance.Hit(-1);
                Destroy(gameObject);
            }

            if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
            {
                Destroy(gameObject);
            }
        }
    }
    

    #endregion
}
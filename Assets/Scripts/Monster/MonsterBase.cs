using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.VisualScripting;
using UnityEngine;

public abstract class MonsterBase : MonoBehaviour
{
    #region PublicVariables
    public Rigidbody2D m_rigidbody;
    public Collider2D m_collider;
    public float m_offset;

    public float m_maxHealth;
    public float m_health;
    #endregion

    #region PrivateVariables
    [Header("Stat")]
    
    [SerializeField] protected float m_damage;

    [Header("Target")]
    [SerializeField] protected GameObject m_player;
    [SerializeField] protected Vector3 m_targetDirection;
    #endregion

    #region PublicMethod
    protected virtual void Start()
    {
        TryGetComponent<Rigidbody2D>(out m_rigidbody);
        TryGetComponent<Collider2D>(out m_collider);

        m_player = Player.instance.gameObject;
    }

    protected abstract void Move();

    protected void GetTargetDirection()
    {
        m_targetDirection = (m_player.transform.position - transform.position).normalized;
    }

    /// <summary>
    /// 데미지를 줄 때 인자로 데미지를 넘겨서 사용
    /// </summary>
    public virtual void GetDamage()
    {
        m_health -= m_damage;

        if (m_health <= 0)
            Dead();
    }

    public virtual void Dead()
    {
        Destroy(gameObject);
    }

    #endregion

    #region PublicMethod
    #endregion

    #region PrivateMethod
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Player.instance.Hit(-1);
        }
    }
    #endregion
}

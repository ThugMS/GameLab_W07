using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterBase : MonoBehaviour
{
    #region PublicVariables
    public Rigidbody2D m_rigidbody;
    public Collider2D m_collider;
    #endregion

    #region PrivateVariables
    [Header("Stat")]
    [SerializeField] protected float m_health;
    [SerializeField] protected float m_power;

    [Header("Target")]
    [SerializeField] protected GameObject m_player;
    [SerializeField] protected Vector3 m_targetDirection;
    #endregion

    #region PublicMethod
    protected virtual void Start()
    {
        TryGetComponent<Rigidbody2D>(out m_rigidbody);
        TryGetComponent<Collider2D>(out m_collider);

        // TO_DO : Player 오브젝트 얻어오기
    }

    protected abstract void Move();

    protected void GetTargetDirection()
    {
        m_targetDirection = (m_player.transform.position - transform.position).normalized;
    }

    /// <summary>
    /// 데미지를 줄 때 인자로 데미지를 넘겨서 사용
    /// </summary>
    /// <param name="_damage"></param>
    public void GetDamage(float _damage)
    {
        m_health -= _damage;

        if (m_health <= 0)
            Dead();
    }

    public void Dead()
    {
        Destroy(gameObject);
    }
    #endregion

    #region PrivateMethod
    
    #endregion
}

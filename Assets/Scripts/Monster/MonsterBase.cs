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
    public SpriteRenderer m_spriteRenderer;
    #endregion

    #region PrivateVariables
    [Header("Stat")]
    
    [SerializeField] protected float m_damage;

    [Header("Target")]
    [SerializeField] protected GameObject m_player;
    [SerializeField] protected Vector3 m_targetDirection;

    private Color m_hitColor = new Color(255f / 255f, 129f / 255f, 133f / 255f);
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
        ShowHitEffect();

        if (m_health <= 0)
            Dead();
    }

    public virtual void GetDamage(float _damage)
    {
        m_health -= _damage;
        ShowHitEffect();

        if (m_health <= 0)
            Dead();
    }

    public virtual void Dead()
    {
        Destroy(gameObject);
    }

    public void ShowHitEffect()
    {
        m_spriteRenderer.color = m_hitColor;

        Invoke(nameof(ReturnColor), 0.05f);
    }

    public void ReturnColor()
    {
        m_spriteRenderer.color = Color.white;
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

using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class Slime : MonsterBase
{
    #region PublicVariables
    private int m_moveLayerMask;
    #endregion

    #region PrivateVariables
    [Header("Move")]
    [SerializeField] private float m_moveDis;
    [SerializeField] private bool m_canMove = true;
    [SerializeField] private float m_moveTime = 1.0f;
    [SerializeField] private float m_moveCooldown;
    [SerializeField] private AnimationCurve m_moveEase;
    [SerializeField] private Tween m_moveTween;
    [SerializeField] private bool m_canStart = false;
    [SerializeField] private float m_startDelayTime;


    #endregion
    #region PublicMethod
    protected override void Start()
    {
        base.Start();

        m_moveLayerMask = LayerMask.GetMask("Monster", "Wall");
        m_startDelayTime = Random.Range(0.5f, 1.0f);
        
    }

    private void Update()
    {
        if (m_canStart == true)
        {
            CheckMove();
        }
        else
        {
            m_startDelayTime -= Time.deltaTime;

            if (m_startDelayTime <= 0)
                m_canStart = true;
        }
    }

    protected override void Move()
    {   
        GetTargetDirection();
        Vector3 movePos;
        
        if(m_moveDis > (m_player.transform.position - transform.position).magnitude)
           movePos = m_player.transform.position;

        else
            movePos = transform.position + m_targetDirection * m_moveDis;

        m_moveTween  = transform.DOMove(movePos, m_moveTime).SetEase(m_moveEase);
    }
    #endregion

    #region PrivateMethod
    /// <summary>
    /// 움직일 수 있는지 여부를 체크하고 움직일 수 있다면 움직이게 합니다.
    /// </summary>
    private void CheckMove()
    {
        if (m_canMove)
        {
            Move();
            m_moveCooldown = m_moveTime;
            m_canMove = false;
        }
        else
        {
            m_moveCooldown -= Time.deltaTime;

            if (m_moveCooldown <= 0)
                m_canMove = true;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        GetTargetDirection();
        float dis = (m_player.transform.position - transform.position).magnitude;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, m_targetDirection, dis, m_moveLayerMask);
        Debug.DrawRay(transform.position, m_targetDirection, Color.green);

        if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            m_moveTween.Kill();
        }
        else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {
            
        }
    }

    #endregion
}

using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class Slime : MonsterBase
{
    #region PublicVariables
    #endregion

    #region PrivateVariables
    [Header("Move")]
    [SerializeField] private float m_moveDis;
    [SerializeField] private bool m_canMove = true;
    [SerializeField] private float m_moveTime = 1.0f;
    [SerializeField] private float m_moveCooldown;
    [SerializeField] private AnimationCurve m_moveEase;

    #endregion

    #region PublicMethod
    private void Update()
    {
        CheckMove();
    }

    protected override void Move()
    {   
        GetTargetDirection();
        Vector3 movePos;
        
        if(m_moveDis > (m_player.transform.position - transform.position).magnitude)
           movePos = m_player.transform.position;

        else
            movePos = transform.position + m_targetDirection * m_moveDis;

        transform.DOMove(movePos, m_moveTime).SetEase(m_moveEase);
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
    #endregion
}

using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefensiveBoss : MonsterBase
{
    #region PublicVariables
    public Transform m_shiledRotation;
    #endregion

    #region PrivateVariables
    [Header("Shiled")]
    [SerializeField] private float m_turnSpeed;

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
        RotateShiled();
        CheckMove();
    }

    protected override void Move()
    {
        GetTargetDirection();
        Vector3 movePos;

        if (m_moveDis > (m_player.transform.position - transform.position).magnitude)
            movePos = m_player.transform.position;

        else
            movePos = transform.position + m_targetDirection * m_moveDis;

        transform.DOMove(movePos, m_moveTime - 0.3f).SetEase(m_moveEase);
    }
    #endregion

    #region PrivateMethod
    /// <summary>
    /// 플레이어 방향으로 실드 돌려준다.
    /// </summary>
    private void RotateShiled()
    {
        GetTargetDirection();
        float angle = Vector2.SignedAngle(Vector3.up, m_targetDirection);

        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);

        m_shiledRotation.rotation = Quaternion.Lerp(m_shiledRotation.rotation, targetRotation, m_turnSpeed);
    }

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

    private void Charge()
    {

    }
    #endregion
}

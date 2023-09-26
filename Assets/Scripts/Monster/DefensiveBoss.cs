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
    [SerializeField] private Transform m_shieldRotate;
    [SerializeField] private SpriteRenderer m_sr;

    [Header("Move")]
    [SerializeField] private float m_moveDis;
    [SerializeField] private bool m_canMove = true;
    [SerializeField] private float m_moveTime = 1.0f;
    [SerializeField] private float m_moveCooldown;
    [SerializeField] private AnimationCurve m_moveEase;

    [Header("Charge")]
    [SerializeField] private float dis;
    [SerializeField] private AnimationCurve m_chargeEase;
    private int m_chargeLayer;
    #endregion

    #region PublicMethod
    protected virtual void Start()
    {
        base.Start();

        m_chargeLayer = LayerMask.GetMask("Wall");
    }

    private void Update()
    {
        RotateShiled();
        //CheckMove();
        m_sr.sortingOrder = GetSortingOrderByAngle();

        if (Input.GetKeyDown(KeyCode.Space))
            Charge();
            
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
        GetTargetDirection();

        RaycastHit2D hit = Physics2D.Raycast(transform.position, m_targetDirection, 2000f, m_chargeLayer);

        if(hit == true)
        {
            Vector3 pos = hit.point - new Vector2((m_targetDirection * m_offset).x, (m_targetDirection * m_offset).y);
            transform.DOMove(pos, 0.2f).SetEase(m_chargeEase);
        }
    }

    private int GetSortingOrderByAngle()
    {
        return m_shieldRotate.rotation.eulerAngles.z > 90 && m_shieldRotate.rotation.eulerAngles.z < 270 ? 1 : -1;
    }
    #endregion
}

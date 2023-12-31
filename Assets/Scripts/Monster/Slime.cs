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
    [SerializeField] private float m_delayTime;
    [SerializeField] private float m_moveCooldown;
    [SerializeField] private AnimationCurve m_moveEase;
    [SerializeField] private Tween m_moveTween;
    [SerializeField] private bool m_canStart = false;
    [SerializeField] private float m_startDelayTime;

    [Header("RayCheck")]
    public Vector3 m_movePos;
    [SerializeField] private bool m_isWall = false;

    [Header("Animation")]
    [SerializeField] private Animator m_animator;


    #endregion

    #region PublicMethod
    protected override void Start()
    {
        base.Start();

        m_moveLayerMask = LayerMask.GetMask("Spike", "Wall");
        m_startDelayTime = Random.Range(0.5f, 1.0f);
        TryGetComponent<Animator>(out m_animator);
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
        
        if(m_moveDis > (m_player.transform.position - transform.position).magnitude)
           m_movePos = m_player.transform.position;

        else
            m_movePos = transform.position + m_targetDirection * m_moveDis;

        CheckMonster();
        CheckWall();
        m_animator.SetTrigger("Move");
        transform.localScale = m_targetDirection.x < 0 ? new Vector3(-1,1,1) : new Vector3(1, 1, 1);

        if(m_isWall==false)
            m_moveTween  = transform.DOMove(m_movePos, m_moveTime).SetEase(m_moveEase);
        
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
            m_moveCooldown = Random.Range(1f, 1.5f);
            m_canMove = false;
        }
        else
        {
            m_moveCooldown -= Time.deltaTime;

            if (m_moveCooldown <= 0)
                m_canMove = true;
        }
    }

    //private void OnCollisionStay2D(Collision2D collision)
    //{
    //    GetTargetDirection();
    //    float dis = (m_movePos - transform.position).magnitude;

    //    RaycastHit2D hit = Physics2D.Raycast(transform.position, m_targetDirection, dis, LayerMask.GetMask("Wall"));
    //    Debug.DrawRay(transform.position, m_targetDirection, Color.green);
    //    Debug.Log("is wall1");
    //    if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Wall"))
    //    {
    //        m_canMove = false;
    //        m_moveTween.Kill();
    //    }

    //}

    private void CheckWall()
    {
        //GetTargetDirection();
        float dis = (m_movePos - transform.position).magnitude;
        m_targetDirection = m_movePos - transform.position;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, m_targetDirection, dis, m_moveLayerMask);
        Debug.DrawRay(transform.position, m_targetDirection, Color.green);
        if (hit.transform != null)
        {
            m_isWall = true;
        }
        else
        {
            m_isWall = false;
        }
    }

    private void CheckMonster()
    {
        float closestDistance = 10f;
        Vector3 boxCenter = transform.position; // 박스의 중심 위치
        Vector2 boxSize = new Vector2(2, 2);

        Collider2D[] colliders = Physics2D.OverlapBoxAll(boxCenter, boxSize, 0f, LayerMask.GetMask("Monster"));
        Collider2D curCollider = null;

        if( colliders.Length >0 )
        {
            foreach (Collider2D collider in colliders)
            {
                if(collider.gameObject != this.gameObject)
                {
                    float distance = Vector3.Distance(transform.position, collider.transform.position);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        curCollider = collider;
                    }
                }
            }

            if(curCollider != null)
            {
                Vector3 newDirection = m_movePos - ((this.transform.position + curCollider.transform.position) / 2);
                Vector3 newMovePos = newDirection + this.transform.position;
                m_movePos = newMovePos;
            }
        }
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireCube(transform.position, new Vector2(1.5f,1.5f));
    //}
}
    #endregion

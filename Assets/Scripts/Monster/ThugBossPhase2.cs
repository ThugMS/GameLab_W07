using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static UnityEngine.GraphicsBuffer;
using System.Security.Cryptography;

public class ThugBossPhase2 : MonoBehaviour
{
    #region PublicVariables
    [Header("Stat")]
    public bool m_isAttack = false;
    public bool m_canAttack = false;
    public bool m_isMove = false;
    public bool m_canMove = true;
    public float m_attackDelay;

    public GameObject m_bulletParents;
    public Animator m_animator;
    public SpriteRenderer m_body;
    #endregion
   
    #region PrivateVariables
    [Header("TakeDown")]
    [SerializeField] private float m_jumpReadyTime = 1f;
    [SerializeField] private GameObject m_jumpShadowPrefab;
    [SerializeField] private GameObject m_jumpEffect;
    [SerializeField] private GameObject m_takeDownEffect;
    [SerializeField] private GameObject m_jumpShadow;
    [SerializeField] private float m_jumpShadowSpeed = 1f;
    [SerializeField] private bool m_isShadow = false;
    [SerializeField] private float m_jumpTime;
    [SerializeField] private Ease m_jumpEase;
    [SerializeField] private Tweener m_takeDownTweener;

    [Header("Wave")]
    [SerializeField] private GameObject m_rock;
    [SerializeField] private float m_rockNum;

    [Header("Laser")]
    [SerializeField] private GameObject m_laser;
    [SerializeField] private float m_laserSpeed;
    [SerializeField] private int m_laserNum;

    [Header("Move")]
    [SerializeField] private float m_moveDis;
    [SerializeField] private float m_moveTime;
    [SerializeField] private AnimationCurve m_moveEase;
    #endregion

    #region PublicMethod
    private void Start()
    {
        m_bulletParents = GameObject.Find("Bullets");

    }
    private void Update()
    {
        //Test start
        if (Input.GetKeyDown(KeyCode.F1))
            JumpAttack();

        if (Input.GetKeyDown(KeyCode.F2))
            Laser();
        //Test end

        if(m_isShadow == true)
        {
            TrackingShadow();
        }

        if (m_isMove == false && m_canAttack == true)
        {
            Attack();
        }

        else if (m_canMove == true && m_isAttack == false)
        {
            Move();
        }
    }

    private void OnEnable()
    {
        Invoke(nameof(AttackDelay), m_attackDelay);
    }
    #endregion

    #region PrivateMethod
    private void Attack()
    {
        m_isAttack = true;
        m_canAttack = false;

        int value = Random.Range(0, 2);

        switch (value) 
        {
            case 0:
                JumpAttack();
                break;

            case 1:
                Laser();
                break;
        }
    }

    private void JumpAttack()
    {
        StartCoroutine(nameof(IE_Jump));
    }

    private void MakeShadow()
    {
        m_jumpShadow = Instantiate(m_jumpShadowPrefab, transform.position, Quaternion.identity);
        m_isShadow = true;

        Invoke(nameof(TakeDown), m_jumpTime);
    }

    private void TrackingShadow()
    {
        //StartCoroutine(IE_MoveShadow(Player.instance.transform.position));

        Vector3 dir = Player.instance.transform.position - m_jumpShadow.transform.position;

        m_jumpShadow.transform.position += dir * m_jumpShadowSpeed * Time.deltaTime;
    }

    private void RemoveShadow()
    {
        Destroy(m_jumpShadow);
    }

    private void TakeDown()
    {
        transform.position = new Vector3(m_jumpShadow.transform.position.x, transform.position.y, 0);
        m_isShadow = false;
        StopAllCoroutines();

        StartCoroutine(nameof(IE_TakeDown));
    }

    private void Laser()
    {
        StartCoroutine(nameof(IE_Laser));
    }

    private void Move()
    {   
        m_isMove = true;
        m_canMove = false;

        Vector3 target = (Player.instance.transform.position - transform.position).normalized;

        Vector3 movePos;

        if (m_moveDis > (Player.instance.transform.position - transform.position).magnitude)
            movePos = Player.instance.transform.position;

        else
            movePos = transform.position + target * m_moveDis;

        m_animator.Play("Move");

        Debug.Log(target);
        Debug.Log(movePos);
        transform.DOMove(movePos, m_moveTime - 0.3f).SetEase(m_moveEase);
        Invoke(nameof(MoveEnd), m_moveTime);
    }

    private void CreateWave()
    {
        float term = 360 / m_rockNum;

        for(int i = 0; i < m_rockNum; i++)
        {
            float angle = 0f + term*i;
            
            Vector3 dir = (transform.position + new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0f)) - transform.position;

            GameObject rock = Instantiate(m_rock, transform.position, Quaternion.identity, m_bulletParents.transform);
            rock.GetComponent<Rock>().InitSetting(dir.normalized, false); 
        }
    }

    private void MoveEnd()
    {
        m_isMove = false;
        m_canMove = true;
    }

    private void AttackEnd()
    {
        m_isAttack = false;
        Invoke(nameof(AttackDelay), m_attackDelay);
    }

    private void AttackDelay()
    {
        m_canAttack = true;
    }

    private void SetLayer()
    {
        if(transform.position.y > Player.instance.transform.position.y)
        {
            m_body.sortingLayerID = 3;
        }
        else
        {
            m_body.sortingLayerID = -3;
        }
    }

    private IEnumerator IE_Jump()
    {
        //m_takeDownTweener = transform.DOShakePosition(m_jumpReadyTime, 0.3f);
        m_animator.Play("JumpReady");

        //yield return m_takeDownTweener.WaitForCompletion();
        yield return new WaitForSeconds(m_jumpReadyTime);

        transform.DOMoveY(20f, 1f).SetEase(m_jumpEase);
        Instantiate(m_jumpEffect, transform.position, Quaternion.identity);
        MakeShadow();
    }

    private IEnumerator IE_TakeDown()
    {
        m_takeDownTweener = transform.DOMove(m_jumpShadow.transform.position, 0.7f).SetEase(Ease.InExpo);

        yield return m_takeDownTweener.WaitForCompletion();

        Destroy(m_jumpShadow);
        m_animator.Play("TakeDown");
        Instantiate(m_takeDownEffect, transform.position, Quaternion.identity);
        CreateWave();
        Invoke(nameof(AttackEnd), 1f);
    }

    private IEnumerator IE_Laser()
    {
        

        for(int i = 0; i < m_laserNum; i++)
        {
            float angle = Random.Range(0, 360);
            Instantiate(m_laser, Player.instance.transform.position, Quaternion.Euler(0, 0, angle));

            yield return new WaitForSeconds(m_laserSpeed);
        }
        AttackEnd();
    }
    #endregion
}

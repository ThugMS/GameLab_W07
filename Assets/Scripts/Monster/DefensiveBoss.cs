using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefensiveBoss : MonsterBase
{
    public static DefensiveBoss instance;

    #region PublicVariables
    public Transform m_shiledRotation;
    public bool m_isShieldOpen = false;

    [Header("Pattern")]
    public bool m_isAct = false;
    public bool m_canAct = true;
    public int m_phase = 1;
    public int m_pattern;
    public float m_actTerm = 5f;
    #endregion

    #region PrivateVariables
    [Header("Shiled")]
    [SerializeField] private float m_turnSpeed;
    [SerializeField] private Transform m_shieldRotate;
    [SerializeField] private SpriteRenderer m_sr1;
    [SerializeField] private SpriteRenderer m_sr2;
    [SerializeField] private float m_shieldOpenOffset = 0.5f;
    [SerializeField] private float m_shieldOpenTime = 0.2f;

    [Header("Move")]
    [SerializeField] private float m_moveDis;
    [SerializeField] private bool m_canMove = true;
    [SerializeField] private float m_moveTime = 1.0f;
    [SerializeField] private float m_moveCooldown;
    [SerializeField] private AnimationCurve m_moveEase;

    [Header("Charge")]
    [SerializeField] private float dis;
    [SerializeField] private AnimationCurve m_chargeEase;
    [SerializeField] private float m_chargeTime = 0.5f;
    private int m_chargeLayer;

    [Header("Fire")]
    [SerializeField] GameObject m_fire;
    [SerializeField] private float m_fireTime = 5.0f;

    [Header("Shadow")]
    [SerializeField] private GameObject m_shadow;
    [SerializeField] private Vector3 m_returnPos;
    [SerializeField] private float m_shadowTime;
    [SerializeField] private float m_shadowCooldown;

    [Header("Reflect")]
    [SerializeField] private GameObject m_reflectRegion;
    [SerializeField] private float m_reflectTime = 3f;
    #endregion

    #region PublicMethod
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }    
    }

    public void SelectState()
    {
        if (m_canAct == false)
            return;

        m_canAct = false;
        m_isAct = true;

        if(m_phase == 1)
        {
            CheckPhase1Pattern();
        }
        else if (m_phase == 2)
        {
            CheckPhase2Pattern();
        }
    }

    protected override void Start()
    {
        base.Start();

        m_chargeLayer = LayerMask.GetMask("Wall");
        BossHpGUI.instance.ShowGUI();
        BossHpGUI.instance.SetMaxHp((int) m_maxHealth);
        SetHPGUI();
    }

    private void Update()
    {
        RotateShiled();
        CheckMove();
        SelectState();
        ChangePhase();

        m_sr1.sortingOrder = GetSortingOrderByAngle();
        m_sr2.sortingOrder = GetSortingOrderByAngle();



        ////Test
        //if (Input.GetKeyDown(KeyCode.Space))
        //    Charge();

        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //    Fire();

        //if (Input.GetKeyDown(KeyCode.Alpha2))
        //    MakeShadow();


        if (Input.GetKeyDown(KeyCode.Alpha4))
            ReflectProjectile();
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

    public void EndAct()
    {
        m_isAct = false;
        Invoke(nameof(ReadyAct), m_actTerm);
    }

    public void ReadyAct()
    {
        m_canAct = true;
    }

    public override void GetDamage()
    {
        base.GetDamage();

        SetHPGUI();
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
            if (m_isAct == true)
                return;

            m_moveCooldown -= Time.deltaTime;

            if (m_moveCooldown <= 0)
                m_canMove = true;
        }
    }
    /// <summary>
    /// 방패를 열고 불을 쏨. 불을 쏜 후 방패를 닫음.
    /// </summary>
    private void Fire()
    {
        ChangeShieldPosition();
        StartCoroutine(IE_SetActiveFireByTime(true, m_shieldOpenTime));
        StartCoroutine(IE_SetActiveFireByTime(false, m_fireTime));

        Invoke(nameof(ChangeShieldPosition), m_fireTime);
        Invoke(nameof(EndAct), m_fireTime + 1f);
    }
    /// <summary>
    /// 돌진형 기믹
    /// </summary>
    private void Charge()
    {
        GetTargetDirection();

        RaycastHit2D hit = Physics2D.Raycast(transform.position, m_targetDirection, 2000f, m_chargeLayer);

        if(hit == true)
        {
            Vector3 pos = hit.point - new Vector2((m_targetDirection * m_offset).x, (m_targetDirection * m_offset).y);
            transform.DOMove(pos, m_chargeTime).SetEase(m_chargeEase);
        }

        Invoke(nameof(EndAct), m_chargeTime + 1f);
    }
    /// <summary>
    /// 그림자 만드는 기믹
    /// </summary>
    private void MakeShadow()
    {
        GameObject shadow = Instantiate(m_shadow, Player.instance.transform.position, Quaternion.identity);
        shadow.GetComponent<Shadow>().InitSetting(m_shadowTime);

        Invoke(nameof(EndAct), 1f);
    }

    private void ReflectProjectile()
    {
        m_reflectRegion.GetComponent<Reflect>().InitSetting(m_reflectTime);

        Invoke(nameof(EndAct), m_reflectTime + 1f);
    }

    /// <summary>
    /// 각도에 따라서 방패의 layer 변경
    /// </summary>
    /// <returns></returns>
    private int GetSortingOrderByAngle()
    {
        return m_shieldRotate.rotation.eulerAngles.z > 90 && m_shieldRotate.rotation.eulerAngles.z < 270 ? 1 : -1;
    }
    /// <summary>
    /// 방패 열고 닫는 과정
    /// </summary>
    private void ChangeShieldPosition()
    {
        int moveSign = 1;

        if (m_isShieldOpen == true)
        {
            moveSign *= -1;
        }

        m_isShieldOpen = !m_isShieldOpen;

        m_sr1.transform.DOLocalMoveX(m_sr1.transform.localPosition.x - moveSign * m_shieldOpenOffset, m_shieldOpenTime).SetEase(Ease.Linear);
        m_sr2.transform.DOLocalMoveX(m_sr2.transform.localPosition.x + moveSign * m_shieldOpenOffset, m_shieldOpenTime).SetEase(Ease.Linear);
    }

    private void ChangePhase()
    {
        if (m_health <= m_maxHealth / 2 && m_phase == 1)
            m_phase = 2;
    }

    private void CheckPhase1Pattern()
    {
        float dis = Vector3.Distance(transform.position, Player.instance.transform.position);
        
        m_pattern = Random.Range(0, 10);

        if(dis > 15)
        {
            Charge();
        }
        else
        {
            if(m_pattern > 7)
            {
                Charge();
            }
            else
            {
                Fire();
            }
        }
    }

    private void CheckPhase2Pattern()
    {
        m_pattern = Random.Range(0, 3);

        switch (m_pattern)
        {
            case 0:
                Charge();
                break;
            case 1:
                MakeShadow();
                break;
            case 2:
                ReflectProjectile();
                break;
        }
    }

    private void SetHPGUI()
    {
        BossHpGUI.instance.SetHp((int)m_health);
    }

    private IEnumerator IE_SetActiveFireByTime(bool _value, float _time)
    {
        yield return new WaitForSeconds(_time);

        m_fire.SetActive(_value);
    }
    #endregion
}

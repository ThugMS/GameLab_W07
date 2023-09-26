using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefensiveBoss : MonsterBase
{
    #region PublicVariables
    public Transform m_shiledRotation;
    public bool m_isShieldOpen = false;
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
    private int m_chargeLayer;

    [Header("Fire")]
    [SerializeField] GameObject m_fire;
    [SerializeField] private float m_fireTime = 5.0f;

    [Header("Shadow")]
    [SerializeField] private GameObject m_shadow;
    [SerializeField] private Vector3 m_returnPos;
    [SerializeField] private float m_shadowTime;
    [SerializeField] private float m_shadowCooldown;
    #endregion

    #region PublicMethod
    protected override void Start()
    {
        base.Start();

        m_chargeLayer = LayerMask.GetMask("Wall");
    }

    private void Update()
    {
        RotateShiled();
        //CheckMove();
        m_sr1.sortingOrder = GetSortingOrderByAngle();
        m_sr2.sortingOrder = GetSortingOrderByAngle();

        //Test
        if (Input.GetKeyDown(KeyCode.Space))
            Charge();

        if (Input.GetKeyDown(KeyCode.Alpha1))
            Fire();

        if (Input.GetKeyDown(KeyCode.Alpha2))
            MakeShadow();
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
    /// <summary>
    /// 방패를 열고 불을 쏨. 불을 쏜 후 방패를 닫음.
    /// </summary>
    private void Fire()
    {
        ChangeShieldPosition();
        StartCoroutine(IE_SetActiveFireByTime(true, m_shieldOpenTime));
        StartCoroutine(IE_SetActiveFireByTime(false, m_fireTime));
        Invoke(nameof(ChangeShieldPosition), m_fireTime);
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
            transform.DOMove(pos, 0.2f).SetEase(m_chargeEase);
        }
    }

    private void MakeShadow()
    {
        GameObject shadow = Instantiate(m_shadow, Player.instance.transform.position, Quaternion.identity);
        shadow.GetComponent<Shadow>().InitSetting(m_shadowTime);
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

    private IEnumerator IE_SetActiveFireByTime(bool _value, float _time)
    {
        yield return new WaitForSeconds(_time);

        m_fire.SetActive(_value);
    }
    #endregion
}

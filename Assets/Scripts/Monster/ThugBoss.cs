using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ThugBoss : MonsterBase
{
    public static ThugBoss instance;
    #region PublicVariables
    [Header("Pattern")]
    public bool m_isAct = false;
    public bool m_canAct = true;
    public int m_phase = 1;
    public int m_pattern;
    public float m_actTerm = 3f;
    public Animator animator;
    #endregion
    #region PrivateVariables
    private ThugBossPhase1 thugBossPhase1;
    private ThugBossPhase2 thugBossPhase2;

    [Header("Animation")]
    [SerializeField] private Animator m_bodyAni;
    [SerializeField] private Animator m_shackleLeftAni;
    [SerializeField] private Animator m_shackleRightAni;
    #endregion


    #region PublicMethod

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    protected override void Start()
    {
        //test
        InitSetting();

        //페이즈1
        thugBossPhase1.enabled = true;
        thugBossPhase2.enabled = false;

        BossHpGUI.instance.ShowGUI();
        BossHpGUI.instance.SetMaxHp((int)m_maxHealth);
        SetHPGUI();
    }
    public void InitSetting()
    {
        TryGetComponent<ThugBossPhase1>(out thugBossPhase1);
        TryGetComponent<ThugBossPhase2>(out thugBossPhase2);
        //페이즈1
        thugBossPhase1.enabled = true;
        thugBossPhase2.enabled = false;

        instance = this;

        m_health = m_maxHealth;
        SetHPGUI();
        m_canAct = false;
        Invoke(nameof(ReadyAct), 4.0f);
        m_isAct = false;
        m_phase = 1;
    }

    private void Update()
    {

        SelectState();
    }

    protected override void Move()
    {
    }

    public void SelectState()
    {
        if (m_canAct == false)
            return;

        m_canAct = false;
        m_isAct = true;

        if (m_phase == 1)
        {
            animator.Play("ShotReady");
            Invoke(nameof(CheckPhase1Pattern),1);

        }
    }

    private void CheckPhase1Pattern()
    {
        m_pattern = Random.Range(0, 2);
        
        switch (m_pattern)
        {
            
            case 0:
                thugBossPhase1.m_isCircleBulletOn = true;
                break;
            case 1:
                thugBossPhase1.m_isZigZagBulletOn = true;
                break;
        }
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

    public override void GetDamage(float _damage)
    {
        base.GetDamage(_damage);

        SetHPGUI();
    }

    #endregion
    #region PrivateMethod
    private void SetHPGUI()
    {
        BossHpGUI.instance.SetHp((int)m_health);
        if (m_health <= m_maxHealth / 2 && m_phase == 1)
        {
            m_phase = 2;
            thugBossPhase1.StopAllCoroutines();
            //페이즈2
            thugBossPhase1.enabled = false;
            ShowUnlockShackleAnimation();
            Invoke(nameof(StartPhase2), 1f);
            
        }
            
    }

    private void ShowUnlockShackleAnimation()
    {
        m_bodyAni.Play("UnlockShackle");
        m_shackleLeftAni.Play("Shackle");
        m_shackleRightAni.Play("Shackle");
    }

    private void StartPhase2()
    {
        thugBossPhase2.enabled = true;
    }

    private void OnEnable()
    {
        BossHpGUI.instance?.ShowGUI();
    }

    private void OnDisable()
    {
        BossHpGUI.instance?.HideGUI();
    }
    public override void Dead()
    {
        base.Dead();

        SceneManager.LoadScene(1);
    }

    public void CleanBullet()
    {
        Transform[] children = thugBossPhase1.bulletParents.transform.GetComponentsInChildren<Transform>();

        foreach (Transform child in children)
        {
            if (child != null && child != thugBossPhase1.bulletParents.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }
    #endregion
}
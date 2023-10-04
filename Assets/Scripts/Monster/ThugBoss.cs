using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThugBoss : MonsterBase
{
    public static ThugBoss instance;
    #region PublicVariables
    public int m_phase = 1;
    #endregion
    #region PrivateVariables
    private ThugBossPhase1 thugBossPhase1;
    private ThugBossPhase2 thugBossPhase2;
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
        
        TryGetComponent<ThugBossPhase1>(out thugBossPhase1);
        TryGetComponent<ThugBossPhase2>(out thugBossPhase2);

        BossHpGUI.instance.ShowGUI();
        BossHpGUI.instance.SetMaxHp((int)m_maxHealth);
        SetHPGUI();
    }
    public void InitSetting()
    {
        m_health = m_maxHealth;
        SetHPGUI();

        transform.rotation = Quaternion.Euler(0, 0, 180);
        StopAllCoroutines();
        m_phase = 1;
    }

    protected override void Move()
    {
    }

    public void SelectState()
    {
        if (m_phase == 1)
        {
            //페이즈1
            thugBossPhase1.enabled = true;
            thugBossPhase2.enabled = false;

        }
        else if (m_phase == 2)
        {
            //페이즈2
            thugBossPhase1.enabled = false;
            thugBossPhase2.enabled = true;
        }
    }

    public override void GetDamage()
    {
        base.GetDamage();

        SetHPGUI();
    }

    #endregion
    #region PrivateMethod
    private void SetHPGUI()
    {
        BossHpGUI.instance.SetHp((int)m_health);
    }

    private void OnEnable()
    {

        BossHpGUI.instance?.ShowGUI();
    }

    private void OnDisable()
    {
        BossHpGUI.instance?.HideGUI();
    }
    #endregion
}
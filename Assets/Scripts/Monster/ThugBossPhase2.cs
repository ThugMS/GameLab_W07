using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class ThugBossPhase2 : MonoBehaviour
{
    #region PublicVariables
    [Header("Stat")]
    public bool m_isAttack = false;
    public bool m_canMove = false;
    public float m_attackDelay;

    public Animator m_animator;
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
    #endregion

    #region PublicMethod
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

    }
    #endregion

    #region PrivateMethod
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

    private void CreateWave()
    {
        float term = 360 / m_rockNum;

        for(int i = 0; i < m_rockNum; i++)
        {
            float angle = 0f + term*i;
            
            Vector3 dir = (transform.position + new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0f)) - transform.position;

            GameObject rock = Instantiate(m_rock, transform.position, Quaternion.identity);
            rock.GetComponent<Rock>().InitSetting(dir.normalized, false);
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
    }

    private IEnumerator IE_Laser()
    {
        for(int i = 0; i < m_laserNum; i++)
        {
            float angle = Random.Range(0, 360);
            Instantiate(m_laser, Player.instance.transform.position, Quaternion.Euler(0, 0, angle));

            yield return new WaitForSeconds(m_laserSpeed);
        }
    }
    #endregion
}

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
    [SerializeField] private GameObject m_jumpShadow;
    [SerializeField] private float m_jumpShadowDelay = 0.2f;
    [SerializeField] private bool m_isShadow = false;
    [SerializeField] private float m_jumpTime;
    [SerializeField] private Ease m_jumpEase;
    [SerializeField] private Tweener m_takeDownTweener;

    [Header("Wave")]
    [SerializeField] private GameObject m_rock;
    [SerializeField] private float m_rockNum;
    #endregion

    #region PublicMethod
    private void Update()
    {
        //Test start
        if (Input.GetKeyDown(KeyCode.F1))
            JumpAttack();
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
        m_jumpShadow = Instantiate(m_jumpShadowPrefab, Player.instance.transform.position, Quaternion.identity);
        m_isShadow = true;

        Invoke(nameof(TakeDown), m_jumpTime);
    }

    private void TrackingShadow()
    {
        StartCoroutine(IE_MoveShadow(Player.instance.transform.position));
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

        transform.DOMove(m_jumpShadow.transform.position, 0.7f);
    }

    private void CreateWave()
    {

    }

    private IEnumerator IE_Jump()
    {
        //m_takeDownTweener = transform.DOShakePosition(m_jumpReadyTime, 0.3f);
        m_animator.Play("JumpReady");

        //yield return m_takeDownTweener.WaitForCompletion();
        yield return new WaitForSeconds(m_jumpReadyTime);

        transform.DOMoveY(50f, 1f).SetEase(m_jumpEase);
        Instantiate(m_jumpEffect, transform.position, Quaternion.identity);
        MakeShadow();
    }

    private IEnumerator IE_MoveShadow(Vector3 _pos)
    {
        yield return new WaitForSeconds(m_jumpShadowDelay);

        m_jumpShadow.transform.position = _pos;
    }
    #endregion
}

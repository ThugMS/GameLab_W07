using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThugBossPhase1 : MonoBehaviour
{
    #region PublicVariables
    public GameObject bullet;
    [HideInInspector]public GameObject bulletParents;
    #endregion
    #region PrivateVariables
    [Header("CircleBullet")]
    [SerializeField] private float m_circleBulletfireCoolTime = 0.2f; // 탄막 층 생성 간격
    [SerializeField] private int m_circleBulletBranch; // 탄막 갈래
    [SerializeField] private float m_circleBulletSpawnRadius; // 탄막 생성 반지름
    [SerializeField] private float m_circleBulletSpeed; //탄막 속도
    [SerializeField] private float m_circleBulletRotationVelocity; // 탄막 중심 회전 속도
    [SerializeField] private int m_circleBulletSpawnDirection; // 탄막 중심 회전 방향 (시계방향: 1, 시계반대방향: -1)
    [SerializeField] private float m_circleBulletReverseDuration; // 공격 회전 방향 바꾸는 시간
    [SerializeField] private float m_circleBulletDuration; // 공격 시간
    public bool m_isCircleBulletOn = false; // 탄막 공격 시작?
    private bool m_canCircleBulletFire = false; // 탄막 쿨타임?
    private float m_circleBulletRotation = 0; // 회전각
    private bool m_isCircleBulletSequenceRunning = false;

    [Header("ZigZag")]
    [SerializeField] private float m_zigZagBulletfireCoolTime = 0.2f; // 탄막 층 생성 간격
    [SerializeField] private int m_zigZagBulletBranch; // 탄막 갈래
    [SerializeField] private float m_zigZagBulletSpawnRadius; // 탄막 생성 반지름
    [SerializeField] private float m_zigZagBulletSpeed; //탄막 속도
    [SerializeField] private float m_zigZagBulletRotationVelocity; // 탄막 중심 회전 속도
    [SerializeField] private int m_zigZagBulletSpawnDirection; // 탄막 중심 회전 방향 (시계방향: 1, 시계반대방향: -1)
    [SerializeField] private float m_zigZagBulletReverseDuration; // 공격 회전 방향 바꾸는 시간
    [SerializeField] private float m_zigZagBulletDuration; // 공격 시간
    public bool m_isZigZagBulletOn = false; // 탄막 공격 시작?
    private bool m_canZigZagBulletFire = false; // 탄막 쿨타임?
    private float m_zigZagBulletRotation = 0; // 회전각
    private bool m_isZigZagBulletSequenceRunning = false;

    private float m_fireCoolTimeSub;

    #endregion
    #region PublicMethod
    #endregion
    #region PrivateMethod
    private void Start()
    {
        bulletParents = GameObject.Find("Bullets");
    }
    private void Update()
    {
        //Test

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            m_isCircleBulletOn = !m_isCircleBulletOn;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            m_isZigZagBulletOn = !m_isZigZagBulletOn;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            m_circleBulletSpawnDirection = m_circleBulletSpawnDirection == 1 ? -1 : 1;
        }


        if (m_isCircleBulletOn)
        {
            StartCircleBullet();
            if(m_isCircleBulletSequenceRunning == false)
            {
                StopAllCoroutines();
                m_isCircleBulletSequenceRunning = true;
                StartCoroutine(CircleBulletSequence());
            }
        }

        if (m_isZigZagBulletOn)
        {
            StartZigZagBullet();
            if (m_isZigZagBulletSequenceRunning == false)
            {
                StopAllCoroutines();
                m_isZigZagBulletSequenceRunning = true;
                StartCoroutine(ZigZagBulletSequence());
            }
        }

    }

    private void StartCircleBullet()
    {
        m_circleBulletRotation += m_circleBulletRotationVelocity * Time.deltaTime * m_circleBulletSpawnDirection;

        if (m_canCircleBulletFire)
        {
            FireCircleBullet();
            m_fireCoolTimeSub = m_circleBulletfireCoolTime;
            m_canCircleBulletFire = false;
        }
        else
        {
            m_fireCoolTimeSub -= Time.deltaTime;

            if (m_fireCoolTimeSub <= 0)
                m_canCircleBulletFire = true;
        }
    }

    private void FireCircleBullet()
    {
        for (int i = 0; i < m_circleBulletBranch; i++)
        {
            float angle = 360f / m_circleBulletBranch * (i + 1) + m_circleBulletRotation;
            Vector2 spawnPosition = transform.position + new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad) * m_circleBulletSpawnRadius, Mathf.Sin(angle * Mathf.Deg2Rad) * m_circleBulletSpawnRadius, 0f);

            GameObject circleBullet = Instantiate(bullet, spawnPosition, Quaternion.identity);
            circleBullet.transform.SetParent(bulletParents.transform);
            circleBullet.GetComponent<ThugBossBullet>().Init(m_circleBulletSpeed, transform.position);
        }
    }


    // Circle 탄막 시퀀스
    private IEnumerator CircleBulletSequence()
    {
        yield return new WaitForSeconds(m_circleBulletReverseDuration);

        if(m_circleBulletReverseDuration > 0)
        {
            m_circleBulletSpawnDirection = m_circleBulletSpawnDirection == 1 ? -1 : 1;
            Debug.Log(m_circleBulletSpawnDirection);
        }
        yield return new WaitForSeconds(m_circleBulletDuration- m_circleBulletReverseDuration); // 일정 시간 대기

        // 탄막 공격 종료
        m_isCircleBulletOn = false;
        m_isCircleBulletSequenceRunning = false;
        ThugBoss.instance.EndAct();
    }


    private void StartZigZagBullet()
    {
        m_zigZagBulletRotation += m_zigZagBulletRotationVelocity * Time.deltaTime * m_zigZagBulletSpawnDirection;

        if (m_canZigZagBulletFire)
        {
            FireZigZagBullet();
            m_fireCoolTimeSub = m_zigZagBulletfireCoolTime;
            m_canZigZagBulletFire = false;

        }
        else
        {
            m_fireCoolTimeSub -= Time.deltaTime;

            if (m_fireCoolTimeSub <= 0)
                m_canZigZagBulletFire = true;
        }
    }

    private void FireZigZagBullet()
    {
        for (int i = 0; i < m_zigZagBulletBranch; i++)
        {
            float angle = 360f / m_zigZagBulletBranch * (i + 1) + m_zigZagBulletRotation;
            Vector2 spawnPosition = transform.position + new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad) * m_zigZagBulletSpawnRadius, Mathf.Sin(angle * Mathf.Deg2Rad) * m_zigZagBulletSpawnRadius, 0f);

            GameObject zigZagBullet = Instantiate(bullet, spawnPosition, Quaternion.identity);
            zigZagBullet.transform.SetParent(bulletParents.transform);
            zigZagBullet.GetComponent<ThugBossBullet>().Init(m_zigZagBulletSpeed, transform.position);
        }
    }

    // 지그재그 탄막 시퀀스
    private IEnumerator ZigZagBulletSequence()
    {
        yield return new WaitForSeconds(m_zigZagBulletReverseDuration);
        if(m_zigZagBulletReverseDuration > 0)
        {
            m_zigZagBulletSpawnDirection = m_zigZagBulletSpawnDirection == 1 ? -1 : 1;
        }
        

        yield return new WaitForSeconds(m_zigZagBulletDuration - m_zigZagBulletReverseDuration); // 일정 시간 대기

        // 탄막 공격 종료
        m_isZigZagBulletOn = false;
        m_isZigZagBulletSequenceRunning = false;
        ThugBoss.instance.EndAct();
    }

    #endregion
}
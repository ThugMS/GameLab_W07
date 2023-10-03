using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThugBossPhase1 : MonoBehaviour
{
    #region PublicVariables
    public GameObject bullet;
    #endregion
    #region PrivateVariables
    [Header("CircleBullet")]
    [SerializeField] private bool m_isCircleBulletOn = false; // 탄막 공격 시작?
    [SerializeField] private bool m_canFire = false; // 탄막 쿨타임?
    [SerializeField] private float m_fireCoolTime = 0.2f; // 탄막 층 생성 간격
    [SerializeField] private int m_circleBulletBranch; // 탄막 갈래
    [SerializeField] private float m_circleBulletDelay; // 탄막 딜레이
    [SerializeField] private float m_circleBulletSpawnRadius; // 탄막 생성 반지름
    [SerializeField] private float m_bulletSpeed; //탄막 속도
    [SerializeField] private float m_bulletRotationVelocity; // 탄막 중심 회전 속도
    [SerializeField] private int m_bulletSpawnDirection; // 탄막 중심 회전 방향 (시계방향: 1, 시계반대방향: -1)
    [SerializeField] private float m_circleBulletRotation =0; // 회전각
    private float m_fireCoolTimeSub;

    #endregion
    #region PublicMethod
    #endregion
    #region PrivateMethod
    private void Start()
    {
    }
    private void Update()
    {
        //Test

      if (Input.GetKeyDown(KeyCode.Alpha1)){
            m_isCircleBulletOn = !m_isCircleBulletOn;
        }

        if (m_isCircleBulletOn)
        {
            m_circleBulletRotation += m_bulletRotationVelocity * Time.deltaTime * m_bulletSpawnDirection;
            // m_bulletSpawnPos의 로테이션 설정

            if (m_canFire)
            {
                FireCircleBullet();
                m_fireCoolTimeSub = m_fireCoolTime;
                m_canFire = false;
            }
            else
            {
                m_fireCoolTimeSub -= Time.deltaTime;

                if (m_fireCoolTimeSub <= 0)
                    m_canFire = true;
            }
        }

    }
    private void FireCircleBullet()
    {
        for(int i = 0; i < m_circleBulletBranch; i++)
        {
            float angle = 360f / m_circleBulletBranch * (i + 1) + m_circleBulletRotation;
            Vector2 spawnPosition = transform.position + new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad) * m_circleBulletSpawnRadius, Mathf.Sin(angle * Mathf.Deg2Rad) * m_circleBulletSpawnRadius, 0f);

            GameObject circleBullet = Instantiate(bullet, spawnPosition, Quaternion.identity);
            //circleBullet.transform.SetParent(transform);
            circleBullet.GetComponent<ThugBossBullet>().Init(m_bulletSpeed);
        }
    }

    #endregion
}
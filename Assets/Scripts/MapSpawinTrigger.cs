using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MapSpawinTrigger : MonoBehaviour
{
    public static MapSpawinTrigger instance;

    #region PublicVariables
    public Vector2 m_spawnPos;
    public int m_stageNum = 0;
    public TutorialTileTwo tutorialTileTwo;
    public Transform[] spawnPoint;
    public GameObject m_bossPrefab;
    #endregion

    #region PrivateVariables
    private GameObject boss;
    #endregion

    #region PublicMethod
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        boss = DefensiveBoss.instance.gameObject;
        boss.SetActive(false);
    }

    private void FixedUpdate()
    {
        m_stageNum = 3;
    }

    public void SetSpawnPos(Transform _tf)
    {
        m_spawnPos = new Vector2(_tf.position.x, _tf.position.y);
    }

    public void SpawnPlayer()
    {

        switch (m_stageNum)
        {
            case 1:
                SetSpawnPos(spawnPoint[0]);
                break;

            case 2:
                tutorialTileTwo.SpawnMonsters(20, 1);
                SetSpawnPos(spawnPoint[1]);
                break;

            case 3:
                SetSpawnPos(spawnPoint[2]);

                Vector3 bossPos = spawnPoint[2].position + new Vector3(0, 5, 0);

                Destroy(boss);

                GameObject obj = Instantiate(m_bossPrefab, bossPos, Quaternion.identity);
                obj.GetComponent<DefensiveBoss>().InitSetting();
                boss = obj;
                tutorialTileTwo.SpawnMonsters(10, 2);

                //boss.SetActive(true);
                //boss.GetComponent<DefensiveBoss>().InitSetting();
                //DefensiveBoss.instance.gameObject.SetActive(true);
                //DefensiveBoss.instance.InitSetting();
                break;
        }

        Player.instance.SetPosition(m_spawnPos);
        Player.instance.Initialize();

    }
    #endregion

    #region PrivateMethod


    #endregion
}

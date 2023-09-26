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
    #endregion

    #region PrivateVariables
    #endregion

    #region PublicMethod
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public void SetSpawnPos(Transform _tf)
    {
        m_spawnPos = new Vector2(_tf.position.x, _tf.position.y);
    }

    public void SpawnPlayer()
    {
        Player.instance.SetPosition(m_spawnPos);
        Player.instance.Initialize();

        switch (m_stageNum)
        {
            case 1:
                break;

            case 2:
                break;

            case 3:
                DefensiveBoss.instance.InitSetting();
                break;
        }
    }
    #endregion

    #region PrivateMethod


    #endregion
}

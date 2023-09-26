using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow : MonoBehaviour
{
    #region PublicVariables
    public LineRenderer m_line;
    #endregion

    #region PrivateVariables
    private float m_time;
    #endregion

    #region PublicMethod
    /// <summary>
    /// 생성시 초기화 함수. 플레이어가 돌아가는데 걸리는 시간을 인자로 준다.
    /// </summary>
    public void InitSetting(float _time)
    {
        m_time = _time;
        StartCoroutine(nameof(IE_Destroy));
    }

    public void ReturnPlayer()
    {
        // To-Do 플레이어 그림자로 이동시키는 함수
        Player.instance.transform.position = transform.position;
    }

    private void Start()
    {   
        TryGetComponent<LineRenderer>(out m_line);
        m_line.positionCount = 2;    
    }

    private void Update()
    {
        m_line.SetPosition(0, transform.position);
        m_line.SetPosition(1, Player.instance.transform.position);
    }
    #endregion

    #region PrivateMethod
    private IEnumerator IE_Destroy()
    {
        yield return new WaitForSeconds(m_time);
        ReturnPlayer();
        Destroy(gameObject);
    }
    #endregion
}

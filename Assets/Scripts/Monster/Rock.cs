using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UIElements;

public class Rock : MonoBehaviour
{
    #region PublicVariables
    public bool m_isDivide = false;
    public GameObject m_divideRock;
    #endregion

    #region PrivateVariables
    [Header("Stat")]
    [SerializeField] private float m_speed;
    [SerializeField] private Vector3 m_dir;

    [Header("Divde")]
    [SerializeField] private int m_divdeNum = 1;
    [SerializeField] private float m_divdeAngle = 120f;
    #endregion

    #region PublicMethod
    private void Start()
    {
        if (m_isDivide == true)
        {
            gameObject.transform.Find("renderer").transform.localScale = Vector3.one;
        }
    }

    private void Update()
    {
        Move();
    }

    public void InitSetting(Vector3 _dir, bool _divde)
    {
       m_dir = _dir;
       m_isDivide = _divde;
    }
    #endregion

    #region PrivateMethod
    private void Move()
    {
        transform.position += m_dir * m_speed * Time.deltaTime;
    }

    private void Divide()
    {
        Vector3 dir = -m_dir;
        float angle = m_divdeAngle / m_divdeNum;
        

        for(int i=-m_divdeNum/2; i <= m_divdeNum / 2; i++)
        {
            var quater = Quaternion.Euler(0, 0, angle * i);
            Vector3 v = quater * dir;

            GameObject rock = Instantiate(m_divideRock, transform.position, Quaternion.identity);
            rock.GetComponent<Rock>().InitSetting(v.normalized, true);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            if (m_isDivide == false)
            {
                Divide();
            }
            Destroy(gameObject);
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Player.instance.Hit(-1);
            Destroy(gameObject);
        }
    }
    #endregion
}

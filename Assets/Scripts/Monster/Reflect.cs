using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Audio;

public class Reflect : MonoBehaviour
{
    #region PublicVariables
    public int m_arrowLayerMask;
    public CircleCollider2D m_collider;
    public bool m_isThrowProjectTile = false;
    public GameObject m_effect;
    #endregion

    #region PrivateVariables
    [SerializeField] private GameObject m_projectile;
    [SerializeField] private float m_reflectTime;
    [SerializeField] private float m_deceleration = 0.5f;
    [SerializeField] List<Collider2D> m_arrows = new List<Collider2D>();
    [SerializeField] List<ReflectProjectile> m_projs = new List<ReflectProjectile>();
    [SerializeField] private float m_throwCool = 0.5f;
    [SerializeField] private float m_projectileSpeed = 20f;
    #endregion

    #region PublicMethod
    private void Awake()
    {
        m_arrowLayerMask = LayerMask.GetMask("");
        TryGetComponent<CircleCollider2D>(out m_collider);
    }

    private void Update()
    {
        DecreaseSpeed();
    }

    public void InitSetting(float _time)
    {
        m_reflectTime = _time;
        m_collider.enabled = true;
        m_isThrowProjectTile = false;
        m_arrows = new List<Collider2D>();
        m_effect.SetActive(true);

        StartCoroutine(nameof(IE_Reflect));
    }

    public void ReflectAttack()
    {
        m_collider.enabled = false;
        m_isThrowProjectTile = true;
        m_effect.SetActive(false);

        StopAndChangeArrow();
        
        StartCoroutine(nameof(IE_ThrowProjectile));
    }
    #endregion

    #region PrivateMethod
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Arrow obj = collision.gameObject.GetComponent<Arrow>();

        if (obj == null)
            return;

        m_arrows.Add(obj.GetComponent<Collider2D>());
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Arrow obj = collision.gameObject.GetComponent<Arrow>();

        if (obj == null)
            return;

        float speed = obj.GetSpeed();
        obj.SetSpeed(speed * m_deceleration);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Arrow obj = collision.gameObject.GetComponent<Arrow>();

        if (obj == null)
            return;

        int hash = obj.gameObject.GetHashCode();

        foreach(var iter in m_arrows)
        {
            if(hash == iter.GetHashCode())
            {
                m_arrows.Remove(iter);
            }
        }
    }

    private void DecreaseSpeed()
    {
        if (m_isThrowProjectTile == true)
            return;

        for (int i = 0; i < m_arrows.Count; i++)
        {
            Collider2D iter = m_arrows[i];

            if (iter.gameObject.activeSelf == false )
            {
                m_arrows.Remove(iter);
                i--;
                continue;
            }

            Arrow obj = iter.GetComponent<Arrow>();

            float speed = obj.GetSpeed();
            obj.SetSpeed(speed * m_deceleration);
        }
    }

    private void StopAndChangeArrow()
    {
        for (int i = 0; i < m_arrows.Count; i++)
        {
            Collider2D iter = m_arrows[i];

            Arrow obj = iter.GetComponent<Arrow>();
            obj.Deactive();

            GameObject objProj = Instantiate(m_projectile, iter.transform.position, Quaternion.identity, transform);
            m_projs.Add(objProj.GetComponent<ReflectProjectile>());
        }
    }

    private IEnumerator IE_Reflect()
    {
        yield return new WaitForSeconds(m_reflectTime);

        ReflectAttack();
    }

    private IEnumerator IE_ThrowProjectile()
    {
        foreach (var iter in m_projs)
        {
            iter.InitSetting(m_projectileSpeed);

            yield return new WaitForSeconds(m_throwCool);
        }
    }
    #endregion
}

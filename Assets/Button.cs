using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    #region PublicVariables
    public Sprite buttonSprite;

    #endregion
    #region PrivateVariables
    private SpriteRenderer sr;
    
    #endregion
    #region PublicMethod
    #endregion
    #region PrivateMethod

    private void Start()
    {
        TryGetComponent<SpriteRenderer>(out sr);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            StartCoroutine(ChangeSprite());
        }
        
    }

    private IEnumerator ChangeSprite()
    {
        sr.sprite = buttonSprite;
        yield return new WaitForSeconds(1f);
        GoNextStage();
    }

    private void GoNextStage()
    {
        MapSpawinTrigger.instance.m_stageNum += 1;
        MapSpawinTrigger.instance.SpawnPlayer();
    }

    #endregion
}
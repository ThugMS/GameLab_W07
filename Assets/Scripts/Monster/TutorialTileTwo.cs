using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTileTwo : MonoBehaviour
{
    #region PublicVariables
    #endregion
    #region PrivateVariables
    [SerializeField] private int numberOfMonsters = 20;
    [SerializeField] private GameObject monsterPrefab;
    [SerializeField] private float spawnRadius=8f;
    [SerializeField] private bool isCleaningForRestart=false;
    public Transform monsterParents;
    

    public List<GameObject> monsters = new List<GameObject>(); 
    #endregion
    #region PublicMethod
    #endregion
    #region PrivateMethod

    private void Start()
    {
    }

    private void Update()
    {
        if (MapSpawinTrigger.instance.m_stageNum != 2)
        {
            return;
        }

        if (monsterParents.gameObject.transform.childCount == 0 && !isCleaningForRestart)
        {
            // 다음 스테이지로 이동
            Debug.Log("lext stage");
            MapSpawinTrigger.instance.m_stageNum += 1;
            MapSpawinTrigger.instance.SpawnPlayer();
            isCleaningForRestart = true;
        }
    }

    [Button]
    public void SpawnMonsters()
    {
        CleanMonsters();
        for (int i = 0; i<numberOfMonsters; i++)
        {
            //float angle = Random.Range(0f, 360f);
            float angle = 360f / numberOfMonsters * (i + 1);
            Vector2 spawnPosition = Player.instance.transform.position + new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad) * spawnRadius, Mathf.Sin(angle * Mathf.Deg2Rad) * spawnRadius, 0f);

          
            GameObject monster = Instantiate(monsterPrefab, spawnPosition, Quaternion.identity);
            monsters.Add(monster);
            monster.transform.SetParent(monsterParents);

        }

        isCleaningForRestart = false;
    }

    [Button]
    public void CleanMonsters()
    {
        Debug.Log("CleanMonsters");
        isCleaningForRestart = true;
        for (int i = monsters.Count - 1; i >= 0; i--)
        {
            if (monsters[i] != null)
            {
                Destroy(monsters[i]);
            }
            monsters.RemoveAt(i);
        }

    }

#endregion
}
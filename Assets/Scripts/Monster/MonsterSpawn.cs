using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawn : MonoBehaviour
{
    #region PublicVariables
    #endregion
    #region PrivateVariables
    [SerializeField] private int numberOfMonsters = 20;
    [SerializeField] private GameObject monsterPrefab;
    [SerializeField] private float spawnRadius=8f;
    [SerializeField] private float spawnDelay = 0f;
    #endregion
    #region PublicMethod
    #endregion
    #region PrivateMethod

    private void Start()
    {
        StartCoroutine(SpawnMonsters());

    }
    private IEnumerator SpawnMonsters()
    {
        for (int i = 0; i<numberOfMonsters; i++)
        {
            //float angle = Random.Range(0f, 360f);
            float angle = 360f / numberOfMonsters * (i + 1);
            Vector2 spawnPosition = Player.instance.transform.position + new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad) * spawnRadius, Mathf.Sin(angle * Mathf.Deg2Rad) * spawnRadius, 0f);

          
            Instantiate(monsterPrefab, spawnPosition, Quaternion.identity);

            yield return new WaitForSeconds(spawnDelay);
        }
    }
#endregion
}
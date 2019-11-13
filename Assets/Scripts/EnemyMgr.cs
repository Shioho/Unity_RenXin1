using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMgr : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform enemiesShowNode;
    public Transform enemiesPoolNode;

    private List<GameObject> _enemiesPool = new List<GameObject>();


    //初始化敌人相关
    public void InitEnemies(List<Vector3> posList)
    {
        InitEnemiesPool();
        InitEnemiesView(posList);
    }

    //初始化对象池
    private void InitEnemiesPool()
    {
        int num = 20;//初始化数量

        for (int i = 0; i < num; i++)
        {
            CreateEnemy();
        }
    }


    private GameObject CreateEnemy()
    {
        GameObject enemy = Instantiate(enemyPrefab, enemiesPoolNode);
        enemy.GetComponent<EnemyObj>().ResetObj();
        AddEnemy2Pool(enemy);
        return enemy;
    }

    public void AddEnemy2Pool(GameObject obj) {
        _enemiesPool.Add(obj);
    }

    private GameObject GetFreeEnemy() {
        GameObject enemy;

        if (_enemiesPool.Count > 0)
        {
            enemy = _enemiesPool[0];
            _enemiesPool.Remove(enemy);
        }
        else
        {
            enemy = CreateEnemy();
            _enemiesPool.Remove(enemy);
        }
        enemy.GetComponent<EnemyObj>().ShowObj();
        enemy.transform.SetParent(enemiesShowNode);
        return enemy;
    }

    private void RecoveryObjPool(GameObject obj) {
        obj.GetComponent<EnemyObj>().ResetObj();
        _enemiesPool.Add(obj);
        obj.transform.SetParent(enemiesPoolNode);
    }



    private void InitEnemiesView(List<Vector3> posList)
    {
        for (int i = 0; i < posList.Count; i++) {
            GameObject enemy = GetFreeEnemy();
            enemy.transform.position = posList[i];
        }
    }


    public void UpdateEnemies(List<Vector3> newPointList) {

        for (int i = 0; i < newPointList.Count; i++)
        {
            GameObject enemy = GetFreeEnemy();
            enemy.transform.position = newPointList[i];
        }

    }

    public void DoKillEnemy() {
        if (enemiesShowNode.childCount < 1) return;

        GameObject target = enemiesShowNode.GetChild(0).gameObject;
        RecoveryObjPool(target);
    }








}

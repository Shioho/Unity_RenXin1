using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainMgr : MonoBehaviour
{


    private List<int> _terrainsDataList = new List<int>();//用于存储当前的地形数据,包括空地
    private Dictionary<int, List<GameObject>> _terriansPool = new Dictionary<int, List<GameObject>>();//地形池
    private int _curTerrainType;//当前渲染的地形

    private Vector3 _smoothVecolity;
    private float _terrainMoveDisZ = 0;//地形z总偏移量


    public Transform terrainsShowNode;//要展示的地形父节点
    public Transform terrainsPoolNode;//对象池存放的父节点
    public GameObject[] terrainsPreb;//所有地形预制
    public int maxTerrainsCnt = 8;//渲染最大地形数，包括空地形

    //初始化地形相关
    public void InitTerrains()
    {
        InitTerrainsPool();
        InitTerrainsData();
        InitTerrainsView();
    }

    public List<int> GetTerrainDataList() {
        return _terrainsDataList;
    }

    public bool CheckIsEmptyTerrainByType(int type) {

        return type == 0;
    }











    //初始化对象池
    private void InitTerrainsPool()
    {

        for (int i = 0; i < terrainsPreb.Length; i++)
        {
            _terriansPool[i] = new List<GameObject>();
            int num = 5;//每个地形创建几个
            for (int j = 0; j < num; j++)
            {
                CreateTerrainByType(i);
            }
        }
    }

    //根据类型创建地形预制
    private GameObject CreateTerrainByType(int type)
    {
        List<GameObject> list = _terriansPool[type];
        GameObject terrain = Instantiate(terrainsPreb[type], terrainsPoolNode);
        terrain.GetComponent<TerrainObj>().ResetObj();
        terrain.GetComponent<TerrainObj>().InitData(type);
        list.Add(terrain);
        return terrain;
    }

    //根据类型获取地形实例
    private GameObject GetTerrianByType(int type)
    {
        List<GameObject> terrainList = _terriansPool[type];
        GameObject terrain;

        if (terrainList.Count > 0)
        {
            terrain = terrainList[0];
            terrainList.Remove(terrain);
        }
        else
        {
            terrain = CreateTerrainByType(type);
        }
        terrain.GetComponent<TerrainObj>().ShowObj();
        terrain.transform.SetParent(terrainsShowNode);

        return terrain;
    }

    //初始化当前地形数据
    private void InitTerrainsData()
    {
        //第一个地形不为空
        int type = Random.Range(1, 8);
        _curTerrainType = type;
        _terrainsDataList.Add(type);

        while (_terrainsDataList.Count < maxTerrainsCnt)
        {
            type = getNextTerrainType();
            _terrainsDataList.Add(type);
        }

    }
    //确保不会连续刷出空地
    private int getNextTerrainType()
    {
        int minInt = -terrainsPreb.Length + 3;
        int type = Mathf.Max(0, Random.Range(minInt, terrainsPreb.Length));
        //type=0为空地
        if (_curTerrainType <= 0)
        {
            while (type <= 0)
            {
                type = Random.Range(minInt, terrainsPreb.Length);
            }
        }
        _curTerrainType = type;
        return type;
    }

    //初始化地形
    private void InitTerrainsView()
    {
        for (int i = 0; i < _terrainsDataList.Count; i++)
        {
            int type = _terrainsDataList[i];
            GameObject terrain = GetTerrianByType(type);

            Vector3 pos = terrain.transform.position;


            terrain.transform.position = new Vector3(pos.x, pos.y, _terrainMoveDisZ);
            AddZMoveOffset(type);

        }


        PrintTerrainInfo();
    }

    private void AddZMoveOffset(int type) {
        float zEmptyOffset = 5.5f;//地形间的偏移量
        float zOneStandOffset = 1.7f;//地形间的偏移量
        float zOneStandSmallOffset = 1.0f;//地形间的偏移量
        float zTwoStandOffset = 2.8f;//地形间的偏移量
        float zThreeStandOffset = 3.8f;//地形间的偏移量

        float targetOffset = 0;

        if (type == 0)
        {
            targetOffset = zEmptyOffset;
        }
        else if (type < 7)
        {
            targetOffset = zOneStandOffset;
        }
        else if (type < 9)
        {
            targetOffset = zOneStandSmallOffset;
        }
        else if (type < 11)
        {
            targetOffset = zTwoStandOffset;
        }
        else {
            targetOffset = zThreeStandOffset;
        }

        _terrainMoveDisZ += targetOffset;
    }



    //更新地形数据信息
    private void UpdateTerrainsData()
    {
        _terrainsDataList.Remove(_terrainsDataList[0]);
        _terrainsDataList.Add(getNextTerrainType());
    }




    //刷新地形
    private void UpdateTerrainsView()
    {

        int cnt = terrainsShowNode.childCount;
        if (cnt <= 0) return;


        //加入最后一个
        int type = _terrainsDataList[_terrainsDataList.Count - 1];
        GameObject terrain = GetTerrianByType(type);

        AddZMoveOffset(type);
        terrain.transform.position = new Vector3(terrain.transform.position.x, terrain.transform.position.y, _terrainMoveDisZ);


        //移除第一个
        Transform obj = GetTerrainObjByIdx(0).transform;
        type = obj.GetComponent<TerrainObj>().GetPoolType();
        List<GameObject> terrainList = _terriansPool[type];
        terrainList.Add(obj.gameObject);
        obj.GetComponent<TerrainObj>().ResetObj();
        obj.SetParent(terrainsPoolNode);


        PrintTerrainInfo();
    }



    //移动地形
    public void DoTerrainMove()
    {
        UpdateTerrainsData();
        UpdateTerrainsView();
    }


    //获取当前角色所在地形实例
    public GameObject GetCurPlayerTerrain()
    {
        return GetTerrainObjByIdx(0);
    }


    public GameObject GetTerrainObjByIdx(int idx)
    {
        return terrainsShowNode.GetChild(idx).gameObject;
    }


    public int GetMaxTerrainCnt() {
        return maxTerrainsCnt;
    }





















    private void PrintTerrainInfo()
    {
        //return;
        string info = "";
        for (int i = 0; i < _terrainsDataList.Count; i++)
        {
            int type = _terrainsDataList[i];

            info += type + ",";
        }
        print(info);
    }




}

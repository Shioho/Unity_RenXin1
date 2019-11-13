using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PointType
{
    None,
    Empty,
    Enemy
}


public class GameMgr : MonoBehaviour    //主控制管理器
{
    private TerrainMgr _terrainMgr;
    private EnemyMgr _enemyMgr;


    private Transform _myPlayer;
    private Camera _cameraMain;
    private Transform _playerCameraPos;
    private Vector3 _cameraSmoothVecolity;
    private Vector3 _playerSmoothVecolity;
    private Vector3 _playerTargetPos;


    private Dictionary<int, List<PointType>> _pointTypeDic = new Dictionary<int, List<PointType>>();//用于存储当前地形对应的所有移动点类型
    private PlayerObj _myPlayerScript;
    private List<Vector3> _enemiesPosList = new List<Vector3>();

    public GameObject playerPrefab;

    private void Awake()
    {
        _terrainMgr = GetComponent<TerrainMgr>();
        _enemyMgr = GetComponent<EnemyMgr>();

        //1.初始化地形

        _terrainMgr.InitTerrains();
        //2.存储当前地形的点信息
        InitPointList();

        //3.初始化敌人
        _enemyMgr.InitEnemies(_enemiesPosList);


        //4.创建角色摄像机跟随
        _myPlayer = Instantiate(playerPrefab).transform;
        _myPlayerScript = _myPlayer.GetComponent<PlayerObj>();
        _cameraMain = Camera.main;
        _playerCameraPos = _myPlayer.Find("cameraPos");

    }

    private void InitPointList()
    {
        _pointTypeDic[0] = new List<PointType>();
        TerrainObj script = _terrainMgr.GetTerrainObjByIdx(0).GetComponent<TerrainObj>();
        int cnt = script.GetWalkPointCnt();
        for (int i = 0; i < cnt; i++)
        {
            //出生点为空
            if (i == 0)
            {
                _pointTypeDic[0].Add(PointType.Empty);
            }
            else
            {
                _pointTypeDic[0].Add(PointType.Enemy);
                _enemiesPosList.Add(script.GetWalkPointPos(i));
            }
        }

        List<int> terrainList = _terrainMgr.GetTerrainDataList();
        for (int i = 1; i < terrainList.Count; i++)
        {
            int preType = terrainList[i - 1];

            script = _terrainMgr.GetTerrainObjByIdx(i).GetComponent<TerrainObj>();
            cnt = script.GetWalkPointCnt();
            _pointTypeDic[i] = new List<PointType>();
            if (cnt == 0)
            {
                _pointTypeDic[i].Add(PointType.None);
                continue;
            }

            for (int j = 0; j < cnt; j++)
            {
                if (_terrainMgr.CheckIsEmptyTerrainByType(preType))
                {
                    preType = 1;
                    _pointTypeDic[i].Add(PointType.Empty);
                }
                else
                {
                    //TODO暂时先去设置为敌人，后面可能是物品啥的
                    _pointTypeDic[i].Add(PointType.Enemy);
                    _enemiesPosList.Add(script.GetWalkPointPos(j));
                }
            }
        }
    }

    private void UpdatePlayerPos(int tIdx, int pIdx)
    {
        _myPlayerScript.UpdatePosState(tIdx, pIdx);
    }




    // Start is called before the first frame update
    void Start()
    {
        UpdatePlayerPos(0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        //A键跳跃
        if (Input.GetKeyDown(KeyCode.A))
        {
            DoPlayerJump();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            DoPlayerKill();
        }

    }

    private void FixedUpdate()
    {
        UpdatePlayerMove();
        UpdateCameraPos();
    }

    private void DoPlayerKill()
    {
        PointType type = GetNextPointType();
        switch (type)
        {
            case PointType.Enemy:
                {
                    //杀敌
                    DoPlayerMove();
                    _enemyMgr.DoKillEnemy();
                    break;
                }
            case PointType.None:
            case PointType.Empty:
                {
                    //TODO操作失误
                    Debug.LogWarning("操作失误拉！！！");
                    break;
                }

        }

    }


    private void DoPlayerJump()
    {

        PointType type = GetNextPointType();

        switch (type)
        {
            case PointType.Empty:
            case PointType.None:
                {
                    DoPlayerMove();
                    break;
                }
            case PointType.Enemy:
                {
                    //TODO操作失误
                    Debug.LogWarning("操作失误拉！！！");
                    break;
                }

        }
    }



    private PointType GetNextPointType()
    {

        int tIdx = _myPlayerScript.GetCurTerrainIdx();
        int pIdx = _myPlayerScript.GetCurPointIdx();

        List<PointType> list = _pointTypeDic[tIdx];
        pIdx++;
        if (list.Count > pIdx)
        {
            return list[pIdx];
        }
        else
        {
            tIdx++;
            return _pointTypeDic[tIdx][0];
        }
    }

    private PointType GetCurPointType()
    {
        int tIdx = _myPlayerScript.GetCurTerrainIdx();
        int pIdx = _myPlayerScript.GetCurPointIdx();
        List<PointType> list = _pointTypeDic[tIdx];
        return list[pIdx];
    }



    private void UpdatePointTypeDic() {
        List<int> terrainList = _terrainMgr.GetTerrainDataList();
        int idx = terrainList.Count - 1;
        _enemiesPosList = new List<Vector3>();

        //前移
        for (int i = 0; i < idx; i++) {
            _pointTypeDic[i] = _pointTypeDic[i + 1];
        }


        //更新最后一个
        int preType = terrainList[idx-1];
        int type = terrainList[idx];
        _pointTypeDic[idx] = new List<PointType>();

        TerrainObj  script = _terrainMgr.GetTerrainObjByIdx(idx).GetComponent<TerrainObj>();
        int cnt = script.GetWalkPointCnt();
        if (cnt == 0)
        {
            _pointTypeDic[idx].Add(PointType.None);
            return;
        }
        for (int i = 0; i < cnt; i++) {
            if (_terrainMgr.CheckIsEmptyTerrainByType(preType))
            {
                preType = 1;
                _pointTypeDic[idx].Add(PointType.Empty);
            }
            else
            {
                //TODO暂时先去设置为敌人，后面可能是物品啥的
                _pointTypeDic[idx].Add(PointType.Enemy);
                _enemiesPosList.Add(script.GetWalkPointPos(i));
            }
        }

        
    }






    private void DoPlayerMove()
    {

        TerrainObj terrainObj = _terrainMgr.GetCurPlayerTerrain().GetComponent<TerrainObj>();

        int pIdx = _myPlayerScript.GetCurPointIdx();
        pIdx++;
        List<PointType> list = _pointTypeDic[0];

        if (list.Count > pIdx)
        {
            _playerTargetPos = terrainObj.GetWalkPointPos(pIdx);
            UpdatePlayerPos(0, pIdx);
        }
        else
        {
            _terrainMgr.DoTerrainMove();

            terrainObj = _terrainMgr.GetCurPlayerTerrain().GetComponent<TerrainObj>();
            _playerTargetPos = terrainObj.GetWalkPointPos(0);
            UpdatePlayerPos(0, 0);
            UpdatePointTypeDic();
            _enemyMgr.UpdateEnemies(_enemiesPosList);

        }

        while (GetCurPointType()==PointType.None) {
            DoPlayerMove();
        }
    }



    private void UpdatePlayerMove()
    {
        _myPlayer.position = Vector3.SmoothDamp(_myPlayer.position, _playerTargetPos, ref _playerSmoothVecolity, 0.1f);
    }


    private void UpdateCameraPos()
    {
        _cameraMain.transform.eulerAngles = _playerCameraPos.eulerAngles;
        _cameraMain.transform.position = Vector3.SmoothDamp(_cameraMain.transform.position, _playerCameraPos.position, ref _cameraSmoothVecolity, 0.1f);
    }


}

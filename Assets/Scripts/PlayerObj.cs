using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObj : MonoBehaviour
{
    private int _curTerrainIdx = 0;//当前所在的地形索引
    private int _curPointIdx = 0;//当前所在的地形点索引


    public void UpdatePosState(int terrainIdx,int pointIdx) {
        _curTerrainIdx = terrainIdx;
        _curPointIdx = pointIdx;
    }

    public int GetCurTerrainIdx() {
        return _curTerrainIdx;
    }

    public int GetCurPointIdx() {
        return _curPointIdx;
    }


}

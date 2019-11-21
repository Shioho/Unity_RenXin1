using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TerrainType
{
    StandNone,
    StandOne,
    StandTwo,
    StandThree
}

public class TerrainObj : MonoBehaviour
{
    [Header("============當前地形能站幾個人==============")]
    public TerrainType standType;

    public GameObject[] walkPoints;

    //在对象池中对应的类型
    private int _curPoolType = 0;



    public void InitData(int type)
    {
        _curPoolType = type;
    }



    public int GetPoolType()
    {
        return _curPoolType;
    }

    //重置数据
    public void ResetObj()
    {
        gameObject.SetActive(false);
        transform.position = Vector3.zero;
    }
    public void ResetObjWait()
    {
        if (gameObject.activeSelf)
        {


            StartCoroutine(HideObj());
        }


    }

    IEnumerator HideObj()
    {
        yield return new WaitForSeconds(2.0f);
        gameObject.SetActive(false);
        transform.position = Vector3.zero;
    }

    public void ShowObj()
    {
        gameObject.SetActive(true);
    }

    //获取位置
    public Vector3 GetWalkPointPos(int idx)
    {
        if (idx >= walkPoints.Length) return Vector3.zero;

        Vector3 pos = walkPoints[idx].transform.position;
        return pos;
    }

    public Vector3[] GetWalkPointPosArr()
    {
        Vector3[] posArr = new Vector3[walkPoints.Length];

        for (int i = 0; i < walkPoints.Length; i++)
        {
            posArr[i] = walkPoints[i].transform.position;
        }

        return posArr;
    }

    public int GetWalkPointCnt()
    {
        return walkPoints.Length;
    }



}

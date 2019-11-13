using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyObj : MonoBehaviour
{

    //重置数据
    public void ResetObj()
    {
        gameObject.SetActive(false);
        transform.position = Vector3.zero;
    }

    public void ShowObj()
    {
        gameObject.SetActive(true);
    }

}

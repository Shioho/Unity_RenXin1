using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyObj : MonoBehaviour
{
    public Animator animator;
    public float prepareAttackTime = 1.0f;

    private bool _isDie = false;
    //重置数据

    public void ResetObj()
    {
        gameObject.SetActive(false);
        _isDie = false;
        transform.position = Vector3.zero;
    }

    public void ShowObj()
    {
        gameObject.SetActive(true);

    }

    private void Update()
    {
        if (gameObject.activeSelf && !_isDie)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player)
            {
                Vector3 dir = player.transform.position - transform.position;

                transform.forward = dir;
            }
        }

    }

    public void DoDieAnima()
    {
        _isDie = true;
        animator.SetTrigger("die");
    }

    public void DoPrepareAttackAnima()
    {
        if (!gameObject.activeSelf || _isDie) return;
        animator.SetTrigger("prepareAttack");
    }

    private void OnEnemyAttackEnter(GameObject obj,string type)
    {
        if(type!="OnEnemyAttackEnter")return;
        if(obj.transform.parent.gameObject!=gameObject) return;
        // print("Attack Success!!!");
        DoPrepareAttackAnima();
    }

    private void Awake() {
        FSMOnEnter.onEnterEvents+=OnEnemyAttackEnter;
    }


}

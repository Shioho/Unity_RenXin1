using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObj : MonoBehaviour
{
    public Animator animator;
    private int _curTerrainIdx = 0;//当前所在的地形索引
    private int _curPointIdx = 0;//当前所在的地形点索引
    private Rigidbody _rigidbody;

    private void Awake() {
        _rigidbody = GetComponent<Rigidbody>();
    }

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


    public void DoAttackAnima(){
        animator.SetTrigger("attack");
    }

    public void DoJumpAnima(){
        // _rigidbody.AddForce(new Vector3(0,10,0));
        animator.SetBool("isJumping",true);
    }
    public void StopJumpAnima(){
        animator.SetBool("isJumping",false);
    }

    private void OnCollisionEnter(Collision other) {
        if(other.transform.tag == "Terrain"){
            StopJumpAnima();
        }
    }


    public void ChangeModelForward(Vector3 forward){
        animator.transform.forward = forward;
    }

    public void DoBeatenAnima(){
        animator.SetTrigger("beaten");
    }


}

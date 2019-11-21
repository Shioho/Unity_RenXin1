using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectStep
{
    Normal,
    Gray,
    LinerAndColor,
    BlackLiner,
    OneColorLiner1,
    OneColorLiner2,
    EndColorLiner
}


public class CameraEffectMgr : MonoBehaviour
{
    public MonoBehaviour[] effectScripts;

    private Camera _mainCamera;
    private EffectStep _curSetp = EffectStep.Normal;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    public void UpdateEffectState(int combo)
    {
        int baseCnt = 10;
        if (combo < baseCnt)
        {
            SetState(EffectStep.Normal);
        }
        else if (combo < baseCnt * 2)
        {
            SetState(EffectStep.Gray);
        }
        else if (combo < baseCnt * 3)
        {
            SetState(EffectStep.LinerAndColor);
        }
        else if (combo < baseCnt * 4)
        {
            SetState(EffectStep.BlackLiner);
        }
        else if (combo < baseCnt * 5)
        {
            SetState(EffectStep.OneColorLiner1);
        }
        else if (combo < baseCnt * 6)
        {
            SetState(EffectStep.OneColorLiner2);
        }
        else
        {
            SetState(EffectStep.EndColorLiner);
        }

    }

    private void SetState(EffectStep step)
    {
        if (_curSetp == step) return;
        _curSetp = step;
        for (int i = 0; i < effectScripts.Length; i++)
        {
            effectScripts[i].enabled = false;
        }

        switch (step)
        {
            case EffectStep.Gray:
                {
                    effectScripts[0].enabled = true;
                    break;
                }
            case EffectStep.LinerAndColor:
                {
                    effectScripts[1].enabled = true;
                    break;
                }
            case EffectStep.BlackLiner:
                {
                    effectScripts[1].enabled = true;
                    effectScripts[2].enabled = true;
                    break;
                }
            case EffectStep.OneColorLiner1:
                {
                    effectScripts[3].enabled = true;
                    break;
                }
            case EffectStep.OneColorLiner2:
                {
                    effectScripts[3].enabled = true;
                    effectScripts[4].enabled = true;
                    break;
                }
            case EffectStep.EndColorLiner:
                {
                    effectScripts[4].enabled = true;
                    effectScripts[5].enabled = true;
                    effectScripts[6].enabled = true;
                    break;
                }
        }

    }




}

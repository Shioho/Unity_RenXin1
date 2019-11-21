using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasMgr : MonoBehaviour
{
    public Text fpsInfo;
    public Text comboInfo;
    // Start is called before the first frame update
    public float updateInterval = 0.5F;
    private double _lastInterval;
    private int _frames = 0;
    private float _fps;

    private void Awake() {
        Application.targetFrameRate=60;  
    }

    void Start()
    {
        comboInfo.text = "";
        fpsInfo.text = "";

        _lastInterval = Time.realtimeSinceStartup;
        _frames = 0;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateFps();
    }
    public void UpdateCombo(int combo)
    {
        string info = "";
        if (combo != 0)
        {
            info = "Combo: " + combo.ToString();
        }
        comboInfo.text = info;
    }

    private void UpdateFps()
    {
        _frames++;
        float nowTime = Time.realtimeSinceStartup;
        if (nowTime > _lastInterval + updateInterval)
        {
            _fps = (float)(_frames / (nowTime - _lastInterval));
            _frames = 0;
            _lastInterval = nowTime;
        }
        fpsInfo.text = "FPS: " + _fps.ToString("f2");
    }


}

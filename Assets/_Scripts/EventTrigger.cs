﻿using BitBenderGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventTrigger : MonoBehaviour {
    string TAG = "EventTrigger==";
    GameObject mPointCube;

    public CanvasGameMenu mCanvasGameMenu;

    Vector3 mOriginalPosition;

    MobileTouchCamera mMobileTouchCamera;

    void Start () {
        mPointCube = GameObject.Find("Point");
        mOriginalPosition = mPointCube.transform.position;
        mMobileTouchCamera = GetComponent<MobileTouchCamera>();
    }
	
	void Update () {
        if (IsPointerOverGameObject(Input.mousePosition)) {
            if (mMobileTouchCamera.enabled) {
                mMobileTouchCamera.enabled = false;
            }
            return;
        }
        if (!mMobileTouchCamera.enabled) {
            mMobileTouchCamera.enabled = true;
        }
        RaycastHit hit;
        // 从鼠标所在的位置发射
        Vector2 screenPosition = Input.mousePosition;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(screenPosition), out hit)) {
            if (hit.collider.CompareTag("City")) {
                // 城市
                if (Input.GetMouseButtonUp(0)) {
                    mCanvasGameMenu.ShowCanvasGameMenu(screenPosition);
                    mCanvasGameMenu.SetCityName(hit.collider.GetComponentInChildren<TextMesh>().text, hit.collider.gameObject);
                }
                return;
            }
            // 格子
            float x = Mathf.Ceil(hit.point.x);
            float z = 0;
            if (x % 2 == 0) {
                z = Mathf.Ceil(hit.point.z);
                mPointCube.transform.position = new Vector3(x - 0.5f, mOriginalPosition.y, z - 0.5f);
            } else {
                z = Mathf.Ceil(hit.point.z - 0.5f);
                mPointCube.transform.position = new Vector3(x - 0.5f, mOriginalPosition.y, z);
            }
            if (Input.GetMouseButtonUp(0) && Wujiang.sCurrentWujiang!=null) {
                Wujiang.sCurrentWujiang.transform.position = new Vector3(mPointCube.transform.position.x, Wujiang.sCurrentWujiang.transform.position.y, mPointCube.transform.position.z);
                //Wujiang.sCurrentWujiang.OnMouseDown();
            }
        }

        if (Input.GetMouseButtonDown(0)) {
            mCanvasGameMenu.gameObject.SetActive(false);
        }
    }

    public bool IsPointerOverGameObject(Vector2 screenPosition) {
        //实例化点击事件  
        PointerEventData eventDataCurrentPosition = new PointerEventData(UnityEngine.EventSystems.EventSystem.current);
        //将点击位置的屏幕坐标赋值给点击事件  
        eventDataCurrentPosition.position = new Vector2(screenPosition.x, screenPosition.y);

        List<RaycastResult> results = new List<RaycastResult>();
        //向点击处发射射线  
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        return results.Count > 0;
    }
}

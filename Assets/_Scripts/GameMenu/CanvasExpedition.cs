﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasExpedition : MonoBehaviour {

    public GameObject mExpeditionMeun;

    public void ShowEcpedition() {
        mExpeditionMeun.SetActive(true);
    }

    public void HideEcpedition() {
        mExpeditionMeun.SetActive(false);
    }

}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Strategy : MonoBehaviour {
    public static List<General> GetExpeditionGenerals(List<General> list) {
        General general01 = null;
        General general02 = null;
        General general03 = null;
        if (BattleGameManager.sCurrentGenerals.Count >= 3) {
            general01 = BattleGameManager.sCurrentGenerals[Random.Range(0, BattleGameManager.sCurrentGenerals.Count)];
            while (true) {
                general02 = BattleGameManager.sCurrentGenerals[Random.Range(0, BattleGameManager.sCurrentGenerals.Count)];
                if (general02 != general01) {
                    break;
                }
            }
            while (true) {
                general03 = BattleGameManager.sCurrentGenerals[Random.Range(0, BattleGameManager.sCurrentGenerals.Count)];
                if (general03 != general01 && general03 != general02) {
                    break;
                }
            }
        } else if (BattleGameManager.sCurrentGenerals.Count == 2) {
            general01 = BattleGameManager.sCurrentGenerals[Random.Range(0, BattleGameManager.sCurrentGenerals.Count)];
            while (true) {
                general02 = BattleGameManager.sCurrentGenerals[Random.Range(0, BattleGameManager.sCurrentGenerals.Count)];
                if (general02 != general01) {
                    break;
                }
            }
        } else if (BattleGameManager.sCurrentGenerals.Count == 1) {
            general01 = BattleGameManager.sCurrentGenerals[0];
        }
        List<General> temp = new List<General>();
        temp.Add(general01);
        temp.Add(general02);
        temp.Add(general03);
        return temp;
    }
}
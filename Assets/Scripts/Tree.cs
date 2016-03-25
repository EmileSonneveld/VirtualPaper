using UnityEngine;
using System.Collections;

public class Tree : MonoBehaviour {

    public int nFruit = 5;

	public int nBCutting;
    public int cuttingProgress;

    public Transform p1;
    public Transform p2;
    public Transform p3;

    public Transform cutter1;
    public Transform cutter2;

    public int getCuttingProgress() {
        return cuttingProgress;
    }

    public void setCuttingProgress(int value) {
        cuttingProgress = value;
    }

    public void incrassCuttingProgress(int value) {
        cuttingProgress += value;
    }

}

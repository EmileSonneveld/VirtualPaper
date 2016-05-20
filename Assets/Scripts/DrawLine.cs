using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class DrawLine : MonoBehaviour {

	void Update ()
    {
        for (int i = 0; i < transform.childCount - 1; i++)
            Debug.DrawLine(transform.GetChild(i).position, transform.GetChild(i+1).position, Color.blue);
	}
}

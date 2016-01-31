using UnityEngine;
using System.Collections;

public class House : MonoBehaviour {

    public int nPeople;
    public bool deplacing;
    public int houseFood;

    public int BHomOnHome = 0;

    public Transform p1;
    public Transform p2;

	private float timer = 15;
	private float maxTimer = 15;
	/*

	void FixedUpdate()
	{
		if ((p1 != null) && (p2 != null)) {
			if ((p1.GetComponent<BHom> ().hisTree != null) && (p2.GetComponent<BHom> ().hisTree != null)) {
				if (timer > 0) {
					timer -= Time.fixedDeltaTime;
				} else {
					GameObject newBHom = Instantiate (p1, transform.GetChild (0).position, Quaternion.Euler (0, 0, 0)) as GameObject;
					newBHom.transform.parent = p1.parent.transform;
					timer = maxTimer;
				}
			}
		}


	}*/

}

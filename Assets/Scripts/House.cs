using UnityEngine;
using System.Collections;

public class House : MonoBehaviour {

    public int nPeople;
    public bool deplacing;
    public int houseFood;

    public int BHomOnHome = 0;


    public Sprite defaultH;  //---List of Sprite for different stat of House---
    public Sprite oneBH;
    public Sprite twoBH;

    public Transform p1;
    public Transform p2;

    public float timer = 0;

    private bool animating;

    public void SetAnimation(bool value)
    {
        animating = value;
    }

    void FixedUpdate()
    {
        if (animating)
        {
            if (transform.eulerAngles.x < 355)
            {
                if (timer > 0.01f)
                {
                    timer = 0;
                    transform.Rotate(5, 0, 0);                    
                }
                else timer += Time.deltaTime;
            }
            else animating = false;
        }

        if ((p1 != null) && (p2!=null))
        {
            if (transform.GetChild(1).GetChild(0).GetComponent<SpriteRenderer>().sprite != twoBH)
                transform.GetChild(1).GetChild(0).GetComponent<SpriteRenderer>().sprite = twoBH;
        }
        else if ((p1 != null) || (p2 != null))
        {
            if (transform.GetChild(1).GetChild(0).GetComponent<SpriteRenderer>().sprite != oneBH)
                transform.GetChild(1).GetChild(0).GetComponent<SpriteRenderer>().sprite = oneBH;
        }
        else if ((p1 == null) && (p2 == null))
        {
            if (transform.GetChild(1).GetChild(0).GetComponent<SpriteRenderer>().sprite != defaultH)
                transform.GetChild(1).GetChild(0).GetComponent<SpriteRenderer>().sprite = defaultH;
        }
    }


	/*void Update()
	{
		if ((p1 != null) && (p2 != null)) {
			if ((p1.GetComponent<BHom> ().hisTree != null) && (p2.GetComponent<BHom> ().hisTree != null)) {
				
				if (timer > 0) {
					timer -= Time.deltaTime;
				} else if (timer <= 0){
					GameObject newG = null;
					newG = Instantiate (p1.gameObject, transform.GetChild (0).GetChild (0).position, Quaternion.Euler (0, 0, 0)) as GameObject;
					newG.transform.localScale = new Vector3 (0.02f, 0.02f, 0.02f);

					newG.GetComponent<BHom> ().hisTree = null;
					newG.GetComponent<BHom> ().hisHouse = null;
					newG.transform.parent = p1.parent;
					timer = maxTimer;	

					GameObject.FindGameObjectWithTag ("God").GetComponent<CheckUp> ().checkPerso ();
				}
			}
		}
	}*/
}


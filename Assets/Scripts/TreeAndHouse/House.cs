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
}


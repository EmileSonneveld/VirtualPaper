using UnityEngine;
using System.Collections;

public class Tree2House : MonoBehaviour {

    public Transform house;

    public bool alrFall;

    private float timer;

    private bool animating;
    public float rotx;

    public void SetAnimation(bool value)
    {
        alrFall = true;
        transform.Rotate(-5, 0, 0);
        animating = value;
    }

    void Update()
    {
        if (animating)
        {
            if ((transform.eulerAngles.x > 270))
            {
                if (timer > 0.01f)
                {
                    timer = 0;
                    transform.Rotate(-5, 0, 0);
                }
                else timer += Time.deltaTime;
            }
            else
            {
                animating = false;
                tree2house();
            }
        }
    }

    public void tree2house()  //-----Instance a house when a tree is fallen-----
    {
        GameObject newHouse;
        newHouse = Instantiate(house.gameObject, new Vector3(transform.position.x, house.position.y, transform.position.z), Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z)) as GameObject;
        newHouse.transform.parent = house.parent;

        //newHouse.GetComponent<Animator>().enabled = true;
        newHouse.GetComponent<House>().SetAnimation(true);

        newHouse.GetComponent<House>().p1 = transform.GetComponent<Tree>().cutter1;
        transform.GetComponent<Tree>().cutter1.GetComponent<BHomInfo>().actionToDo = 1;
        transform.GetComponent<Tree>().cutter1.GetComponent<BHomInfo>().cutting = false;
        transform.GetComponent<Tree>().cutter1.GetComponent<BHomInfo>().cutter = false;
        transform.GetComponent<Tree>().cutter1.GetChild(0).GetComponent<Animator>().Play("brun_neutre");
        transform.GetComponent<Tree>().cutter1.GetChild(0).GetChild(0).GetComponent<Animator>().Play("brun_neutre");
        transform.GetComponent<Tree>().cutter1.GetComponent<BHomInfo>().hisHouse = newHouse.transform;
        transform.GetComponent<Tree>().cutter1.GetComponent<BHomInfo>().hisTreeCut = null;

        if (transform.GetComponent<Tree>().cutter2 != null)
        {
            
            newHouse.GetComponent<House>().p2 = transform.GetComponent<Tree>().cutter2;
            transform.GetComponent<Tree>().cutter2.GetComponent<BHomInfo>().actionToDo = 1;
            transform.GetComponent<Tree>().cutter2.GetComponent<BHomInfo>().cutting = false;
            transform.GetComponent<Tree>().cutter2.GetComponent<BHomInfo>().cutter = false;
            transform.GetComponent<Tree>().cutter2.GetChild(0).GetComponent<Animator>().Play("brun_neutre");
            transform.GetComponent<Tree>().cutter2.GetChild(0).GetChild(0).GetComponent<Animator>().Play("brun_neutre");
            transform.GetComponent<Tree>().cutter2.GetComponent<BHomInfo>().hisHouse = newHouse.transform;
            transform.GetComponent<Tree>().cutter2.GetComponent<BHomInfo>().hisTreeCut = null;
        }
        else newHouse.GetComponent<House>().p2 = null;

        newHouse.transform.GetComponent<placeByGod>().setPlaceByGod(false);

        Destroy(gameObject);
    }
}

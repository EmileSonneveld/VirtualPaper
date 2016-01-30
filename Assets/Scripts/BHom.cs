using UnityEngine;
using System.Collections;

public class BHom : MonoBehaviour {

    public bool believe;

    public Transform hisHouse;
    public Transform hisTree;

    public GameObject logoHeart;
    public GameObject logoHungry;
    public GameObject logoHouse;
    public GameObject logoWhat;

    public Transform house;
    public Transform listTree;

    public bool needHungry;
    public bool needHouse;

    private float timerHungry = 10;
    private float timerHouse = 10;

    private bool arlWait = false;

    private NavMeshAgent nMA;


    void Start () {
        nMA = GetComponent<NavMeshAgent>();
    }
	
	void FixedUpdate () {
        if ((needHungry) && (timerHungry > 0))
            timerHungry -= Time.deltaTime;
        else if (!believe)
        {
            int lastelement = 0;
            for (int i = 0; i < transform.parent.childCount; i++)
            {
                if (/*transform.parent.GetChild(i).GetComponent<BHom>().believe &&*/
                    ((Vector3.Distance(transform.parent.GetChild(i).position, transform.position)) <
                    (Vector3.Distance(transform.parent.GetChild(lastelement).position, transform.position))))
                    lastelement = i;
            }

            //nMA
        }
        else if (believe)
        {
            int lastelement = 0;
            for (int i = 0; i < transform.parent.childCount; i++)
            {
                if (!transform.parent.GetChild(i).GetComponent<BHom>().believe &&
                    ((Vector3.Distance(transform.parent.GetChild(i).position, transform.position)) <
                    (Vector3.Distance(transform.parent.GetChild(lastelement).position, transform.position))))
                    lastelement = i;
            }
        }
        if (needHouse) 
            if (timerHouse > 0)
                timerHouse -= Time.deltaTime;
            else if (!believe)
            {
                timerHouse = 5;
                int lastelement = 0;
                for (int i = 0; i < listTree.childCount; i++)
                {
                    if ((Vector3.Distance(listTree.GetChild(i).position, transform.position)) <
                         (Vector3.Distance(listTree.GetChild(lastelement).position, transform.position)))
                        lastelement = i;
                }

                if (Vector3.Distance(listTree.GetChild(lastelement).position, transform.position) > 1)
                    nMA.SetDestination(listTree.GetChild(lastelement).GetChild(0).position);
                else
                {
                    nMA.Stop();
                    GameObject newHouse;
                    newHouse = Instantiate(house.gameObject, listTree.GetChild(lastelement).position, Quaternion.Euler(0, 0, 0)) as GameObject;
                    newHouse.transform.parent = house.parent;
                    newHouse.GetComponent<House>().p1 = null;
                    newHouse.GetComponent<House>().p2 = null;
                    DestroyImmediate(listTree.GetChild(lastelement).gameObject);

                    for (int i = 0; i < transform.parent.childCount; i++)
                    {
                        transform.parent.GetChild(i).GetComponent<BHom>().needHouse = false;

                        for (int j = 0; j < transform.parent.GetChild(j).GetChild(1).childCount; j++)
                        {
                            Destroy(transform.parent.GetChild(j).GetChild(1).GetChild(0).gameObject);
                        }
                    }
                    timerHouse = 20;
                }
            }
            else if (believe)
            {
                if (!arlWait)
                {
                    timerHouse = 15;
                    arlWait = true;
                }
                else believe = false;                 
                //-----------Animm Pière---------
            } 
        



    }
}

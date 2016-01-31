using UnityEngine;
using System.Collections;

public class CheckUp : MonoBehaviour {

    public Transform listHouse;
    public Transform listTree;
    public Transform listBHom;

    public int nHouse;
    public int nTree;
    public int nBHom;

    void Start() {
        nHouse = listHouse.childCount;
        nTree = listTree.childCount;
        nBHom = listBHom.childCount;

        checkPerso();
    }

    void FixedUpdate() {

        //checkPerso();

    }

    public void checkPerso()
    {
        nHouse = listHouse.childCount;
        nTree = listTree.childCount;
        nBHom = listBHom.childCount;

        //bool alrLogo = false;
        for (int i = 0; i < listBHom.childCount; i++)
        {
            if (listBHom.GetChild(i).GetComponent<BHom>().hisHouse == null)
            {
                if ((!checkHouse(i, listHouse)) && (!listBHom.GetChild(i).GetComponent<BHom>().needHouse))
                {
					if (listBHom.GetChild (i).GetChild (0).GetChild (4).childCount <= 0) {
						GameObject logo = Instantiate (listBHom.GetChild (i).GetComponent<BHom> ().logoHouse, listBHom.GetChild (i).GetChild (0).GetChild (4).position, Quaternion.Euler (0, 0, 0)) as GameObject;
						logo.transform.localScale = new Vector3 (0.1f, 0.1f, 0.1f) / 10;
						logo.transform.parent = listBHom.GetChild (i).GetChild (0).GetChild (4);
						//alrLogo = true;

						listBHom.GetChild (i).GetComponent<BHom> ().needHouse = true;
					}
                }
            }
            if (listBHom.GetChild(i).GetComponent<BHom>().hisTree == null)
            {
                if ((!checkTree(i, listTree)) && (!listBHom.GetChild(i).GetComponent<BHom>().needHungry))
                {
					if (listBHom.GetChild (i).GetChild (0).GetChild (4).childCount <= 0) {
						GameObject logo = Instantiate (listBHom.GetChild (i).GetComponent<BHom> ().logoHungry, listBHom.GetChild (i).GetChild (0).GetChild (4).position, Quaternion.Euler (0, 0, 0)) as GameObject;
						/*if (alrLogo)
                    {
                        logo.transform.localPosition = new Vector3(listBHom.GetChild(i).GetChild(1).position.x + 1, listBHom.GetChild(i).GetChild(1).position.y, listBHom.GetChild(i).GetChild(1).position.z);
                        logo.transform.rotation = Quaternion.Euler(0, 180, 0);
                    }*/
						logo.transform.localScale = new Vector3 (0.1f, 0.1f, 0.1f) / 10;
						logo.transform.parent = listBHom.GetChild (i).GetChild (0).GetChild (4);

						listBHom.GetChild (i).GetComponent<BHom> ().needHungry = true;
					}
                }
            }
        }
    }

    bool checkHouse(int i, Transform list)
    {
        int j = 0;
        bool find = false;       

        do
        {
            if (list.GetChild(j).GetComponent<House>().p1 == null)
            {
                
				listBHom.GetChild(i).GetComponent<BHom>().hisHouse = list.GetChild(j);
                list.GetChild(j).GetComponent<House>().p1 =listBHom.GetChild(i);
				if (!listBHom.GetChild(i).GetComponent<BHom>().victime){
				listBHom.GetChild(i).GetComponent<BHom>().nMA.SetDestination(listBHom.GetChild(i).GetComponent<BHom>().hisHouse.GetChild(0).position);
				}
                find = true;
            }else if (list.GetChild(j).GetComponent<House>().p2 == null)
            {
                listBHom.GetChild(i).GetComponent<BHom>().hisHouse = list.GetChild(j);
                list.GetChild(j).GetComponent<House>().p2 = listBHom.GetChild(i);
				if (!listBHom.GetChild(i).GetComponent<BHom>().victime){
					listBHom.GetChild(i).GetComponent<BHom>().nMA.SetDestination(listBHom.GetChild(i).GetComponent<BHom>().hisHouse.GetChild(0).position);
				}
                find = true;
            }
            j++;
        } while ((j < list.childCount) && !find);
        return find;
    }

    bool checkTree(int i, Transform list)
    {
        int j = 0;
        bool find = false;

        do
        {
            if (list.GetChild(j).GetComponent<Tree>().p1 == null)
            {
                listBHom.GetChild(i).GetComponent<BHom>().hisTree = list.GetChild(j);
                list.GetChild(j).GetComponent<Tree>().p1 = listBHom.GetChild(i);
				if (!listBHom.GetChild(i).GetComponent<BHom>().victime){
					listBHom.GetChild(i).GetComponent<BHom>().nMA.SetDestination(listBHom.GetChild(i).GetComponent<BHom>().hisTree.GetChild(0).position);
				}
                find = true;
            }
            else if (list.GetChild(j).GetComponent<Tree>().p2 == null)
            {
                listBHom.GetChild(i).GetComponent<BHom>().hisTree = list.GetChild(j);
                list.GetChild(j).GetComponent<Tree>().p2 = listBHom.GetChild(i);
				if (!listBHom.GetChild(i).GetComponent<BHom>().victime){
					listBHom.GetChild(i).GetComponent<BHom>().nMA.SetDestination(listBHom.GetChild(i).GetComponent<BHom>().hisTree.GetChild(0).position);
				}
                find = true;
            }
            else if (list.GetChild(j).GetComponent<Tree>().p3 == null)
            {
                listBHom.GetChild(i).GetComponent<BHom>().hisTree = list.GetChild(j);
                list.GetChild(j).GetComponent<Tree>().p3 = listBHom.GetChild(i);
				if (!listBHom.GetChild(i).GetComponent<BHom>().victime){
					listBHom.GetChild(i).GetComponent<BHom>().nMA.SetDestination(listBHom.GetChild(i).GetComponent<BHom>().hisTree.GetChild(0).position);
				}
                find = true;
            }
            j++;
        } while ((j < list.childCount) && !find);
        return find;
    }

	public void removeBul()
	{
		for (int i = 0; i < listBHom.childCount; i++) {
			listBHom.GetChild (i).GetComponent<BHom> ().needHouse = false;
			listBHom.GetChild (i).GetComponent<BHom> ().needHungry = false;
			for (int j = 0; j < listBHom.GetChild (i).GetChild (0).GetChild (4).childCount; j++) {  //destroy info bul
				Destroy (listBHom.GetChild (i).GetChild (0).GetChild (4).GetChild (0).gameObject);									
			}
		}
	}
}

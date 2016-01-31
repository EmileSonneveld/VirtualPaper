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

    public Transform monument;
    public Transform house;
    public Transform listTree;

    public bool needHungry;
    public bool needHouse;

    private float timerHungry = 10;
    private float timerHouse = 10;

    private bool arlWait = false;
    private bool choseBHom = false;
    private bool moveAgain = false;

	public bool cueillette = false;
	public bool pick = false;
	public bool cut = false;
	public bool jetter = false;
	public bool victime = false;

	public string prefixe;
	public string fixe;

	private Animator anim;
	private Animator animBack;
    private NavMeshAgent nMA;

	private bool alrRestartCuting = false;

    void Start () {
		nMA = transform.GetComponent<NavMeshAgent>();
		anim = transform.GetChild(0).GetComponent<Animator> ();
		animBack = transform.GetChild(0).GetComponent<Animator> ();
		AniamtionBHom ();
    }
	
	void FixedUpdate () {

		GameObject.FindGameObjectWithTag ("God").GetComponent<CheckUp> ().checkPerso ();
		AniamtionBHom ();

		if (!victime) {
			if (needHouse) {
				int lastelement = 0;
				if (timerHouse > 0)
					timerHouse -= Time.deltaTime;
				else if (!believe) {
					if (!alrRestartCuting) {
						timerHouse = 10;
						alrRestartCuting = true;


						if ((Vector3.Distance (hisTree.GetChild(0).position, transform.position) > 1)) {
							nMA.SetDestination (hisTree.GetChild (0).position);
						} else {
							//transform.position = listTree.GetChild (lastelement).GetChild (0).position;
							anim.Play ("brun_coupe");
							animBack.Play ("brun_coupe");
						}
					}
					if (timerHouse <= 0) {
						//int lastelement = 0;
						if ((Vector3.Distance (hisTree.GetChild(0).position, transform.position) > 1)) {
							nMA.SetDestination (hisTree.GetChild (0).position);
						} else {
							nMA.Stop ();
							//moveAgain = true;
							GameObject newHouse;
							newHouse = Instantiate (house.gameObject, listTree.GetChild (lastelement).position, Quaternion.Euler (0, 0, 0)) as GameObject;
							newHouse.transform.parent = house.parent;
							newHouse.GetComponent<House> ().p1 = null;
							newHouse.GetComponent<House> ().p2 = null;

							DestroyImmediate (listTree.GetChild (lastelement).gameObject);

							for (int i = 0; i < transform.parent.childCount; i++) {
								transform.parent.GetChild (i).GetComponent<BHom> ().needHouse = false;

							for (int j = 0; j < transform.parent.GetChild (i).GetChild (0).GetChild (1).childCount; j++) {  //destroy info bul
								Destroy (transform.parent.GetChild (i).GetChild (0).GetChild (1).GetChild (0).gameObject);
									
								}
							}

							anim.Play ("brun_neutre");
							animBack.Play ("brun_neutre");
							timerHouse = 20;
							alrRestartCuting = false;
							GameObject.FindGameObjectWithTag ("God").GetComponent<CheckUp> ().checkPerso ();
						}
					}
				} else if (believe) {
					if (!arlWait) {
						timerHouse = 15;
						arlWait = true;
						anim.Play ("violet_priere");
						animBack.Play ("violet_priere");
					} else {
						believe = false;
						timerHouse = 3;
						AniamtionBHom ();
					}
				}
			} else if (needHungry) { //Si ils ont faim qu'ils n'ont plus d'arbre
				if (timerHungry > 0)
					timerHungry -= Time.deltaTime;
				else if (!believe) {
					int lastelement = 0;
					if (!choseBHom) {
						choseBHom = true;

						for (int i = 0; i < transform.parent.childCount; i++) {
						if (/*transform.parent.GetChild(i).GetComponent<BHom>().believe &&*/ (!transform.parent.GetChild (i).GetComponent<BHom> ().pick) &&
							(!transform.parent.GetChild (i).GetComponent<BHom> ().victime) &&
								((Vector3.Distance (transform.parent.GetChild (i).position, transform.position)) <
								(Vector3.Distance (transform.parent.GetChild (lastelement).position, transform.position))))
								lastelement = i;
								
						}
					}
					if (Vector3.Distance (transform.parent.GetChild (lastelement).position, transform.position) > 1)
						nMA.SetDestination (transform.parent.GetChild (lastelement).position);
				else if ((transform.parent.GetChild (lastelement).GetChild(1).childCount) < 1){
						nMA.Stop ();
						pick = true;
						anim.Play ("brun_pickup");
						animBack.Play ("brun_pickup");
						transform.parent.GetChild (lastelement).GetComponent<BHom> ().victime = true;
						transform.parent.GetChild (lastelement).position = transform.GetChild (1).GetChild (0).position;
						transform.parent.GetChild (lastelement).parent = transform.GetChild (0).GetChild (1);
						//if (the BHom is taken) go to the end of the desk

					}

				} else if (believe) {
					int lastelement = 0;
					for (int i = 0; i < transform.parent.childCount; i++) {
					if (((!transform.parent.GetChild (i).GetComponent<BHom> ().believe) && (!transform.parent.GetChild (i).GetComponent<BHom> ().pick)) &&
						(!transform.parent.GetChild (i).GetComponent<BHom> ().victime) &&
						((Vector3.Distance (transform.parent.GetChild (i).position, transform.position)) <
		                    (Vector3.Distance (transform.parent.GetChild (lastelement).position, transform.position))))
							lastelement = i;
							pick = true;
					}
					if (Vector3.Distance (transform.parent.GetChild (lastelement).position, transform.position) > 1){
						nMA.SetDestination (transform.parent.GetChild (lastelement).position);
					}
					else {
							nMA.Stop ();
							anim.Play ("violet_pickup");
							animBack.Play ("violet_pickup");
							transform.parent.GetChild (lastelement).GetComponent<BHom> ().victime = true;
						transform.parent.GetChild (lastelement).position = transform.GetChild (0).GetChild(1).position;
						transform.parent.GetChild (lastelement).parent = transform.GetChild (0).GetChild (1);
						}
				}
			} else {
				GameObject.FindGameObjectWithTag ("God").GetComponent<CheckUp> ().checkPerso ();
				Move3();
			}  
		}else{
			if (believe) {
				anim.Play ("violet_victime");
				animBack.Play ("violet_victime");
			} else {
				anim.Play ("brun_victime");
				animBack.Play ("brun_victime");
			}

    	}
	}
	
    public bool transportingFood;

    public int target;

    public float maxTime = 3;
    private float time;

    private void Move3()
    {
		if (((Vector3.Distance(transform.position, hisHouse.GetChild(0).position)) < 0.005 ) /*&& (target == 0)*/)
        {
            nMA.SetDestination(hisTree.GetChild(0).position);
			target = 1;
        }/*
        else if (believe)           
        {
            if ((Vector3.Distance(transform.position, hisTree.GetChild(0).position)) < 1)
                nMA.SetDestination(monument.GetChild(0).position);
            else if ((Vector3.Distance(transform.position, monument.GetChild(0).position)) < 1)
                    nMA.SetDestination(hisHouse.GetChild(0).position);
        }*/
        else            
        {
			if (((Vector3.Distance(transform.position, hisTree.GetChild(0).position)) < 0.005) /*&& (target == 1)*/){
                nMA.SetDestination(hisHouse.GetChild(0).position);
				target = 0;
			}
        }
    }

	void AniamtionBHom ()
	{
		if (believe) {
			prefixe = "violet_";
		} else {
			prefixe = "brun_";
		}
		if (needHungry && needHouse) {
			fixe = "enerve";
		} else if (needHouse) {
			fixe = "neutre";
		} else if (needHungry) {
			fixe = "neutre";
		} else if (!needHungry && !needHouse){
			fixe = "content";
		}
		if (!pick && !cut && !cueillette && !jetter) {
			if (nMA.velocity.x > 0.1) {
				anim.Play (prefixe + fixe + "_marche");
				animBack.Play (prefixe + fixe + "_marche");
			} else if (!needHungry && !needHouse){
				anim.Play (prefixe + fixe);
				animBack.Play (prefixe + fixe);
			}
		} else if (pick) {
			if (nMA.velocity.x > 0.1) {
				anim.Play (prefixe + "pickup_" + "marche");
				animBack.Play (prefixe + "pickup_" + "marche");
			} else {
				anim.Play (prefixe + "pickup");
				animBack.Play (prefixe + "pickup");
			}
		} else if (cut) {
			anim.Play (prefixe + "coupe");
			animBack.Play (prefixe + "coupe");
		}
		else if (cueillette) {
			anim.Play (prefixe + "cueillette");
			animBack.Play (prefixe + "cueillette");
		}
		else if (jetter) {
			anim.Play (prefixe + "jetter");
			animBack.Play (prefixe + "jetter");
		}
	}
}

using UnityEngine;
using System.Collections;

public class ThrowAnimation : MonoBehaviour {

    public Transform bhom;
    public float speed, l;

    private int currentNode = 1;

	void Update ()
    {
	    if (bhom != null)
        {
            if (Vector3.Distance(bhom.position, transform.GetChild(currentNode).position) < Time.deltaTime * -speed * l)
            {
                if (currentNode < transform.childCount - 1)
                    currentNode++;
                else
                {
                    bhom.GetComponent<BHomInfo>().hisBHomMurder.GetComponent<BHomInfo>().isAMurder = true;
                    Destroy(bhom.gameObject);
                    currentNode = 1;
                }
            }
            else
            {
                Vector3 direction = bhom.position - transform.GetChild(currentNode).position;
                direction =  direction.normalized;
                bhom.Translate(direction * Time.deltaTime * speed);
            }
        }
	}
}

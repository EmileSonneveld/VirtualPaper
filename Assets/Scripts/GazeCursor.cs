using UnityEngine;
using System.Collections;

public class GazeCursor : MonoBehaviour {

	public Transform MainCamera;		//the anchor for where we're looking
	public float range = 100f;

	//raycasting
	public Ray ray; 
	public RaycastHit hit; 
	[HideInInspector] public bool rayHit = false;
	[HideInInspector] public bool rayHitInteractive = false;
	[HideInInspector] public GameObject hitObject;
	[HideInInspector] public string hitTag;
	[HideInInspector] public float hitDistance;
	[HideInInspector] public Vector3 hitNormal; //the normal of the ray
	[HideInInspector] public Vector3 focusPoint; //the ray hitpoint

	//cursor
	private GameObject cursor;
	private SpriteRenderer cursorSprite;

	private Vector3 originalScale;				//original scale of cursor
	private float tempDistForStandardCursor = 4f;
	private Transform head;

	// Use this for initialization
	void Start () {
		cursor = GameObject.FindWithTag("Cursor");
		cursorSprite = cursor.GetComponent <SpriteRenderer>();

		GameObject headGO = GameObject.FindGameObjectWithTag("Head"); //locates the player
		head = headGO.transform;
		originalScale = transform.localScale;
		
		hitTag = "Untagged";
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		//setup raycast
		ray.origin = MainCamera.position;
		ray.direction = MainCamera.forward; 
		
		rayHit = false;
		rayHitInteractive = false;
		
		if (Physics.Raycast (ray, out hit, range)) {
			rayHit = true;
			hitObject = hit.transform.gameObject;
			hitTag = hitObject.tag;
			hitDistance = hit.distance;
			
			if(hitTag!="Untagged")
				rayHitInteractive = true;
			//Debug.Log (rayHitInteractive +"-"+hit.transform.gameObject.tag );
			focusPoint = hit.point;

			hitNormal = hit.normal;
		} else{
			//set focuPoint at fixed distance when looking at the sky
			focusPoint = MainCamera.forward * 5f;
		}
			
		scale ();

		//set cursor at focuspoint
		cursor.transform.position = focusPoint;
	}

	private void scale()
	{
		/*float dist = Vector3.Distance (head.position, cursor.transform.position);
		//Debug.Log (dist);
		float factor = dist / sizeDistOrigin;
		cursor.transform.localScale = originalScale*factor*0.7f;*/
	}
}

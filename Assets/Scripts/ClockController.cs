using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;

public class ClockController : MonoBehaviour {

	[Header("Quantities")]
	public int _Population;
	public int _Housing;
	public int _Trees;

	[Header("Alarms")]
	public UnityEvent _HousingShortageEvent;
	public UnityEvent _FoodShortageEvent;
	private bool _HousingShortage = false;
	private bool _FoodShortage = false;

	[Header("Text References")]
	public Text _PopulationText;
	public Text _HousingText;
	public Text _TreesText;
	public Text _Date;
	public Text _Time;

	void Start () {
		InvokeRepeating ("UpdateHour", 0, 1f);
		InvokeRepeating ("UpdateStats", 0, 0.5f);
	}

	/*STATS*/

	private void UpdateStats(){
		//get current number of people and trees
		int pplPerHouse = 2;
		int pplPerTree = 3;
		int houses = 3;
		int trees = 4;
		_Housing = pplPerHouse * houses;
		_Trees = pplPerTree * trees;

		_PopulationText.text = AddZero(Mathf.Clamp(_Population, 0, 99f)); 
		_HousingText.text = AddZero(Mathf.Clamp(_Housing, 0, 99f));
		_TreesText.text = AddZero(Mathf.Clamp(_Trees, 0, 99f));

		if (!_HousingShortage && _Population > _Housing) {
			_HousingShortage = true;
		}
		if (_HousingShortage && _Population <= _Housing) {
			_HousingShortage = false;
		}

		if (!_FoodShortage && _Population > _Trees) {
			_FoodShortage = true;
		}
		if (_FoodShortage && _Population <= _Trees) {
			_FoodShortage = false;
		}
	}

	/*DATE & TIME*/

	private void UpdateHour(){
		_Date.text = System.DateTime.Now.Day+" "+ GetMonthString(System.DateTime.Now.Month)+" "+System.DateTime.Now.Year;
		string separator = (_Time.text.Contains(":")) ? " " : ":" ;
		_Time.text = AddZero(System.DateTime.Now.Hour ) + separator + AddZero(System.DateTime.Now.Minute) + " " + GetAmPm(System.DateTime.Now.Hour);
	}

	private string GetAmPm(int m){
		if (m > 12) {
			return "PM";
		} else {
			return "AM";
		}
	}

	private string AddZero(float I){
		return (I < 10) ? "0" + I : I+"";
	}

	private string GetMonthString(int m){
		switch (m) {
		case 1:
			return "jan";
		case 2:
			return "feb";
		case 3:
			return "mar";
		case 4:
			return "apr";
		case 5:
			return "may";
		case 6:
			return "jun";
		case 7:
			return "jul";
		case 8:
			return "aug";
		case 9:
			return "sep";
		case 10:
			return "okt";
		case 11:
			return "nov";
		case 12:
			return "dec";
		}

		return "jan";
	}
}

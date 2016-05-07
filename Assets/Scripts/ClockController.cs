using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;

public class ClockController : MonoBehaviour {

	public AgeOfPaperManage _God;

	[Header("QUANTITIES")]
	public int _Population;
	public int _Housing;
	public int _Trees;

	[Header("ALARMS")]
	public Material _NeutralMat;
	public Material _RedMat;
	public UnityEvent _HousingShortageEvent;
	public UnityEvent _FoodShortageEvent;
	private bool _HousingShortage = false;
	private bool _FoodShortage = false;
	private bool _IsRed = false;

	[Header("TEXT REFERENCES")]
	public Text _PopulationText;

	public Image _HouseIcon;
	public Text _NHousingText;
	public Text _HousingText;

	public Image _TreeIcon;
	public Text _NTreesText;
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
		int houses = _God.nHouse;
		int trees = _God.nTree;
		_Population = _God.nBHom;
		_Housing = pplPerHouse * houses;
		_Trees = pplPerTree * trees;

		//set UI text
		_PopulationText.text = AddZero(Mathf.Clamp(_Population, 0, 99f));
		_NHousingText.text = AddZero(Mathf.Clamp(houses, 0, 99f));
		_HousingText.text = AddZero(Mathf.Clamp(_Housing, 0, 99f));
		_NTreesText.text = AddZero(Mathf.Clamp(trees, 0, 99f));
		_TreesText.text = AddZero(Mathf.Clamp(_Trees, 0, 99f));

		//check if there is housing or food shortage, invoke events if so
		if (!_HousingShortage && _Population > _Housing) {
			_HousingShortage = true;
			_HousingShortageEvent.Invoke ();
		}
		if (_HousingShortage && _Population <= _Housing) {
			_HousingShortage = false;
		}

		if (!_FoodShortage && _Population > _Trees) {
			_FoodShortage = true;
			_FoodShortageEvent.Invoke ();
		}
		if (_FoodShortage && _Population <= _Trees) {
			_FoodShortage = false;
		}

		//update text flikker
		if (_FoodShortage) {
			if(_IsRed){
				_NTreesText.material = _NeutralMat; 
				_TreeIcon.material = _NeutralMat; 
			} else{
				_NTreesText.material = _RedMat;
				_TreeIcon.material = _RedMat;
			}
		}
		if (_HousingShortage) {
			if(_IsRed){
				_NHousingText.material = _NeutralMat; 
				_HouseIcon.material = _NeutralMat; 
			} else{
				_NHousingText.material = _RedMat;
				_HouseIcon.material = _RedMat;
			}
		}
		_IsRed = !_IsRed;

	}

	/*DATE & TIME*/

	private void UpdateHour(){
		
		_Date.text = System.DateTime.Now.Day+" "+ GetMonthString(System.DateTime.Now.Month)+" "+System.DateTime.Now.Year;

		string separator = (_Time.text.Contains(":")) ? " " : ":" ;

		_Time.text = AddZero(PMCorrect(System.DateTime.Now.Hour) ) + separator + AddZero(System.DateTime.Now.Minute) + " " + GetAmPm(System.DateTime.Now.Hour);
	}

	private string GetAmPm(int h){
		if (h > 12) {
			return "PM";
		} else {
			return "AM";
		}
	}

	private int PMCorrect(int h){
		if (h > 12) {
			return h - 12;
		} else {
			return h;
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

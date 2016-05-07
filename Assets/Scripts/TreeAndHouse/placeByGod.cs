using UnityEngine;
using System.Collections;

public class placeByGod : MonoBehaviour {

    public bool placeByG;

    public void setPlaceByGod(bool value)
    {
        this.placeByG = value;
    }

    public bool getPlaceByGod()
    {
        return this.placeByG;
    }
}

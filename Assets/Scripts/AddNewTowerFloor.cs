using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddNewTowerFloor : MonoBehaviour
{
    //we will gonna use this to add new floor to the tower, top floor will remain top floor but new floor will be added to the bottom

    public GameObject towerFloorPrefab;

    public GameObject towerTopFloor;

    public List<GameObject> towerFloors;

    public int towerFloorCount = 0;

    public int towerFloorIndex = 0;

    public int towerFloorIndexForTopFloor = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            AddNewFloor();
            RiseTower();
        }
    }

    public void AddNewFloor()
    {
        towerFloorCount++;

        towerFloorIndex++;

        towerFloorIndexForTopFloor++;

        GameObject towerFloor = Instantiate(towerFloorPrefab, towerTopFloor.transform.position, Quaternion.identity);

        towerFloor.transform.SetParent(gameObject.transform);

        towerFloor.transform.localPosition = new Vector3(0, -towerFloorIndex * .33f, 0);

        towerFloor.transform.localScale = new Vector3(1, .33f, 1);

        towerFloor.name = "Tower Floor " + towerFloorIndex;

        towerTopFloor = towerFloor;

        towerFloors.Add(towerFloor);
        
    }

    public void RiseTower()
    {
        for (int i = 0; i < towerFloors.Count; i++)
        {
            towerFloors[i].transform.localPosition = new Vector3(0, -i * .33f, 0);
        }
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddNewTowerFloor : MonoBehaviour
{
    //we will gonna use this to add new floor to the tower, top floor will remain top floor but new floor will be added to the bottom

    public GameObject towerFloorPrefab;

    public GameObject towerTopFloor;

    public List<GameObject> towerFloors;

    [SerializeField]
    List<TurretData> turretDatas;

    public int towerFloorIndex = 0;

    public int maxTowerFloors = 4;

    public void Start()
    {
        towerFloors = new List<GameObject>();
        towerFloors.Add(towerTopFloor);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && !IsTowerFull())
        {
            PickRandomTurretData();
        }
    }

    void PickRandomTurretData()
    {
        int randomIndex = Random.Range(0, turretDatas.Count);
        AddNewFloor(turretDatas[randomIndex]);
    }

    public void AddNewFloor(TurretData turretData)
    {
        GameObject newTowerFloor = Instantiate(towerFloorPrefab,towerFloors[towerFloorIndex].transform.position,Quaternion.identity, this.transform);
        towerFloorIndex++;
        RiseTower();
        towerFloors.Add(newTowerFloor);
        SetTurretData(turretData, towerFloorIndex);
        newTowerFloor.name = "TowerFloor" + towerFloorIndex;
    }

    public void RiseTower()
    {
        foreach (GameObject towerFloor in towerFloors)
        {
            towerFloor.transform.localPosition = new Vector3(0, towerFloor.transform.localPosition.y + .66f, 0);

            if(towerFloor.transform.GetChild(0).TryGetComponent<AutoTurrets>(out AutoTurrets towerDefence))
            {
                towerDefence.SetRange(towerDefence.GetRange() * 1.2f);
            }
        }
    }

    public void SetTurretData(TurretData turretData, int towerFloorIndex)
    {
        towerFloors[towerFloorIndex].GetComponentInChildren<AutoTurrets>().InitializeTurret(turretData);
    }

    public bool IsTowerFull()
    {
        return towerFloors.Count >= maxTowerFloors;
    }
}

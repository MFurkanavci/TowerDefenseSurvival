using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerFloorManager : MonoBehaviour
{
    public GameObject towerFloorPrefab;

    public GameObject towerTopFloor;

    public List<GameObject> towerFloors;

    [SerializeField] private int towerHealth;

    [SerializeField]
    List<TurretData> turretDatas;

    [SerializeField]
    List<FloorData> floorDatas;

    public int towerFloorIndex = 0;

    public int maxTowerFloors = 4;

    public void Start()
    {
        towerFloors = new List<GameObject>();
        towerFloors.Add(towerTopFloor);
    }
    public TurretData PickRandomTurretData()
    {
        int randomIndex = Random.Range(0, turretDatas.Count);
        return turretDatas[randomIndex];
    }

    public FloorData PickRandomFloorData()
    {
        int randomIndex = Random.Range(0, floorDatas.Count);
        return floorDatas[randomIndex];
    }

    public void BuildFloor(FloorData floorData)
    {
        GameObject newTowerFloor = Instantiate(floorData.floorPrefab, towerFloors[towerFloorIndex].transform.position, Quaternion.identity, this.transform);
        towerFloorIndex++;
        RiseTower();
        towerFloors.Add(newTowerFloor);
        newTowerFloor.name = "TowerFloor" + towerFloorIndex;
    }

    public void RiseTower()
    {
        foreach (GameObject towerFloor in towerFloors)
        {
            towerFloor.transform.localPosition = new Vector3(0, towerFloor.transform.localPosition.y + 1, 0);

            if(towerFloor.transform.GetChild(0).TryGetComponent<AutoTurrets>(out AutoTurrets towerDefence))
            {
                towerDefence.SetRange(towerDefence.GetRange() * 1.2f);
            }
        }
    }

    public void SetTurretData(TurretData turretData, int towerFloorIndex)
    {
        towerFloors[towerFloorIndex].GetComponentInChildren<AutoTurrets>().InitializeTurret(turretData);
        towerFloors[towerFloorIndex].GetComponentInChildren<AutoTurrets>().enabled = true;
    }

    public bool IsTowerFull()
    {
        return towerFloors.Count >= maxTowerFloors;
    }

    public int TurretCount()
    {
        int turretCount = 0;
        foreach (GameObject towerFloor in towerFloors)
        {
            if (towerFloor.GetComponentInChildren<AutoTurrets>() != null)
            {
                turretCount++;
            }
        }
        return turretCount;
    }

    public bool FloorAvailable(int floorIndex)
    {
        return towerFloors[floorIndex].GetComponentInChildren<AutoTurrets>() == null;
    }

    public void RemoveTurret(int floorIndex)
    {
        Destroy(towerFloors[floorIndex].GetComponentInChildren<AutoTurrets>().gameObject);
    }

    public void ReCalculateHealth()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            BuildFloor(PickRandomFloorData());
        }
    }
}

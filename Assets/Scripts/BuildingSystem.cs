using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSystem : MonoBehaviour
{
    [SerializeField] private TowerResources towerResources;
    [SerializeField] private TowerFloorManager towerFloorManager;
    [SerializeField] private OutsideObjectsManager outsideObjectsManager;

    public void BuildTurret(TurretData turretData, int towerFloorIndex)
    {
        BuildingType turretType = turretData.buildingType;
        if (towerResources.HasResources(turretType) && !towerFloorManager.IsTowerFull() && towerFloorManager.FloorAvailable(towerFloorIndex))
        {
            towerResources.SpendResources(turretType);
            towerFloorManager.SetTurretData(turretData, towerFloorIndex);
        }
        else
        {
            Debug.Log("Not enough resources to build a turret.");
        }
    }

    public void BuildFloor(FloorData floorData)
    {
        BuildingType floorType = floorData.buildingType;
        if (towerResources.HasResources(floorType))
        {
            towerResources.SpendResources(floorType);
            towerFloorManager.BuildFloor(floorData);
        }
        else
        {
            Debug.Log("Not enough resources to build a floor.");
        }
    }

    public void BuildRadioTower(RadioTowerData radioTowerData)
    {
        BuildingType radioTowerType = radioTowerData.buildingType;
        if (towerResources.HasResources(radioTowerType))
        {
            towerResources.SpendResources(radioTowerType);
            outsideObjectsManager.BuildRadioTower(radioTowerData);
        }
        else
        {
            Debug.Log("Not enough resources to build a radio tower.");
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutsideObjectsManager : MonoBehaviour
{
    [SerializeField] private Transform radiotowerBuildPoint;

    public RadioTowerData radioTowerData;

    public void BuildRadioTower(RadioTowerData radioTowerData)
    {
        GameObject newRadioTower = Instantiate(radioTowerData.radioTowerPrefab, radiotowerBuildPoint.position, Quaternion.identity, radiotowerBuildPoint);
        newRadioTower.name = radioTowerData.name;
        newRadioTower.transform.position = new Vector3(newRadioTower.transform.position.x, 4.35f, newRadioTower.transform.position.z);
        newRadioTower.transform.rotation = Quaternion.Euler(0, 0, 90);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.V))
        {
            BuildRadioTower(radioTowerData);
        }
    }
}

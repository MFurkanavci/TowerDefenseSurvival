using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;

public class PlayerInandOut : MonoBehaviour
{
    public bool isInTrigger = false;
    public bool isInside = false;
    public GameObject player, TowerResourcesPanel;   
    public TowerResources towerResources;

    public MainTurretControls mainTurretControls;

    public TextMeshProUGUI infoText, resourceText;
    Vector3 tempPos;

    public CinemachineVirtualCamera insideCamera;

    private void Update()
    {
        if (isInTrigger)
        {
            if (isInside)
            {
                infoText.text = "Press \"Space\" to go outside";
            }
            else
            {
                infoText.text = "Press \"Space\" to go inside\nPress \"Q\" to bank resources";
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (isInside)
                {
                    isInside = false;
                    player.transform.position = tempPos;
                    player.SetActive(true);
                    mainTurretControls.SetIsInside(false);
                }
                else
                {
                    isInside = true;
                    tempPos = player.transform.position;
                    player.SetActive(false);
                    mainTurretControls.SetIsInside(true);

                }
                CameraControls();
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                foreach (ResourceTypes.Resources resource in System.Enum.GetValues(typeof(ResourceTypes.Resources)))
                {
                    towerResources.ModifyResource(resource, player.GetComponent<ResourceGathering>().GetResourceAmount(resource));
                }
                player.GetComponent<ResourceGathering>().ResetResource();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            TowerResourcesPanel.SetActive(true);
            isInTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            infoText.text = "";
            TowerResourcesPanel.SetActive(false);
            isInTrigger = false;
        }
    }

    public void CameraControls()
    {

        float insideCameraFOV = 90f;
        float outsideCameraFOV = 60f;

        float  insideCameraFollowOffsetx = 0f;
        float  insideCameraFollowOffsety = 25f;
        float  insideCameraFollowOffsetz = -8.5f;

        float outsideCameraFollowOffsetx = 0f;
        float outsideCameraFollowOffsety = 15f;
        float outsideCameraFollowOffsetz = -10f;

        if(isInside)
        {
            insideCamera.Follow = gameObject.transform.parent.transform;
            insideCamera.LookAt = gameObject.transform.parent.transform;
            insideCamera.m_Lens.FieldOfView = insideCameraFOV;
            insideCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = new Vector3(insideCameraFollowOffsetx, insideCameraFollowOffsety, insideCameraFollowOffsetz);
        }
        else
        {
            insideCamera.Follow = player.transform;
            insideCamera.LookAt = player.transform;
            insideCamera.m_Lens.FieldOfView = outsideCameraFOV;
            insideCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = new Vector3(outsideCameraFollowOffsetx, outsideCameraFollowOffsety, outsideCameraFollowOffsetz);
        }
    }
}

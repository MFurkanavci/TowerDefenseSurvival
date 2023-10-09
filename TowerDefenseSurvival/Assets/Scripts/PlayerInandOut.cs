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
            int resource = player.GetComponent<ResourceGathering>().resource;
            if (isInside)
            {
                infoText.text = "Press E to go outside";
            }
            else
            {
                infoText.text = "Press \"E\" to go inside\nPress \"Q\" to bank resources";
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (isInside)
                {
                    isInside = false;
                    player.transform.position = tempPos;
                    player.SetActive(true);
                    mainTurretControls.isInside = false;
                }
                else
                {
                    isInside = true;
                    tempPos = player.transform.position;
                    player.SetActive(false);
                    mainTurretControls.isInside = true;

                }
                CameraControls();
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                towerResources.ModifyResource(TowerResources.ResourceType.Wood, resource, true);
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
        //we need to adjust few thing for camera, first we need to chanege the camera follow and lookat to the player or the tower and change the fov,and follow offsets

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

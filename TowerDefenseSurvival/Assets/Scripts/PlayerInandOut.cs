using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;

public class PlayerInandOut : MonoBehaviour
{
    public bool isInTrigger = false;
    public bool isInside = false;
    public GameObject player;   
    public TowerResources towerResources;

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
                    insideCamera.Follow = player.transform;
                    insideCamera.LookAt = player.transform;
                    insideCamera.m_Lens.FieldOfView = 60f;
                    isInside = false;
                    player.transform.position = tempPos;
                    player.GetComponent<CharaterMovement>().enabled = true;
                    player.GetComponent<CharacterController>().enabled = true;
                }
                else
                {
                    player.GetComponent<CharaterMovement>().enabled = false;
                    player.GetComponent<CharacterController>().enabled = false;
                    tempPos = player.transform.position;
                    tempPos.y = 0.5f;
                    player.transform.position = new Vector3(0,0,0);
                    isInside = true;
                    insideCamera.Follow = gameObject.transform.parent.transform;
                    insideCamera.LookAt = gameObject.transform.parent.transform;
                    insideCamera.m_Lens.FieldOfView = 90f;

                }
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isInTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            infoText.text = "";
            isInTrigger = false;
        }
    }
}

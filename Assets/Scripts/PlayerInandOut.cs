using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;

public class PlayerInandOut : MonoBehaviour
{
    private bool isInTrigger = false;
    private bool isInside = false;

    public GameObject playerGameObject, TowerResourcesPanel,model;   
    public TowerResources towerResources;

    public MainTurretControls mainTurretControls;

    public TextMeshProUGUI infoText;
    Vector3 tempPos;

    public CinemachineVirtualCamera insideCamera;

    public float camY { get; set;}
    private void OnEnable()
    {
        GameManager.OnGameStateChanged += HandleGameStateChanged;
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= HandleGameStateChanged;
    }

    private void HandleGameStateChanged(GameState currentState)
    {
        switch (currentState)
        {
            case GameState.MainMenu:
                break;
            case GameState.Respawning:
                Respawning();
                break;
            case GameState.Playing:
                break;
            case GameState.Paused:
                break;
            case GameState.GameOver:
                break;
        }
    }

    private void Update()
    {
        if (!isInTrigger)
        {
            infoText.text = "";
            return;
        }

        if (!playerGameObject.GetComponent<Player>().isAlive)
        {
            infoText.text = "You are dead wait for respawn";
            return;
        }

        if (isInside)
        {
            infoText.text = "Press \"Space\" to go outside";
            playerGameObject.GetComponent<CharaterMovement>().enabled = false;
        }
        else
        {
            infoText.text = "Press \"Space\" to go inside\nPress \"Q\" to bank resources";
            playerGameObject.GetComponent<CharaterMovement>().enabled = true;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isInside && playerGameObject.GetComponent<Player>().isAlive)
            {
                ExitInside();
            }
            else
            {
                EnterInside();
            }
            CameraControls();
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            BankResources();
        }
    }

    private void ExitInside()
    {
        isInside = false;
        playerGameObject.transform.position = tempPos;
        model.SetActive(true);
        mainTurretControls.SetIsInside(false);
        playerGameObject.GetComponent<Collider>().enabled = true;
    }

    private void EnterInside()
    {
        isInside = true;
        tempPos = playerGameObject.transform.position;
        model.SetActive(false);
        mainTurretControls.SetIsInside(true);
        playerGameObject.GetComponent<Collider>().enabled = false;
    }

    private void BankResources()
    {
        foreach (ResourceTypes.Resources resource in System.Enum.GetValues(typeof(ResourceTypes.Resources)))
        {
            towerResources.ModifyResource(resource, playerGameObject.GetComponent<ResourceGathering>().GetResourceAmount(resource));
        }
        playerGameObject.GetComponent<ResourceGathering>().ResetResource();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Player>(out Player player))
        {
            TowerResourcesPanel.SetActive(true);
            isInTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Player>(out Player player))
        {
            TowerResourcesPanel.SetActive(false);
            isInTrigger = false;
        }
    }

    public void CameraControls()
    {

        float insideCameraFOV = 90f;
        float outsideCameraFOV = 60f;

        float  insideCameraFollowOffsetx = 0f;
        float  insideCameraFollowOffsety = 25f + camY;
        float  insideCameraFollowOffsetz = -6.5f;

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
            insideCamera.Follow = playerGameObject.transform;
            insideCamera.LookAt = playerGameObject.transform;
            insideCamera.m_Lens.FieldOfView = outsideCameraFOV;
            insideCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = new Vector3(outsideCameraFollowOffsetx, outsideCameraFollowOffsety, outsideCameraFollowOffsetz);
        }
    }

    private void Respawning()
    {
        isInTrigger = true;
        isInside = true;
        playerGameObject.transform.position = tempPos;
        model.SetActive(false);
        mainTurretControls.SetIsInside(true);
        TowerResourcesPanel.SetActive(true);
        CameraControls();
    }
}

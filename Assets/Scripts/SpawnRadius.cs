using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRadius : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    public void CalculateCulling(Camera cam)
    {
        Vector3 camPos = cam.transform.position;
        Vector3 camForward = cam.transform.forward;
        Vector3 camRight = cam.transform.right;
        Vector3 camUp = cam.transform.up;

        Vector3 frustumCenter = camPos + camForward * cam.farClipPlane * 0.5f;
        Vector3 frustumUp = camUp * cam.farClipPlane * Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad);
        Vector3 frustumRight = camRight * cam.farClipPlane * Mathf.Tan(cam.fieldOfView * cam.aspect * 0.5f * Mathf.Deg2Rad);

        Vector3 frustumFarTopLeft = frustumCenter + frustumUp - frustumRight;
        Vector3 frustumFarTopRight = frustumCenter + frustumUp + frustumRight;
        Vector3 frustumFarBottomLeft = frustumCenter - frustumUp - frustumRight;
        Vector3 frustumFarBottomRight = frustumCenter - frustumUp + frustumRight;
    }

    void Update()
    {
        CalculateCulling(mainCamera);
    }
}

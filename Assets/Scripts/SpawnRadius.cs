using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRadius
{
    private Camera mainCamera;

    private static SpawnRadius instance;

    public static SpawnRadius Instance
    {
        get
        {
            if (instance == null)
                instance = new SpawnRadius();
            return instance;
        }
    }

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    public bool IsInCullingArea(Camera cam, Vector3 position, float radius)
    {
        Vector3 screenPoint = cam.WorldToViewportPoint(position);
        bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
        return onScreen;
    }
}

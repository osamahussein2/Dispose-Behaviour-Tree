using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageCollectorData : MonoBehaviour
{
    public float garbageCollectorSpeed;
    public List<float> garbageSpawnTime;
    public GameObject garbagePrefab;
    public List<Vector3> garbageSpawnPoint;
    public float garbagePickupDistance;
    public float garbageBinDistance;
    public GameObject garbageBin;
    public Vector3 cameraOffset;
    public float garbageFallSpeed;
    public float playerMoveSpeed;
}

using NodeCanvas.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseGuardData : MonoBehaviour
{
    public float rotationSpeed;
    public float lookAroundTime;
    public float rotationThreshold;
    public float houseGuardRadius;
    public LayerMask collectorMask;
    public GameObject player;
    public GameObject bulletPrefab;
    public float bulletSpeed;
    public float bulletFireCooldownTimer;
}

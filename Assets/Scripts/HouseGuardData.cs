using NodeCanvas.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public GameObject castleGuard;
    public float distanceToCastleGuard;
    public float moveToCastleGuardSpeed;
    public Slider houseGuardInteractionSlider;
    public Slider castleGuardInteractionSlider;
    public float houseGuardInteractionIncreaseRate;
    public float castleGuardInteractionIncreaseRate;
    public float houseGuardInteractionDecreaseRate;
    public float castleGuardInteractionDecreaseRate;
    public float lowInteractionLevelValue;
    public GameObject houseLocation;
    public float houseGuardSpeed;
}

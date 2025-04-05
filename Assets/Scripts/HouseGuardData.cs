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
    public Text houseGuardStateText;
    public GameObject shootingAlert;
    public GameObject lookingAlert;
    public GameObject movingToCastleGuardAlert;
    public GameObject interactingWithCastleGuardAlert;
    public float houseGuardSliderValue;
    public float castleGuardSliderValue;
    public GameObject hitAlert;

    // Variables for updating the house guard no matter what their current state is
    public static Slider houseGuardHealth;

    private void Start()
    {
        // Get the house guard's health slider before setting its value to maximum
        houseGuardHealth = GameObject.Find("House Guard Health Slider").GetComponent<Slider>();

        if (houseGuardHealth != null) houseGuardHealth.value = houseGuardHealth.maxValue;
    }

    private void Update()
    {
        // If the house guard's health goes below 0, destroy house guard
        if (houseGuardHealth.value <= 0.0f)
        {
            Destroy(gameObject);
        }
    }
}

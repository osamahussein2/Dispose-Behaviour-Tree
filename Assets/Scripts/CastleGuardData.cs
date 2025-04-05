using NodeCanvas.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CastleGuardData : MonoBehaviour
{
    public GameObject garbageCollector;
    public float castleGuardPatrolSpeed;
    public float castleGuardChaseSpeed;
    public float meleeAttackRange;
    public GameObject exclamationMark;
    public GameObject castleLocation;
    public float castleGuardRadius;
    public LayerMask collector;
    public float castleGuardWarning;
    public GameObject questionMark;
    public GameObject hitAlert;
    public GameObject swordPrefab;
    public GameObject castleGuardHand;
    public Text castleGuardStateText;

    // Variables for updating the castle guard no matter what their current state is
    public static Slider castleGuardHealth;

    private void Start()
    {
        // Get the castle guard's health slider before setting its value to maximum
        castleGuardHealth = GameObject.Find("Castle Guard Health Slider").GetComponent<Slider>();

        if (castleGuardHealth != null) castleGuardHealth.value = castleGuardHealth.maxValue;
    }

    private void Update()
    {
        // If castle guard's health goes below 0, destroy castle guard
        if (castleGuardHealth.value <= 0.0f)
        {
            Destroy(gameObject);
        }
    }
}

using NodeCanvas.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}

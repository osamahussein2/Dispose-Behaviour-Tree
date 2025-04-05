using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    public Text playerStateText;
    public GameObject playerMeleeWeaponPrefab;

    public GameObject castleGuard;
    public GameObject houseGuard;

    public GameObject houseObjectiveMarker;
    public GameObject castleObjectiveMarker;

    public Text houseGuardStateText;
    public Text castleGuardStateText;

    public Text houseGuardInteractionLevel;
    public Text castleGuardInteractionLevel;

    public Slider garbageCollectorHealth;

    public GameObject hitAlert;

    public RawImage levelCompletedRawImage;

    private float timer;

    private void Start()
    {
        castleObjectiveMarker.SetActive(true);
        houseObjectiveMarker.SetActive(true);

        levelCompletedRawImage.gameObject.SetActive(false);

        // Make sure the player has max health to start
        garbageCollectorHealth.value = garbageCollectorHealth.maxValue;

        timer = 0.0f;
    }

    private void Update()
    {
        // If the castle guard is gone while the castle marker is still active and the player is close to it
        if (castleGuard.IsDestroyed() && castleObjectiveMarker.activeInHierarchy &&
            Vector3.Distance(gameObject.transform.position, castleObjectiveMarker.transform.position) <= 0.5f)
        {
            // Hide the castle objective marker
            castleObjectiveMarker.SetActive(false);
        }

        // If the house guard is gone while the house marker is still active and the player is close to it
        if (houseGuard.IsDestroyed() && houseObjectiveMarker.activeInHierarchy &&
            Vector3.Distance(gameObject.transform.position, houseObjectiveMarker.transform.position) <= 0.5f)
        {
            // Hide the house objective marker
            houseObjectiveMarker.SetActive(false);
        }

        // If both house and castle objective markers are hidden, show the level completed screen
        if (!houseObjectiveMarker.activeInHierarchy && !castleObjectiveMarker.activeInHierarchy)
        {
            levelCompletedRawImage.gameObject.SetActive(true);

            timer += Time.deltaTime; // Increase the timer until it exceeds a certain time

            // Once the timer exceeds a certain value, restart the level
            if (timer >= 2.0f)
            {
                SceneManager.LoadScene("DisposeScene");
            }
        }

        // Call the functions
        IfCastleGuardIsDestroyed();
        IfHouseGuardIsDestroyed();

        // If the player's health reaches 0, restart the level
        if (garbageCollectorHealth.value <= 0.0f)
        {
            SceneManager.LoadScene("DisposeScene");
        }
    }

    private void IfCastleGuardIsDestroyed()
    {
        // Make sure to destroy the castle guard state text once castle guard is destroyed
        if (castleGuard.IsDestroyed() && !castleGuardStateText.IsDestroyed())
        {
            Destroy(castleGuardStateText.gameObject);

            hitAlert.SetActive(false); // Hide hit alert too
        }

        // Make sure to destroy the other texts once castle guard is destroyed as well
        if (castleGuard.IsDestroyed() && !houseGuardInteractionLevel.IsDestroyed() && 
            !castleGuardInteractionLevel.IsDestroyed())
        {
            Destroy(houseGuardInteractionLevel.gameObject);
            Destroy(castleGuardInteractionLevel.gameObject);
        }
    }

    private void IfHouseGuardIsDestroyed()
    {
        // Make sure to destroy the house guard state text once house guard is destroyed
        if (houseGuard.IsDestroyed() && !houseGuardStateText.IsDestroyed())
        {
            Destroy(houseGuardStateText.gameObject);

            hitAlert.SetActive(false); // Hide hit alert too
        }

        // Make sure to destroy the other texts once house guard is destroyed as well
        if (houseGuard.IsDestroyed() && !houseGuardInteractionLevel.IsDestroyed() && 
            !castleGuardInteractionLevel.IsDestroyed())
        {
            Destroy(houseGuardInteractionLevel.gameObject);
            Destroy(castleGuardInteractionLevel.gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Actor that fires bullets
/// </summary>
[RequireComponent(typeof(BulletManager))]
public class Actor : MonoBehaviour {

    public WeaponConfig weapon;
    public float fireRate;

    private float fireCooldown = 0;
    private BulletManager bulletManager;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake() {
        bulletManager = GetComponent<BulletManager>();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update() {
        // handles bullet firing and cooldown
        if (fireCooldown > 0) {
            fireCooldown -= Time.deltaTime;
        } else {
            fireCooldown = 1f / (fireRate * weapon.rateMulti);
            Shoot();
        }
    }

    /// <summary>
    /// Shoots every bullet in a burst according to the weapon configuration
    /// </summary>
    private void Shoot() {
        // setting up positions and angles
        Vector2 distance = weapon.distance;
        List<float> angles = new List<float>();
        List<Vector3> positions = new List<Vector3>();
        for (int i = 0; i < weapon.bulletCount; i++) {
            if (!weapon.nova) {
                float iMulti = i - (weapon.bulletCount - 1) / 2f;
                angles.Add(weapon.angle * iMulti);
                positions.Add(new Vector3(distance.x * iMulti, 0,
                    distance.y * Mathf.Abs(iMulti)));
            } else {
                angles.Add(360f / weapon.bulletCount * i);
                positions.Add((Quaternion.Euler(0, angles[i], 0) *
                    transform.forward).normalized * distance.y);
            }
        }
        // firing each bullet
        for (int i = 0; i < weapon.bulletCount; i++) {
            float angle = 0;
            Vector3 pos = new Vector3();
            // if random order is set, choose a random angle and position, removing them from the lists
            if (weapon.randomOrder) {
                int order = Random.Range(0, angles.Count);
                angle = angles[order];
                pos = positions[order];
                angles.RemoveAt(order);
                positions.RemoveAt(order);
            } else {
                angle = angles[i];
                pos = positions[i];
            }
            // if burst spread is set, fires the bullets with a delay between them
            if (weapon.burstSpread > 0) {
                float delay = 1f / (fireRate * weapon.rateMulti) * weapon.burstSpread /
                    weapon.bulletCount * i;
                StartCoroutine(InitBulletDelayed(weapon, pos, angle, delay));
            } else {
                Bullet bullet = bulletManager.Get();
                bullet.Init(weapon, gameObject, transform.position + pos, angle);
            }
        }
    }

    /// <summary>
    /// Fires a bullet after a set delay, used for burst spread
    /// </summary>
    /// <param name="weapon">Weapon config</param>
    /// <param name="pos">Bullet starting position</param>
    /// <param name="angle">Bullet starting angle</param>
    /// <param name="delay">Delay in seconds</param>
    /// <returns>Coroutine delay enumerator</returns>
    private IEnumerator InitBulletDelayed(WeaponConfig weapon, Vector3 pos, float angle, float delay) {
        yield return new WaitForSeconds(delay);
        Bullet bullet = bulletManager.Get();
        bullet.Init(weapon, gameObject, transform.position + pos, angle);
    }

    /// <summary>
    /// Returns the actor's bullet manager
    /// </summary>
    /// <returns>the bullet manager</returns>
    public BulletManager GetBulletManager() {
        return bulletManager;
    }
}
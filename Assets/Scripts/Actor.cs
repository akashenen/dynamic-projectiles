using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour {

    public WeaponConfig weapon;
    public float fireRate;

    private float fireCooldown = 0;

    // Update is called once per frame
    void Update() {
        if (fireCooldown > 0) {
            fireCooldown -= Time.deltaTime;
        } else {
            fireCooldown = 1f / (fireRate * weapon.rateMulti);
            Shoot();
        }
    }

    private void Shoot() {
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
        for (int i = 0; i < weapon.bulletCount; i++) {
            float angle = 0;
            Vector3 pos = new Vector3();
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
            if (weapon.burstSpread > 0) {
                float delay = 1f / (fireRate * weapon.rateMulti) * weapon.burstSpread /
                    weapon.bulletCount * i;
                StartCoroutine(InitBulletDelayed(weapon, pos, angle, delay));
            } else {
                Bullet bullet = BulletManager.Instance.Get();
                bullet.Init(weapon, gameObject, transform.position + pos, angle);
            }
        }
    }

    private IEnumerator InitBulletDelayed(WeaponConfig weapon, Vector3 pos, float angle, float delay) {
        yield return new WaitForSeconds(delay);
        Bullet bullet = BulletManager.Instance.Get();
        bullet.Init(weapon, gameObject, transform.position + pos, angle);
    }
}
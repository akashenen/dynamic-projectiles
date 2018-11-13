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
            fireCooldown = 1 / (fireRate * weapon.rateMulti);
            Shoot();
        }
    }

    private void Shoot() {
        Vector2 distance = weapon.distance;
        float angle = weapon.angle;
        for (int i = 0; i < weapon.bulletCount; i++) {
            float iMulti = i - (weapon.bulletCount - 1) / 2f;
            Vector3 pos = new Vector3();
            if (!weapon.nova) {
                pos = new Vector3(distance.x * iMulti, 0,
                    distance.y * Mathf.Abs(iMulti));
            } else {
                angle = 360f / weapon.bulletCount;
                pos += (Quaternion.Euler(0, angle * iMulti, 0) * transform.forward).normalized * distance.y;
            }
            Bullet bullet = BulletManager.Instance.Get();
            bullet.Init(weapon, gameObject, transform.position + pos,
                angle * iMulti);
        }
    }
}
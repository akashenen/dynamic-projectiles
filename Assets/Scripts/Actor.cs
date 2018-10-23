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
            fireCooldown = 1 / fireRate;
            Shoot();
        }
    }

    private void Shoot() {
        for (int i = 0; i < weapon.bulletCount; i++) {
            float iMulti = i - (weapon.bulletCount - 1) / 2f;
            Vector3 pos = new Vector3(weapon.distance.x * iMulti,
                weapon.distance.y * Mathf.Abs(iMulti), 0);
            Bullet bullet = BulletManager.Instance.Get();
            bullet.Init(weapon, gameObject, transform.position + pos,
                weapon.angle * iMulti);
        }
    }
}
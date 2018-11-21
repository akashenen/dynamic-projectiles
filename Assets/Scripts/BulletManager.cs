using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour {

    public static BulletManager Instance;

    public Bullet bulletPrefab;
    private List<Bullet> bulletPool;
    private int activeCount;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        bulletPool = new List<Bullet>();
        activeCount = 0;
    }

    public Bullet Get() {
        if (activeCount >= bulletPool.Count) {
            Bullet newBullet = Instantiate(bulletPrefab);
            newBullet.transform.parent = transform;
            bulletPool.Add(newBullet);
            activeCount++;
            return newBullet;
        }
        foreach (Bullet b in bulletPool) {
            if (!b.gameObject.activeInHierarchy) {
                b.gameObject.SetActive(true);
                activeCount++;
                return b;
            }
        }
        return null;
    }

    public void Return(Bullet b) {
        b.gameObject.SetActive(false);
        activeCount--;
    }

    private void ClearPool() {
        foreach (Bullet b in bulletPool) {
            Destroy(b.gameObject);
        }
        bulletPool.Clear();
        activeCount = 0;
    }

    public void SetNewPrefab(Bullet prefab) {
        ClearPool();
        bulletPrefab = prefab;
    }
}
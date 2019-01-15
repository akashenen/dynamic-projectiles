using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages and stores bullets for an actor, will create more bullets when needed.
/// </summary>
public class BulletManager : MonoBehaviour {

    public Bullet bulletPrefab;
    private List<Bullet> bulletPool;
    private int activeCount;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    private void Start() {
        bulletPool = new List<Bullet>();
        activeCount = 0;
    }

    /// <summary>
    /// Returns an inactive bullet or creates a new one if all of them are active. 
    /// </summary>
    /// <returns>Bullet ready to be initialized</returns>
    public Bullet Get() {
        // If there are no inactive bullets, create a new one
        if (activeCount >= bulletPool.Count) {
            Bullet newBullet = Instantiate(bulletPrefab);
            newBullet.SetManager(this);
            newBullet.transform.parent = transform;
            bulletPool.Add(newBullet);
            activeCount++;
            return newBullet;
        }
        // If there are inactive bullets, activates and returns one of them
        foreach (Bullet b in bulletPool) {
            if (!b.gameObject.activeInHierarchy) {
                b.gameObject.SetActive(true);
                activeCount++;
                return b;
            }
        }
        return null;
    }

    /// <summary>
    /// After a bullet finishes being used, is returned and deactivated
    /// </summary>
    /// <param name="b">Bullet to be returned</param>
    public void Return(Bullet b) {
        // Checks if the bullet really belongs to the manager, otherwise can create problems with activeCount
        if (bulletPool.Contains(b)) {
            b.gameObject.SetActive(false);
            activeCount--;
        } else {
            Debug.Log("Bullet returned to the wrong manager!", b);
        }
    }

    /// <summary>
    /// Destroys all the bullets inside the bullet pool and resets its state
    /// </summary>
    private void ClearPool() {
        foreach (Bullet b in bulletPool) {
            Destroy(b.gameObject);
        }
        bulletPool.Clear();
        activeCount = 0;
    }

    /// <summary>
    /// This can be used if the bullet prefab of the manager needs to be changed during runtime, 
    /// deleting all previous bullets
    /// </summary>
    /// <param name="prefab"></param>
    public void SetNewPrefab(Bullet prefab) {
        // resets the pool so old prefabs don't stay in the pool incorrectly
        ClearPool();
        bulletPrefab = prefab;
    }
}
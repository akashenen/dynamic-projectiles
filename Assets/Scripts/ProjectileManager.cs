using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages and stores projectiles for an actor, will create more projectiles when needed.
/// </summary>
public class ProjectileManager : MonoBehaviour {

    public Projectile projectilePrefab;
    private List<Projectile> projectilePool;
    private int activeCount;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    private void Awake() {
        projectilePool = new List<Projectile>();
        activeCount = 0;
    }

    /// <summary>
    /// Returns an inactive projectile or creates a new one if all of them are active. 
    /// </summary>
    /// <returns>Projectile ready to be initialized</returns>
    public Projectile Get() {
        // If there are no inactive projectiles, create a new one
        if (activeCount >= projectilePool.Count) {
            Projectile newProjectile = Instantiate(projectilePrefab);
            newProjectile.SetManager(this);
            newProjectile.transform.parent = transform;
            projectilePool.Add(newProjectile);
            activeCount++;
            return newProjectile;
        }
        // If there are inactive projectiles, activates and returns one of them
        foreach (Projectile b in projectilePool) {
            if (!b.gameObject.activeInHierarchy) {
                b.gameObject.SetActive(true);
                activeCount++;
                return b;
            }
        }
        return null;
    }

    /// <summary>
    /// After a projectile finishes being used, is returned and deactivated
    /// </summary>
    /// <param name="b">Projectile to be returned</param>
    public void Return(Projectile b) {
        // Checks if the projectile really belongs to the manager, otherwise can create problems with activeCount
        if (projectilePool.Contains(b)) {
            b.gameObject.SetActive(false);
            activeCount--;
        } else {
            Debug.Log("Projectile returned to the wrong manager!", b);
        }
    }

    /// <summary>
    /// Destroys all the projectiles inside the projectile pool and resets its state
    /// </summary>
    private void ClearPool() {
        foreach (Projectile b in projectilePool) {
            Destroy(b.gameObject);
        }
        projectilePool.Clear();
        activeCount = 0;
    }

    /// <summary>
    /// This can be used if the projectile prefab of the manager needs to be changed during runtime, 
    /// deleting all previous projectiles
    /// </summary>
    /// <param name="prefab"></param>
    public void SetNewPrefab(Projectile prefab) {
        // resets the pool so old prefabs don't stay in the pool incorrectly
        ClearPool();
        projectilePrefab = prefab;
    }
}
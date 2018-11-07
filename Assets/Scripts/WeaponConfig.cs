using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is used to store all information about a specific weapon's visuals and behaviour, 
/// and can be changed during gameplay to change or adapt any properties of that weapon
/// </summary>
[CreateAssetMenu(fileName = "Weapon", menuName = "Data/Weapon", order = 2)]
public class WeaponConfig : ScriptableObject {

	[Header("Visual")]
	[Tooltip("Color used for the main particle.")]
	public Color mainColor;
	[Tooltip("Gradient used for the secondary particle and the trail")]
	public Gradient colorGradient;
	[Tooltip("Length of the bullet's trail")]
	public float trailLength;
	[Tooltip("Width of the bullet's trail")]
	public float trailWidth;

	[Header("Functional")]
	[Tooltip("How many bullets should be fired at each burst.")]
	public float bulletCount;
	[Tooltip("How much time between each bullet should be proportionally spread across the interval of each burst. 0 = every bullet at the same time; 1 = bullets are spread across all the interval, making the bursts seem seamless.")]
	[Range(0f, 1f)]
	public float burstSpread;
	[Tooltip("Starting position distance of each bullet in relation to the center one. Ignored in case of nova type burst.")]
	public Vector2 distance;
	[Tooltip("Angle (in degrees) between each bullet and the next.")]
	public float angle;
	[Tooltip("If checked, will ignore distance and angle properties and distribute the starting points of each bullet around the parent object.")]
	public bool nova;
	[Tooltip("If checked, will put each bullet at a random angle inside an arc defined by the *angle* property.")]
	public bool randomAngle;
	[Tooltip("Traveling speed of each bullet fired")]
	public float speed;
	[Tooltip("Maximum lifetime in seconds for each bullet.")]
	public float duration;
	[Tooltip("How long each bullet takes for all particles to finish their animations before it gets disabled, can take longer depending on the desired effects. Default = 0.5 seconds")]
	public float deathTime = 0.5f;
	[Tooltip("Multiplier for the parent's damage for each bullet.")]
	public float damageMulti;
	[Tooltip("Multiplier for the parent's firing rate.")]
	public float rateMulti;
	[Tooltip("Behaviour of each bullet shot. Each behaviour is programmed individually, so you have to change the bullet script to add more behaviours.")]
	public BulletBehaviour behaviour;
	[Tooltip("WIP. Will have no effect.")]
	public List<BulletEffect> effects;

	public enum BulletBehaviour {
		Straight,
		Homing,
		Spin
	}

	public enum BulletEffectType {
		Explosion,
		Fork,
		Chain,
	}

	public struct BulletEffect {
		public BulletEffectType type;
		public float value;
	}
}
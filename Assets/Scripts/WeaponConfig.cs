using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Data/Weapon", order = 2)]
public class WeaponConfig : ScriptableObject {
	[Header("Visual")]
	public Gradient gradient;
	public Color glowColor;
	public float trailLenght;
	public float trailWidth;

	[Header("Functional")]
	public float bulletCount;
	[Range(0f, 1f)]
	public float burstSpread;
	public Vector2 distance;
	public float angle;
	public bool randomAngle;
	public float speed;
	public float duration;
	public float damageMulti;
	public float rateMulti;
	public BulletBehaviour behaviour;
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
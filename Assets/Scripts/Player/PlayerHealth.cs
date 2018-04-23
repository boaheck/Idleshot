using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {

	float health;
	public Image healthBar;
	public Text healthDisplay;
	public float hBarWidth;
	[SerializeField]float maxHealth = 100f;
	ScoreSystem scores;
	Vector3 spawnPos;

	void Start () {
		health = maxHealth;
		scores = FindObjectOfType<ScoreSystem> ();
		spawnPos = transform.position;
	}

	void UpdateHealthUI () {
		healthBar.rectTransform.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, hBarWidth * (health/maxHealth));
		healthDisplay.text = (int)health + "/" + (int)maxHealth;
	}

	void OnTriggerEnter(Collider other){
		if (other.tag == "EnemyBullet") {
			health -= other.GetComponent<Projectile> ().damage;
			UpdateHealthUI ();
			if (health <= 0) {
				Die ();
			}
			Destroy (other.gameObject);
		}
	}

	void Die(){
		transform.position = spawnPos;
		scores.ClearShells ();
		health = maxHealth;
		UpdateHealthUI ();
	}

	public void FullHeal(){
		health = maxHealth;
		UpdateHealthUI ();
	}

	public void Heal(float amount){
		health += amount;
		if (health > maxHealth) {
			health = maxHealth;
		}
		UpdateHealthUI ();
	}

	public void AddMaxHealth(float amount){
		maxHealth += amount;
		Heal(amount);
		UpdateHealthUI ();
	}

	public void AddMaxHealthPerc(float amount){
		maxHealth *= amount;
		Heal(maxHealth);
		UpdateHealthUI();
	}

	public float GetHealth(){
		return health;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurretHealth : MonoBehaviour {

	float health;
	public Image healthBar;
	public Text healthDisplay;
	public GameObject canvas;
	public float hBarWidth;
	[SerializeField]float maxHealth = 100f;


	void Start () {
		health = maxHealth;
	}

	void UpdateHealthUI () {
		if (!canvas.activeSelf) {
			canvas.SetActive (true);
		}
		healthBar.rectTransform.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, hBarWidth * (health/maxHealth));
		healthDisplay.text = (int)health + "/" + (int)maxHealth;
		if (health >= maxHealth) {
			canvas.SetActive (false);
		}
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
		Destroy (gameObject);
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
		Heal(maxHealth*amount);
		UpdateHealthUI();
	}

	public float GetHealth(){
		return health;
	}
}

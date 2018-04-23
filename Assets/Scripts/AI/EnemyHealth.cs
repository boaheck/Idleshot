using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour {

	float health;
	public Image healthBar;
	public Text healthDisplay;
	public Transform canvas;
	public float hBarWidth;
	[SerializeField]float maxHealth = 100f;
	[SerializeField]int maxDrop = 5;
	public GameObject jelly;
	public GameObject deathParticle;


	void Start () {
		health = maxHealth;
	}

	void Update(){
		canvas.eulerAngles = Vector3.zero;
	}

	void UpdateHealthUI () {
		if (!canvas.gameObject.activeSelf) {
			canvas.gameObject.SetActive (true);
		}
		healthBar.rectTransform.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, hBarWidth * (health/maxHealth));
		healthDisplay.text = (int)health + "/" + (int)maxHealth;
	}

	void OnTriggerEnter(Collider other){
		if (other.tag == "Bullet") {
			health -= other.GetComponent<Projectile> ().damage;
			UpdateHealthUI ();
			if (health <= 0) {
				Die ();
			}
			Destroy (other.gameObject);
		}
	}

	void Die(){
		int amt = Random.Range (0, maxDrop);
		for (int i = 0; i < amt; i++) {
			Instantiate (jelly, transform.position, Quaternion.identity);
		}
		Destroy (transform.parent.gameObject);
		GameObject.Instantiate(deathParticle,transform.position,Quaternion.identity);
	}

	public void SetMaxHealth(float val){
		maxHealth = val;
		health = val;
	}
}

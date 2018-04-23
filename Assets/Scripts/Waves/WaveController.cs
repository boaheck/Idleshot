using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour {

	int wave = 0;
	int timeBetweenWaves = 2;
	int enemiesToSpawn = 5;
	float enemyHealth = 3f;
	float enemyBullets = 1f;
	int waveType = 0;
	public GameObject enemyPrefab;
	bool spawning = false;

	void Start () {
        StartCoroutine(StartNewWave());
	}
	
	void FixedUpdate () {
		if(!spawning){

            EnemyAI[] enemies = GameObject.FindObjectsOfType<EnemyAI>();
			if(enemies.Length <= 0){
				StartCoroutine(StartNewWave());
			}
		}
	}

	IEnumerator StartNewWave() {
		spawning = true;
		wave++;
		waveType++;
		if(waveType >= 3){
			waveType = 0;
		}
		int timeWaited=  0;
        while (timeWaited++ < timeBetweenWaves) {
			Debug.Log(timeBetweenWaves-timeWaited+" Until next wave");
            yield return new WaitForSeconds(1);
        }
		switch(waveType){
			case 0:{
				//Increase Amount
				enemiesToSpawn = (int)(enemiesToSpawn*2.5f);
				break;
			}
			case 1:{
				//Increase Health
				enemyHealth *= 1.8f;
				break;
			}
			case 2:{
				//Increase Bullets
				enemyBullets *= 2f;
				break;
			}
			default: {
				break;
			}
		}
		int spawnedAI = 0;
		while(spawnedAI < enemiesToSpawn){
			spawnedAI++;
			SpawnPoint[] spawnPoints = GameObject.FindObjectsOfType<SpawnPoint>();
			SpawnPoint selectedPoint = spawnPoints[Random.Range(0,spawnPoints.Length)];
			while(selectedPoint.HasPlayer()){
                selectedPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
			}
            Vector3 location = selectedPoint.transform.position + (Random.insideUnitSphere * 4f);
            location.y = selectedPoint.transform.position.y;
            GameObject enemy = GameObject.Instantiate(enemyPrefab, location, Quaternion.Euler(0, Random.Range(0, 360f), 0));

			EnemyAI ai = enemy.GetComponent<EnemyAI>();
			EnemyHealth eh = enemy.GetComponentInChildren<EnemyHealth>();
			EnemyShooting es = enemy.GetComponentInChildren<EnemyShooting>();

			eh.SetMaxHealth(enemyHealth*Random.Range(0.9f,1.1f));
			es.SetShotAmount((int)enemyBullets);

			yield return new WaitForSeconds(Random.Range(0.1f,1f));
		}
		spawning = false;
	}

}

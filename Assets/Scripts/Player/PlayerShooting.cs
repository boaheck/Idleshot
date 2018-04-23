using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerShooting : MonoBehaviour
{

    [SerializeField] Vector3 shotOffset = Vector3.zero;
    [SerializeField] public float rate = 5;
    [SerializeField] public float spread = 5.0f;
    [SerializeField] public bool auto = false;

    float overflow = 0.0f;
    float interval;
    float timer;
    bool fired, firing = false;
    ScoreSystem scores;

    public GameObject projectile;
    public AudioClip shootClip;
    private AudioSource audioSource;

    public float strength = 1f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        timer = 0;
        scores = GameObject.Find("Scores").GetComponent<ScoreSystem>();
        interval = 1.0f / rate;
    }

    void Update()
    {
        fired = Input.GetButtonDown("Fire1");
		firing = Input.GetButton ("Fire1");
    }

    void FixedUpdate()
    {
        #if UNITY_EDITOR
            if(Input.GetKeyDown(KeyCode.P)){
                EnemyAI[] enemies = GameObject.FindObjectsOfType<EnemyAI>();
                foreach(EnemyAI ai in enemies){
                    GameObject.Destroy(ai.gameObject);
                }
            }
        #endif
        bool onUI = EventSystem.current.IsPointerOverGameObject();
        if (!onUI)
        {

            if (auto)
            {
				if (fired && timer <= 0)
                {
                    Fire();
                    timer = interval;
                }
				else if (firing)
                {
                    if (timer <= 0)
                    {
                        if (interval < Time.fixedDeltaTime)
                        {
                            float bulletTot = Time.fixedDeltaTime / interval;
                            bulletTot += overflow;
                            int bulletAmt = Mathf.FloorToInt(bulletTot);
                            overflow = bulletTot - bulletAmt;
                            for (int i = 0; i < bulletAmt; i++)
                            {
                                Fire();
                            }
                        }
                        else
                        {
                            Fire();
                        }
                        timer = interval;
                    }
                }
            }
            else
            {
                if (timer <= 0)
                {
                    if (fired)
                    {
                        Fire();
                        timer = interval;
                    }
                }
                
            }
        }
		if (timer > 0) {
				timer -= Time.fixedDeltaTime;
		}
        fired = false;
    }

    void Fire()
    {
        audioSource.pitch = Random.Range(0.7f, 1.1f);
        audioSource.PlayOneShot(shootClip);
        Quaternion randRot = Quaternion.Euler(Vector3.up * Random.Range(-spread, spread));
        GameObject bullet = Instantiate(projectile, transform.position + (transform.rotation * shotOffset), transform.rotation * randRot);
        bullet.GetComponent<Projectile>().damage = strength;
        scores.AddShells();
    }
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShipHealth : MonoBehaviour {

    public float MaxHealth = 100f;
    public float MyHealth = 100f;
    public float HealthRegainrate;

    public Text HealthUI;

    //SINGLETON SET UP
    private static ShipHealth _instance;
    public static ShipHealth Instance
    {
        get
        {
            if (_instance == null)
            {
                //_instance = new GameObject("GameManager");
                //_instance.AddComponent<GameManager>();
            }

            return _instance;
        }
    }
    void Awake()
    {
        _instance = this;
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        HealthUI.text = Mathf.RoundToInt(MyHealth).ToString();
        RechargeHealth();
    }

    void RechargeHealth ()
    {
        if(MyHealth < MaxHealth)
        {
            MyHealth += (HealthRegainrate * Time.deltaTime);
        }
        if (MyHealth > MaxHealth)
        {
            MyHealth =MaxHealth;
        }

    }

    public void RestartGame()
    {
        MyHealth = MaxHealth;
    }

    void OnTriggerEnter(Collider c)
    {
        if(c.transform.tag == "EnemyLaser")
        {
            if(c.GetComponent<LaserScript>())
            {
                Debug.Log("TakingDamage");
                MyHealth--;

                //Do Kill Cheak
                if(MyHealth <= 0)
                {
                    GameManager.Instance.PlayerDied();
                }
                

            }

        }

    }
}

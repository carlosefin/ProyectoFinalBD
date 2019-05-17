using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using System.Data.SqlClient;

public class PowerItem : MonoBehaviour {

	private GameObject player;
	private AudioSource audio;
	private PlayerHealth playerHealth;
	private ParticleSystem particleSystem;

	private MeshRenderer meshRenderer;
	private ParticleSystem brainParticles;

	public GameObject pickupEffect;

	private PowerItemExplode powerItemExplode;
	private SphereCollider sphereCollider;

	// Use this for initialization
	void Start () {


		player = GameManager.instance.Player;
		playerHealth = player.GetComponent<PlayerHealth>();
		playerHealth.enabled = true;

		particleSystem = player.GetComponent<ParticleSystem>();
		//particleSystem.enableEmission = false;

		meshRenderer = GetComponentInChildren<MeshRenderer>();
		brainParticles = GetComponent<ParticleSystem>();

		powerItemExplode = GetComponent<PowerItemExplode>();
		sphereCollider = GetComponent<SphereCollider>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject == player)
		{
			
			StartCoroutine(InvincibleRoutine());
			meshRenderer.enabled = false;
			
		}
		Debug.Log("Connecting to database...");
        string connectionstring = @"Data Source = 127.0.0.1; 
                                     user id = Videojuego;
                                     password = Pollis;
                                     Initial Catalog = proyectofinal;";

            using (SqlConnection connection = new SqlConnection(connectionstring))   //esta sentencia permite que connection se destruya al salir de bloque , no hay que usar connection.close() en otras palabras
            {
                connection.Open();
                print("Done.");

                
                using (SqlCommand command = new SqlCommand("dbo.INSERTAR_ITEM", connection))
                { 
                    command.CommandType = System.Data.CommandType.StoredProcedure; //si no le ponen esto no funciona
                    command.Parameters.AddWithValue("@NOMBRE","Poder");
                    command.Parameters.AddWithValue("@TYPE", "da invencibilidad");

                    command.ExecuteNonQuery();
                    print("Lista escritura con un SP.");
                }
            }	
	}

	public IEnumerator InvincibleRoutine()
	{
		powerItemExplode.Pickup();
		print("pick PowerItem");
		particleSystem.enableEmission = true;
		playerHealth.enabled = false;
		brainParticles.enableEmission = false;
		sphereCollider.enabled = false;


		yield return new WaitForSeconds(10f);
		print("no more invencible");
		particleSystem.enableEmission = false;
		playerHealth.enabled = true;
		Destroy(gameObject);



	}

	void Pickup()
	{
		Instantiate(pickupEffect, transform.position, transform.rotation);

	}

}

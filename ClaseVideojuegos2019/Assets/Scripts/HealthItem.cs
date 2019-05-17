using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using System.Data.SqlClient;

public class HealthItem : MonoBehaviour {

	private GameObject player;
	private PlayerHealth playerHealth;


	// Use this for initialization
	void Start () {

		player = GameManager.instance.Player;
		playerHealth = player.GetComponent<PlayerHealth>();
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject == player)
		{

			playerHealth.PowerUpHealth();
			Destroy(gameObject);

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
                    command.Parameters.AddWithValue("@NOMBRE","Vida");
                    command.Parameters.AddWithValue("@TYPE", "da mas vida");

                    command.ExecuteNonQuery();
                    print("Lista escritura con un SP.");
                }
        	}			
		}

		
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using System.Data.SqlClient;

public class KnifeItem : MonoBehaviour
{
    private GameObject player;
    int j = 0;
    private Character_Movement cham;
    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.instance.Player;
        cham = player.GetComponent<Character_Movement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
	{
		if (other.gameObject == player && j == 0)
		{
            j++;
            print("choca2");
			cham.KnifeInc();
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
                        command.Parameters.AddWithValue("@NOMBRE","Cuchillo");
                        command.Parameters.AddWithValue("@TYPE", "arma ligera y lanzable");

                        command.ExecuteNonQuery();
                        print("Lista escritura con un SP.");
                    }

                    using (SqlCommand command = new SqlCommand("dbo.UPDATE_AMMMOINI", connection))
                    { 
                        command.CommandType = System.Data.CommandType.StoredProcedure; //si no le ponen esto no funciona
                        //command.Parameters.

                        command.ExecuteNonQuery();
                        print("Lista escritura con un SP.");
                    }
                }	
		}

	}
}

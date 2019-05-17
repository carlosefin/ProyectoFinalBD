using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using System.Data.SqlClient;

public class LifeItem : MonoBehaviour
{
    private GameObject player;
    private int k = 0;
    private LifeManager lifeManager;

    private SpriteRenderer spriteRenderer;
    public GameObject pickUpEffect;
    private PowerItemExplode powerItemExplode;
    private BoxCollider boxCollider;
   

    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.instance.Player;
        lifeManager = FindObjectOfType<LifeManager>();
        spriteRenderer = GetComponentInChildren <SpriteRenderer>();

        powerItemExplode = GetComponent <PowerItemExplode>();
        boxCollider = GetComponent <BoxCollider>();
    }

    void OnTriggerEnter (Collider other)
    {
        if (other.gameObject == player && k == 0)
        {
            k++;
            PickLife();
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
                        command.Parameters.AddWithValue("@NOMBRE","Corazon");
                        command.Parameters.AddWithValue("@TYPE", "recibe una vida mas");

                        command.ExecuteNonQuery();
                        print("Lista escritura con un SP.");
                    }
                }

            print("Life Collected");
        }
    }

    public void PickLife()
    {
        lifeManager.GiveLife();
        powerItemExplode.Pickup();
        spriteRenderer.enabled = false;
        Destroy (gameObject);
    }    
    // Update is called once per frame
    void Update()
    {
        
    }
}

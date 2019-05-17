using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using System.Data.SqlClient;
public class BDController : MonoBehaviour
{

    //private Character_Movement cha;
    public int bal;
    int valor = 10;
    
    // Start is called before the first frame update
    void Start()
    {
        //Balas();
        print ("funciona perra");
        //cha = GetComponent<Character_Movement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int Balas(int n)
    {
        string conn = @"Data Source = 127.0.0.1;
                                     user id = Videojuego;
                                     password = Pollis;
                                     Initial Catalog = proyectofinal;";

    using (SqlConnection connection = new SqlConnection(conn))   //esta sentencia permite que connection se destruya al salir de bloque , no hay que usar connection.close() en otras palabras
                {
                    connection.Open();
                    
                    using (SqlCommand command = new SqlCommand("SELECT dbo.BALAS(@MUN)", connection))
                    { 
                        
                        command.Parameters.AddWithValue("@MUN", n);
  

                        valor = (int)command.ExecuteScalar();
                        print ("Lista la funcion con {0}");
                        print (valor);
                        bal = valor;
                    }
                }
        return bal;
    }

    
}

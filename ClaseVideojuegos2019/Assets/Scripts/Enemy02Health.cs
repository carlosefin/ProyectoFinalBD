﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Data;
using System.Data.SqlClient;

public class Enemy02Health : MonoBehaviour {


	[SerializeField] private int startingHealth = 20;
	[SerializeField] private float timeSinceLastHit = .5f;
	[SerializeField] private float dissapearSpeed = 2f;
	[SerializeField] private int currentHealth;

	private float timer = 0f;
	private Animator anim;
	private bool isAlive;
	private Rigidbody rigidbody;
	private CapsuleCollider capsuleCollider;
	private bool dissapearEnemy = false;
	private DropItem dropItem;



	public bool IsAlive
	{
		get { return isAlive; }
	}
	// Use this for initialization
	void Start()
	{
		rigidbody = GetComponent<Rigidbody>();
		capsuleCollider = GetComponent<CapsuleCollider>();
		anim = GetComponent<Animator>();
		isAlive = true;
		currentHealth = startingHealth;
		dropItem = GetComponent<DropItem>();
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;

		if (dissapearEnemy)
		{
			transform.Translate(-Vector3.up * dissapearSpeed * Time.deltaTime);

		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (timer >= timeSinceLastHit && !GameManager.instance.GameOver)
		{
			if (other.tag == "PlayerWeapon")
			{
				takeHit();
				timer = 0f;
			}
		}
	}


	void takeHit()
	{
		if (currentHealth > 0)
		{
			anim.Play("Hurt");
			currentHealth -= 10;

		}
		if (currentHealth <= 0)
		{

			isAlive = false;
			KillEnemy();
		}

	}


	void KillEnemy()
	{
		capsuleCollider.enabled = false;
		anim.SetTrigger("EnemyDie");
		rigidbody.isKinematic = true;

		StartCoroutine(removeEnemy());
		
		dropItem.Drop();
		print("objeto");
		Debug.Log("Connecting to database...");
            string connectionstring = @"Data Source = 127.0.0.1; 
                                     user id = Videojuego;
                                     password = Pollis;
                                     Initial Catalog = proyectofinal;";

            using (SqlConnection connection = new SqlConnection(connectionstring))   //esta sentencia permite que connection se destruya al salir de bloque , no hay que usar connection.close() en otras palabras
            {
                connection.Open();
                print("Done.");

                
                using (SqlCommand command = new SqlCommand("dbo.update_qtyarquero", connection))
                { 
                    command.CommandType = System.Data.CommandType.StoredProcedure; //si no le ponen esto no funciona
					command.Parameters.AddWithValue("@DATE",System.DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss"));
                    command.ExecuteNonQuery();
                    print("Lista escritura con un SP.");
                }
            }		
	}

	IEnumerator removeEnemy()
	{
		yield return new WaitForSeconds(2f);
		dissapearEnemy = true;
		yield return new WaitForSeconds(1f);
		Destroy(gameObject);
	}
}

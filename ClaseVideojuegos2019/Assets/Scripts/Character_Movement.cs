using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using System.Data;
using System.Data.SqlClient;

public class Character_Movement : MonoBehaviour
{
    public float maxSpeed = 6.0f;
    public bool facingRight = true;
    public float moveDirection;
    new Rigidbody rigidbody;

    new Rigidbody rigidbody2;
    public float jumpSpeed =600.0f;
    public bool grounded = false;
    public Transform groundCheck;
    public float groundRadius = 0.2f;
    public LayerMask whatIsGround;

    public float hammerSpeed = 600.0f;
    public Transform hammerSpawn;
    public Rigidbody hammerPrefab;
    private AudioSource audio;
    public AudioClip projectileAudio;
    public AudioClip jump;

    public AudioClip pickItem;

    public Rigidbody knifePFB;
    int UsedHammer = 0;
    int UsedKnife = 0;
    //private BDController bdController;
    public int HammerQty;
    public int KnifeQty;
    Rigidbody clone;
    Rigidbody clone2;
    public float tiempo = 0.0f;
    //public GameObject player;
    //private GameObject bdcont;


    void Awake()
    {
        groundCheck = GameObject.Find ("GroundCheck").transform;
        hammerSpawn = GameObject.Find ("HammerSpawn").transform;
    }

    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody> ();
        anim = GetComponent<Animator> ();
        audio = GetComponent<AudioSource>();
        rigidbody2 = GetComponent<Rigidbody>();
        HammerQty = Balas(1);
        KnifeQty = Balas(1);
        //player = GetComponent<GameObject>();
    }

    // Update is called once per frame
    void Update()
    { 
        tiempo += Time.deltaTime;
        if (tiempo >= 5.0f)
        {
            FiveSeconds();
            tiempo = 0.0f;
        }
        moveDirection = CrossPlatformInputManager.GetAxis("Horizontal");
        if (grounded && CrossPlatformInputManager.GetButtonDown ("Jump"))
        {
            audio.PlayOneShot(jump);
            anim.SetTrigger ("isJumping");
            rigidbody.AddForce (new Vector2 (0, jumpSpeed));
        }
          if (CrossPlatformInputManager.GetButtonDown("Fire1") && HammerQty > 0)
        {
            Attack();
            StartCoroutine(ExecuteAfterTime(1));
            
            Debug.Log("Connecting to database...");
            string connectionstring = @"Data Source = 127.0.0.1; 
                                     user id = Videojuego;
                                     password = Pollis;
                                     Initial Catalog = proyectofinal;";

            using (SqlConnection connection = new SqlConnection(connectionstring))   //esta sentencia permite que connection se destruya al salir de bloque , no hay que usar connection.close() en otras palabras
            {
                connection.Open();
                print("Done.");

                using (SqlCommand command = new SqlCommand("dbo.UPDATE_DISPARADA", connection))
                { 
                    command.CommandType = System.Data.CommandType.StoredProcedure; //si no le ponen esto no funcionA
                    //command.Parameters.

                    command.ExecuteNonQuery();
                    print("Lista escritura con un SP.");
                }
            }		
        }
         if (CrossPlatformInputManager.GetButtonDown("Fire2") && KnifeQty > 0)
        {
            Attack();
            StartCoroutine(ExecuteAfterTime2(1));
            Debug.Log("Connecting to database...");
            string connectionstring = @"Data Source = 127.0.0.1; 
                                     user id = Videojuego;
                                     password = Pollis;
                                     Initial Catalog = proyectofinal;";

            using (SqlConnection connection = new SqlConnection(connectionstring))   //esta sentencia permite que connection se destruya al salir de bloque , no hay que usar connection.close() en otras palabras
            {
                connection.Open();
                print("Done.");

                using (SqlCommand command = new SqlCommand("dbo.UPDATE_DISPARADA2", connection))
                { 
                    command.CommandType = System.Data.CommandType.StoredProcedure; //si no le ponen esto no funcionA
                    //command.Parameters.

                    command.ExecuteNonQuery();
                    print("Lista escritura con un SP.");
                }
            }		
        }
    }

    void FixedUpdate()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position,groundRadius,whatIsGround);
        rigidbody.velocity = new Vector2(moveDirection * maxSpeed, rigidbody.velocity.y);

        if (moveDirection > 0.0f && !facingRight)
        {
            Flip();
        } else if (moveDirection < 0.0f && facingRight)
        {
            Flip();
        }
        anim.SetFloat( "Speed", Mathf.Abs (moveDirection));

    }

    void Flip()
    {
         facingRight = !facingRight;
         transform.Rotate (Vector3.up, 180.0f, Space.World);
    }

    void Attack()
    {
        anim.SetTrigger ("Attacking");
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        clone = Instantiate (hammerPrefab, hammerSpawn.position, hammerSpawn.rotation) as Rigidbody;
        clone.AddForce (hammerSpawn.transform.right *hammerSpeed);
        audio.PlayOneShot(projectileAudio);
        HammerQty --;
    }
    IEnumerator ExecuteAfterTime2(float time)
    {
        yield return new WaitForSeconds(time);
        clone2 = Instantiate (knifePFB, hammerSpawn.position, hammerSpawn.rotation) as Rigidbody;
        clone2.AddForce (hammerSpawn.transform.right *hammerSpeed);
        audio.PlayOneShot(projectileAudio);
        KnifeQty --;
    }

     public void HammerInc()
    {
		HammerQty += 5;
    }
    public void KnifeInc()
    {
		KnifeQty += 5;
    }
    public int Balas(int n)
    {
        int bal;
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
  

                        int valor = (int)command.ExecuteScalar();
                        print ("Lista la funcion con {0}");
                        print (valor);
                        bal = valor;
                }
         }
        return bal;
    }

    public void FiveSeconds()
    {
         Debug.Log("Connecting to database...");
            string connectionstring = @"Data Source = 127.0.0.1; 
                                     user id = Videojuego;
                                     password = Pollis;
                                     Initial Catalog = proyectofinal;";

            using (SqlConnection connection = new SqlConnection(connectionstring))   //esta sentencia permite que connection se destruya al salir de bloque , no hay que usar connection.close() en otras palabras
            {
                connection.Open();
                print("Done.");

                using (SqlCommand command = new SqlCommand("dbo.INSERT_POSITION", connection))
                { 
                    command.CommandType = System.Data.CommandType.StoredProcedure; //si no le ponen esto no funcionA
                    //command.Parameters.
                    command.Parameters.AddWithValue("@CoordX", transform.position.x);
                    command.Parameters.AddWithValue("@CoordY", transform.position.y);
                    command.Parameters.AddWithValue("@CoordZ", transform.position.z);
                    command.ExecuteNonQuery();
                    print("Lista escritura con un SP.");
                }
            }		
    }
}

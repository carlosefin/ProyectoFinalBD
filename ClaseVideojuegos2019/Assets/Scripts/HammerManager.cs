using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HammerManager : MonoBehaviour
{
    public int satrtingHam = 20;
    private int counter;
    private Text hammerText;
    private GameObject player;
    public Character_Movement charmov;

    // Start is called before the first frame update
    void Start()
    {
        hammerText = GetComponent <Text>();
        counter = charmov.HammerQty;
        player = GameManager.instance.Player;
        charmov = player.GetComponent<Character_Movement>();
    }

    // Update is called once per frame
    void Update()
    {
        counter = Ammo(charmov.HammerQty);
        hammerText.text = "X" + counter;
    }

    public int Ammo(int ammo)
    {
        return counter = ammo;
    }
    
}

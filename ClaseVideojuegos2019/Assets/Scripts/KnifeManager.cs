using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KnifeManager : MonoBehaviour
{
    public int startingKni = 20;
    private int counter;
    private Text knifeText;
    private GameObject player;
    public Character_Movement charmov;

    // Start is called before the first frame update
    void Start()
    {
        knifeText = GetComponent <Text>();
        counter = charmov.KnifeQty;
        player = GameManager.instance.Player;
        charmov = player.GetComponent <Character_Movement>();

        
    }

    // Update is called once per frame
    void Update()
    {
        counter = Ammo(charmov.KnifeQty);
        knifeText.text = "X" + counter;
    }

    public int Ammo(int ammo)
    {
        return counter = ammo;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        hpText = GetComponentInChildren<Text>();
    }

    private void OnEnable()
    {
        player = ObjectManager.Instance.Player.GetComponent<PlayerController>();
    }

    private void Update()
    {
        int currentHp = player.HP;
        if(playerHp != currentHp)
        {
            playerHp = currentHp;
            if (playerHp < 1)
                hpText.text = "DEAD";
            else
                hpText.text = defaultText + playerHp.ToString();
        }
    }



    PlayerController player;
    Text hpText;
    int playerHp;
    const string defaultText = "HP.";
}

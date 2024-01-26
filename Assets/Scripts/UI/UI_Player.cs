using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Player : MonoBehaviour
{

    public Player player;
    public TMP_Text ammoText;
    public TMP_Text weaponText;
    public TMP_Text killCountText;
    public TMP_Text hpText;
    public Slider hpBar;
    public TMP_Text fireMode;
    public TMP_Text curStage;
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        ammoText.text = player.weapon.currentAmmo + " / " + player.weapon.gun.maxAmmo;
        weaponText.text = player.weapon.gun.type.ToString();
        hpText.text = player.curHp + " / " + player.myMaxHp;
        hpBar.value = player.curHp / player.myMaxHp;
      
        killCountText.text = GameManager.KillCount + "/" + GameManager.ClearCount;
        curStage.text = "Stage:" + (GameManager.stageLevel);
        if (player.isSingleShot)
        {
            fireMode.text = "Single";
        }
        else
        {
            fireMode.text = "Rapid";
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GFXManager : MonoBehaviour
{
    public SpriteRenderer prSprite, plSprite, erSprite, elSprite;
    public Sprite plp, prp, pli, pri, prb, plb;
    public Sprite elp, erp, eli, eri, erb, elb, erh, elh;

    public Animator p_anim, e_anim;

    public void changePlayerSprites(string playerAction)
    {
        //Triggers the necessary animations as correctalted by the action
        p_anim.SetBool("Left", playerAction.Contains("lean left"));
        p_anim.SetBool("Duck", playerAction.Contains("duck"));
        p_anim.SetBool("Right", playerAction.Contains("lean right"));

        //Switches to the necessary sprite as correctalted by the action
        switch (playerAction)
        {
            case "right hook":
                prSprite.sprite = prp;
                plSprite.sprite = pli;
                break;
            case "left hook":
                prSprite.sprite = pri;
                plSprite.sprite = plp;
                break;
            case "hammer":
                prSprite.sprite = prp;
                plSprite.sprite = plp;
                break;
            case "block":
                prSprite.sprite = prb;
                plSprite.sprite = plb;
                break;
            case "duck":
                prSprite.sprite = pri;
                plSprite.sprite = pli;
                break;
            case "idle":
                prSprite.sprite = pri;
                plSprite.sprite = pli;
                break;
        }
    }
    
    public void changeEnemySprites(string enemyAction)
    {
        //Triggers the necessary animations as correctalted by the action
        e_anim.SetBool("Left", enemyAction.Contains("lean left"));
        e_anim.SetBool("Duck", enemyAction.Contains("duck"));
        e_anim.SetBool("Right", enemyAction.Contains("lean right"));

        //Switches to the necessary sprite as correctalted by the action
        switch (enemyAction)
        {
            case "right hook":
                erSprite.sprite = erp;
                elSprite.sprite = eli;
                e_anim.Play("rightcharge");
                break;
            case "left hook":
                erSprite.sprite = eri;
                elSprite.sprite = elp;
                e_anim.Play("leftcharge");
                break;
            case "hammer":
                erSprite.sprite = erh;
                elSprite.sprite = elh;
                e_anim.Play("hammercharge");
                break;
            case "block":
                erSprite.sprite = erb;
                elSprite.sprite = elb;
                break;
            case "duck":
                erSprite.sprite = eri;
                elSprite.sprite = eli;
                break;
            case "idle":
                erSprite.sprite = eri;
                elSprite.sprite = eli;
                break;
        }
    }
}

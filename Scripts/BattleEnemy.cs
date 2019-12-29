using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleEnemy : MonoBehaviour
{
    public Text nameText;
    public Image image;
    public Text healthText;
    public Image healthRedMask;
    public Image healthRedImage;
    public Dialogue lowHealthBattleDialogue;
    public Dialogue lowmidHealthBattleDialogue;
    public Dialogue midhighHealthBattleDialogue;
    public Dialogue highHealthBattleDialogue;
    public Dialogue currentBattleDialogue;
    public string name;
    public int maxHealth;
    private int health;
    public string[] weaknesses;
    public int power;
    public float accuracy;
    public FSM fsm;
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;

        nameText.text = name;
        healthText.text = health + " HP";

        fsm = new FSM();

        fsm.setState(new HighHealth(this));
    }

    // Update is called once per frame
    void Update()
    {
        fsm.update();
    }

    public void hitBy(Weapon weapon, Attack attack, ref bool willDie, ref bool weakToAttack, ref int damage)
    {
        for (int i = 0; i < weaknesses.Length; i++)
        {
            if (weapon.name == weaknesses[i])
            {
                weakToAttack = true;
            }
        }
        damage = attack.strength;
        //Double damage if weak to attack
        if (weakToAttack)
        {
            damage *= 2;
        }
        else
        {
            damage *= 1;
        }
        if (name == "FUSED DOG" && weapon.name == "Dog Treats")
        {
            damage *= 100;
        }
        if (damage >= health)
        {
            willDie = true;
        }
        StartCoroutine(decrementHealthBy(damage));
        return;
    }
    IEnumerator decrementHealthBy(int damage)
    {
        for (int i = 0; i < damage; i++)
        {
            Debug.Log("Health: " + health);
            if (i == damage - 1)
            {
            }
            if (health > 0)
            {
                health -= 1;
                healthText.text = health + " HP";
                float healthFraction = (float)health / maxHealth;
                healthRedMask.rectTransform.localPosition = new Vector3(60, 45 - 40 * (1 - healthFraction), 0);
                healthRedImage.rectTransform.localPosition = new Vector3(0, 80 * (1 - healthFraction), 0);
                yield return new WaitForSeconds(0.05f);
            }
            else
            {
                yield break;
            }
        }
    }

    public double percentHealthRemaining()
    {
        return (double)health / maxHealth;
    }

    public class HighHealth : State
    {
        public HighHealth(BattleEnemy battleEnemy)
        {
            this.battleEnemy = battleEnemy;
            justEntered = true;
        }

        public override void enter()
        {
            battleEnemy.currentBattleDialogue = battleEnemy.highHealthBattleDialogue;
            justEntered = false;
        }

        public override void update()
        {
            //Perform actions
            Debug.Log("High Health State");

            //Transition if needed
            double percentHealthRemaining = battleEnemy.percentHealthRemaining();
            Debug.Log("Percent Health Remaining: " + percentHealthRemaining);
            if (percentHealthRemaining < 0.76)
            {
                battleEnemy.fsm.setState(new MidHighHealth(battleEnemy));
            }
        }
    }

    public class MidHighHealth : State
    {
        public MidHighHealth(BattleEnemy battleEnemy)
        {
            this.battleEnemy = battleEnemy;
            justEntered = true;
        }

        public override void enter()
        {
            battleEnemy.currentBattleDialogue = battleEnemy.midhighHealthBattleDialogue;
            justEntered = false;
        }

        public override void update()
        {
            //Perform actions
            Debug.Log("Mid High Health State");

            //Transition if needed
            double percentHealthRemaining = battleEnemy.percentHealthRemaining();
            Debug.Log("Percent Health Remaining: " + percentHealthRemaining);
            if (percentHealthRemaining < 0.51)
            {
                battleEnemy.fsm.setState(new LowMidHealth(battleEnemy));
            }
        }
    }

    public class LowMidHealth : State
    {
        public LowMidHealth(BattleEnemy battleEnemy)
        {
            this.battleEnemy = battleEnemy;
            justEntered = true;
        }

        public override void enter()
        {
            battleEnemy.currentBattleDialogue = battleEnemy.lowmidHealthBattleDialogue;
            justEntered = false;
        }

        public override void update()
        {
            //Perform actions
            Debug.Log("Low Mid Health State");

            //Transition if needed
            double percentHealthRemaining = battleEnemy.percentHealthRemaining();
            Debug.Log("Percent Health Remaining: " + percentHealthRemaining);
            if (percentHealthRemaining < 0.26)
            {
                battleEnemy.fsm.setState(new LowHealth(battleEnemy));
            }
        }
    }

    public class LowHealth : State
    {
        private bool fadingToRed = false;
        public LowHealth(BattleEnemy battleEnemy)
        {
            this.battleEnemy = battleEnemy;
            justEntered = true;
        }

        public override void enter()
        {
            battleEnemy.power *= 2;
            battleEnemy.accuracy *= 0.5f;
            battleEnemy.currentBattleDialogue = battleEnemy.lowHealthBattleDialogue;
            //GameObject.Find("Rage Indicator").GetComponent<Image>().CrossFadeAlpha(1, 2.0f, false);
            fadingToRed = true;
            justEntered = false;
        }

        public override void update()
        {
            //Perform actions
            Debug.Log("Low Health State");
            /*if (battleEnemy.image.color == Color.white)
            {
                battleEnemy.image.CrossFadeColor(Color.red, 2f, false, false);
            }
            else if (battleEnemy.image.color == Color.red)
            {
                battleEnemy.image.CrossFadeColor(Color.white, 2f, false, false);
            }*/
            if (fadingToRed)
            {
                //Fading to red
                Debug.Log("Fading to red");
                if (battleEnemy.image.color.g >= 0.1f && battleEnemy.image.color.b >= 0.1f)
                {
                    battleEnemy.image.color = Color.Lerp(battleEnemy.image.color, Color.red, 2f * Time.deltaTime);
                    Debug.Log("Color: " + battleEnemy.image.color);
                }
                else
                {
                    fadingToRed = false;
                }
            }
            else
            {
                //Fading to white
                Debug.Log("Fading to white");
                if (battleEnemy.image.color.g <= 0.9f && battleEnemy.image.color.b <= 0.9f)
                {
                    battleEnemy.image.color = Color.Lerp(battleEnemy.image.color, Color.white, 2f * Time.deltaTime);
                    Debug.Log("Color: " + battleEnemy.image.color);
                }
                else
                {
                    fadingToRed = true;
                }
            }

            //Transition if needed
            double percentHealthRemaining = battleEnemy.percentHealthRemaining();
            Debug.Log("Percent Health Remaining: " + percentHealthRemaining);
        }
    }

}

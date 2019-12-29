/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]

public class BattleDialogueManager : MonoBehaviour
{

    public Text firstOption;
    public Text secondOption;
    public Text thirdOption;
    public Text fourthOption;
    public Text narration;
    public Image selectorImage;
    private bool atTop;
    private bool atLeft;
    private bool inFightMenu;
    private bool inBackpackMenu;
    private bool returnToMain;
    private bool enemyTurn;
    private bool attemptingToFlee;
    private bool battleOver;

    private PlayerMovement playerMovement;
    private PlayerInventory playerInventory;

    private List<GameObject> textObjectList;

    private AudioSource audioSrc;

    private void Awake()
    {
        audioSrc = GetComponent<AudioSource>();
    }
    // Use this for initialization
    void Start()
    {
        atTop = true;
        atLeft = true;
        inFightMenu = false;
        inBackpackMenu = false;
        returnToMain = false;
        enemyTurn = false;
        attemptingToFlee = false;
        battleOver = false;
        playerMovement = FindObjectOfType<PlayerMovement>();
        playerInventory = FindObjectOfType<PlayerInventory>();
        textObjectList = new List<GameObject>();
        playerMovement.enabled = false;

        initializeSomePlayerStuff();
       

    }
    void Update()
    {
        GameObject door = GameObject.Find("atDoor");
        GameObject PlayerP = GameObject.Find("Player");
        PlayerMovement playerMove = PlayerP.GetComponent<PlayerMovement>();

        playerMovement.enabled = false;
        Vector3 newPosition = selectorImage.rectTransform.localPosition;
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (atLeft == true)
            {
                newPosition.x += 64;
            }
            else
            {
                newPosition.x -= 64;
            }
            selectorImage.rectTransform.localPosition = newPosition;
            atLeft = !atLeft;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (atTop == true)
            {
                newPosition.y -= 10;
            }
            else
            {
                newPosition.y += 10;
            }
            selectorImage.rectTransform.localPosition = newPosition;
            atTop = !atTop;
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (battleOver)
            {
                narration.text = "";
                //Load new scene
                Application.Quit();
                /*
                playerMovement.enabled = true;
                SceneManager.LoadScene("RyderTesting");
                */
                /*SceneManager.LoadScene("CastleRoom");
                if (teleport == 1)
                {
                    teleport = 2;
                    playerMove.transform.position = new Vector4(1.492f, -5.501f, 0f);
                    playerMove.end.position = playerMove.transform.position;
                    playerMove.start.position = playerMove.transform.position;

                    //door.transform.position;
                }*/
/*
            }
            else if (attemptingToFlee)
            {
                string output = "";
                output += "You got away!";
                narration.text = output;
                Debug.Log(output);
                battleOver = true;
            }
            else if (enemyTurn)
            {
                //Enemy's turn
                BattleEnemy battleEnemy = FindObjectOfType<BattleEnemy>();
                narration.text = battleEnemy.nameText.text + " attacked you!";
                attackPlayer(battleEnemy.power);
                enemyTurn = false;
                returnToMain = true;
                //AudioClip audioClip = (AudioClip)AssetDatabase.LoadAssetAtPath("Assets/Sounds/" + battleEnemy.nameText.text + ".mp3", typeof(AudioClip));
                AudioClip audioClip = (AudioClip)Resources.Load("Sounds/" + battleEnemy.nameText.text, typeof(AudioClip));

                audioSrc.clip = audioClip;
                if (!audioSrc.isPlaying)
                {
                    audioSrc.Play();
                }
            }
            else if (returnToMain)
            {
                firstOption.text = "Fight";
                secondOption.text = "Backpack";
                thirdOption.text = "Talk";
                fourthOption.text = "Run";
                narration.text = "";
                atTop = true;
                atLeft = true;
                selectorImage.rectTransform.localPosition = new Vector3(-60.0f, 5.5f, 0);
                selectorImage.enabled = true;
                returnToMain = false;
            }
            else if (inFightMenu)
            {
                int menuOption;
                //Use first attack
                if (atTop == true && atLeft == true)
                {
                    menuOption = 0;
                }
                else if (atTop == true && atLeft == false)
                {
                    menuOption = 1;
                }
                else if (atTop == false && atLeft == true)
                {
                    menuOption = 2;
                }
                else
                {
                    menuOption = 3;
                }
                string itemName = textObjectList[menuOption].GetComponent<Text>().text;
                firstOption.text = "";
                secondOption.text = "";
                thirdOption.text = "";
                fourthOption.text = "";
                narration.text = "You used your " + itemName + "!";
                selectorImage.enabled = false;
                inFightMenu = false;
                returnToMain = false;
                enemyTurn = true;
                attackWith(itemName);
                for (int i = 0; i < textObjectList.Count; i++)
                {
                    Destroy(textObjectList[i]);
                }
            }
            else if (inBackpackMenu)
            {
                int menuOption;
                //Use first attack
                if (atTop == true && atLeft == true)
                {
                    menuOption = 0;
                }
                else if (atTop == true && atLeft == false)
                {
                    menuOption = 1;
                }
                else if (atTop == false && atLeft == true)
                {
                    menuOption = 2;
                }
                else
                {
                    menuOption = 3;
                }
                string itemName = textObjectList[menuOption].GetComponent<Text>().text;
                Item[] items = playerInventory.backpack.getAllItems();
                for (int i = 0; i < items.Length; i++)
                {
                    if (items[i].name == itemName)
                    {
                        if (items[i] is Weapon)
                        {
                            narration.text = "You equipped your " + itemName + "!";
                            playerInventory.backpack.setEquippedWeapon((Weapon)items[i]);
                        }
                    }
                }
                firstOption.text = "";
                secondOption.text = "";
                thirdOption.text = "";
                fourthOption.text = "";
                selectorImage.enabled = false;
                inBackpackMenu = false;
                returnToMain = false;
                enemyTurn = true;
                for (int i = 0; i < textObjectList.Count; i++)
                {
                    Destroy(textObjectList[i]);
                }
            }
            else
            {
                //Fight
                if (atTop == true && atLeft == true)
                {
                    textObjectList = new List<GameObject>();
                    if (playerInventory.backpack.isEmpty())
                    {
                        int rowIndex = 0;
                        showItem("Fists", rowIndex);
                    }
                    else
                    {
                        Weapon weapon = playerInventory.backpack.getEquippedWeapon();
                        for (int i = 0; i < weapon.attacks.Count; i++)
                        {
                            showItem(weapon.attacks[i].name, i);
                        }
                    }
                    firstOption.text = "";
                    secondOption.text = "";
                    thirdOption.text = "";
                    fourthOption.text = "";
                    inFightMenu = true;
                }
                //Backpack
                if (atTop == true && atLeft == false)
                {
                    textObjectList = new List<GameObject>();
                    Item[] items = playerInventory.backpack.getAllItems();
                    for (int i = 0; i < items.Length; i++)
                    {
                        showItem(items[i].name, i);
                    }
                    firstOption.text = "";
                    secondOption.text = "";
                    thirdOption.text = "";
                    fourthOption.text = "";
                    inBackpackMenu = true;
                    selectorImage.rectTransform.localPosition = new Vector3(-60.0f, 5.5f, 0);
                    atLeft = !atLeft;
                }
                //Talk
                if (atTop == false && atLeft == true)
                {

                }
                //Run
                if (atTop == false && atLeft == false)
                {
                    firstOption.text = "";
                    secondOption.text = "";
                    thirdOption.text = "";
                    fourthOption.text = "";
                    selectorImage.enabled = false;
                    string output = "";
                    output += "Attempting to flee...";
                    narration.text = output;
                    Debug.Log(output);
                    attemptingToFlee = true;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            if (inFightMenu)
            {
                for (int i = 0; i < textObjectList.Count; i++)
                {
                    Destroy(textObjectList[i]);
                }
                firstOption.text = "Fight";
                secondOption.text = "Backpack";
                thirdOption.text = "Talk";
                fourthOption.text = "Run";
                inFightMenu = false;
                returnToMain = false;
            }
            else if (inBackpackMenu)
            {
                for (int i = 0; i < textObjectList.Count; i++)
                {
                    Destroy(textObjectList[i]);
                }
                firstOption.text = "Fight";
                secondOption.text = "Backpack";
                thirdOption.text = "Talk";
                fourthOption.text = "Run";
                inBackpackMenu = false;
                returnToMain = false;
            }
        }
    }

    void attackWith(string itemName)
    {
        BattleEnemy battleEnemy = FindObjectOfType<BattleEnemy>();
        bool willDie = false;
        bool weakToAttack = false;
        battleEnemy.hitBy(itemName, ref willDie, ref weakToAttack);
        if (willDie == true)
        {
            string output = "";
            output += battleEnemy.name;
            output += " has been defeated!";
            narration.text = output;
            Debug.Log(output);
            battleOver = true;
        }
        else if (weakToAttack)
        {
            string output = "";
            output += "A " + itemName + "! His weakness!";
            narration.text = output;
            Debug.Log(output);
        }
        AudioClip audioClip = (AudioClip)Resources.Load("Sounds/" + itemName, typeof(AudioClip));
        audioSrc.clip = audioClip;
        if (!audioSrc.isPlaying)
        {
            audioSrc.Play();
        }
        StartCoroutine(playAnimation(itemName));
    }

    void attackPlayer(int power)
    {
        PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
        bool willDie = playerHealth.hitBy(power);
        if (willDie == true)
        {
            string output = "";
            output += "You have been defeated!";
            narration.text = output;
            Debug.Log(output);
            battleOver = true;
        }
    }

    public IEnumerator playAnimation(string itemName)
    {
        Image attackAnimation = GameObject.Find("Attack Animation").GetComponent<Image>();
        Sprite[] sprites = new Sprite[12];
        for (int i = 1; i < 13; i++)
        {
            //Sprite sprite = (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Animations/" + itemName + "/" + i + ".png", typeof(Sprite));
            Sprite sprite = (Sprite)Resources.Load("Animations/" + itemName + "/" + i, typeof(Sprite));
            if (sprite == null)
            {
                yield break;
            }
            sprites[i - 1] = sprite;
        }
        float animationSpeed = 0.1f;
        for (int i = 0; i < sprites.Length; i++)
        {
            attackAnimation.sprite = sprites[i];
            attackAnimation.enabled = true;
            yield return new WaitForSeconds(animationSpeed);
        }
        attackAnimation.enabled = false;
    }

    //Handles the formatting for items when displaying them in the battle menu
    public void showItem(string text, int number)
    {

        GameObject newTextObject = new GameObject(text.Replace(" ", "-"), typeof(RectTransform));
        var newText = newTextObject.AddComponent<Text>();

        newText.text = text;
        newText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        newText.color = new Color(32.0f / 255.0f, 32.0f / 255.0f, 32.0f / 255.0f);
        newText.alignment = TextAnchor.UpperLeft;
        newText.fontSize = 14;
        newText.resizeTextForBestFit = true;
        newText.resizeTextMinSize = 1;
        newText.resizeTextMaxSize = 300;

        newTextObject.transform.SetParent(transform);
        newTextObject.GetComponent<RectTransform>().sizeDelta = new Vector2(75.0f, 12.0f);
        if (number % 2 == 0)
        {
            float yVal = 5 - 5 * number;
            newTextObject.transform.localPosition = new Vector3(-10.0f, yVal, 0.0f);
        }
        else
        {
            float yVal = 5 - 5 * (number - 1);
            newTextObject.transform.localPosition = new Vector3(50.0f, yVal, 0.0f);
        }
        newTextObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        textObjectList.Add(newTextObject);
    }

    public void initializeSomePlayerStuff()
    {
        List<Attack> attacks;
        Attack attack1, attack2, attack3, attack4;
        attacks = new List<Attack>();
        attack1 = new Attack("Swing", 20, 80);
        attack2 = new Attack("Bunt", 5, 100);
        attack3 = new Attack("Smash", 30, 50);
        attacks.Add(attack1);
        attacks.Add(attack2);
        attacks.Add(attack3);
        Weapon weapon = new Weapon("Baseball Bat", attacks);
        playerInventory.backpack.addItem(weapon);
        playerInventory.backpack.setEquippedWeapon(weapon);

        attacks = new List<Attack>();
        attack1 = new Attack("Basic Sling", 20, 80);
        attack2 = new Attack("Crazy Yo-Yo skills", 5, 100);
        attacks.Add(attack1);
        attacks.Add(attack2);
        weapon = new Weapon("Yo-Yo", attacks);
        playerInventory.backpack.addItem(weapon);

        attacks = new List<Attack>();
        attack1 = new Attack("Horn", 20, 80);
        attack2 = new Attack("Crash", 5, 100);
        attack3 = new Attack("Throw", 20, 80);
        attacks.Add(attack1);
        attacks.Add(attack2);
        attacks.Add(attack3);
        weapon = new Weapon("Bicycle", attacks);
        playerInventory.backpack.addItem(weapon);
    }

}
*/
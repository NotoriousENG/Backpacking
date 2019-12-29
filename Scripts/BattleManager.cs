using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]

public class BattleManager : MonoBehaviour
{

    public Text narration;
    public Image selectorImage;
    private int selectorIndex;

    private PlayerMovement playerMovement;
    private PlayerInventory playerInventory;
    private PlayerProgress playerProgress;

    private List<GameObject> textObjectList;

    private AudioSource audioSrc;

    private bool inMainMenu;
    private bool inFightMenu;
    private bool inBackpackMenu;
    private bool menuLevel = false;
    private bool inTalkMenu;
    private bool inRunMenu;

    private bool enemyTurn;
    private bool advanceEnemyText;

    private bool advanceRunningText;

    private bool battleOver;
    private bool battleOverPlayerDefeated;

    private bool firstTimeInMenu;

    int numOfSentences;
    int currentIndex;

    private bool userInteractionEnabled = true;

    private void Awake()
    {
        audioSrc = GetComponent<AudioSource>();
    }
    // Use this for initialization
    void Start()
    {
        playerProgress = GameObject.Find("Player").GetComponent<PlayerProgress>();
        playerMovement = FindObjectOfType<PlayerMovement>();
        playerInventory = FindObjectOfType<PlayerInventory>();
        textObjectList = new List<GameObject>();
        playerMovement.enabled = false;
        
        inMainMenu = true;
        inFightMenu = false;
        inBackpackMenu = false;
        inTalkMenu = false;
        inRunMenu = false;

        enemyTurn = false;
        advanceEnemyText = false;

        advanceRunningText = false;

        playerProgress.recentlyWon = false;
        playerProgress.recentlyRan = false;

        battleOver = false;
        battleOverPlayerDefeated = false;

        firstTimeInMenu = true;

        //initializeSomePlayerStuff();

        GameObject battleInterface = GameObject.Find("Battle Interface"); // Find the Canvas
        GameObject temp = GameObject.Find("Enemy");
        Vector3 tempPos = temp.transform.localPosition;
        Vector3 tempScale = temp.transform.localScale;
        Destroy(GameObject.Find("Enemy"));  // Destroy the default enemy
        GameObject enemyObject = GameObject.Find("Enemy(Clone)"); // the enemyObject is the passed enemy
        enemyObject.transform.parent = battleInterface.transform; // UI.transform = UI.transform (probs not needed)
        enemyObject.transform.SetAsFirstSibling(); // Draw this first
        RectTransform rectTransform = enemyObject.GetComponent<RectTransform>();
        // tempPos.y -= 9;
        rectTransform.localPosition = tempPos; //new Vector3(7.5f, 5.0f);
        rectTransform.localScale = tempScale; //new Vector3(0.25f, 0.25f, 0.25f);

        GameObject image = enemyObject.transform.Find("Image").gameObject;
        RectTransform imageRectTransform = image.GetComponent<RectTransform>();
        imageRectTransform.localPosition = new Vector3(0, 65.0f);
        //imageRectTransform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
        imageRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, image.GetComponent<Image>().sprite.rect.width);
        imageRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, image.GetComponent<Image>().sprite.rect.height);
        if (image.GetComponent<Image>().sprite.rect.width < 32 || image.GetComponent<Image>().sprite.rect.height < 32)
        {
            imageRectTransform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
        }
        else if (image.GetComponent<Image>().sprite.rect.width < 64 || image.GetComponent<Image>().sprite.rect.height < 64)
        {
            imageRectTransform.localScale = new Vector3(2.25f, 2.25f, 2.25f);
        }
        else if (image.GetComponent<Image>().sprite.rect.width < 96 || image.GetComponent<Image>().sprite.rect.height < 96)
        {
            imageRectTransform.localScale = new Vector3(2f, 2f, 2f);
        }
        else if (image.GetComponent<Image>().sprite.rect.width < 128 || image.GetComponent<Image>().sprite.rect.height < 128)
        {
            imageRectTransform.localScale = new Vector3(1.75f, 1.75f, 1.75f);
        }
        else if (image.GetComponent<Image>().sprite.rect.width < 160 || image.GetComponent<Image>().sprite.rect.height < 160)
        {
            imageRectTransform.localScale = new Vector3(1.4f, 1.4f, 1.4f);
        }
        else if (image.GetComponent<Image>().sprite.rect.width < 192 || image.GetComponent<Image>().sprite.rect.height < 192)
        {
            imageRectTransform.localScale = new Vector3(0.9f, 0.9f);
        }
        else if (image.GetComponent<Image>().sprite.rect.width < 224 || image.GetComponent<Image>().sprite.rect.height < 224)
        {
            imageRectTransform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }
        else if (image.GetComponent<Image>().sprite.rect.width <= 256 || image.GetComponent<Image>().sprite.rect.height <= 256)
        {
            imageRectTransform.localScale = new Vector3(0.375f, 0.375f, 0.375f);
        }

        //temp = GameObject.Find("Background");
        //tempPos = temp.transform.localPosition;
        //tempScale = temp.transform.localScale;
        Destroy(GameObject.Find("Background"));  // Destroy the default enemy
        GameObject battleBackgroundTransferred = GameObject.Find("Background(Clone)"); // the enemyObject is the passed enemy
        rectTransform = battleBackgroundTransferred.GetComponent<RectTransform>();
        battleBackgroundTransferred.transform.parent = battleInterface.transform; // UI.transform = UI.transform (probs not needed)
        battleBackgroundTransferred.transform.SetAsFirstSibling(); // Draw this first
        rectTransform.localPosition = new Vector2(0, 0);
        rectTransform.localScale = new Vector3(0.15f, 0.15f, 0.15f);


    }
    void Update()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();

        playerMovement.enabled = false;
        //Handles the selector's movement and index
        selectorControls();

        //END OF BATTLE
        if (battleOver)
        {
            if (userInteractionEnabled && Input.GetKeyDown(KeyCode.Z))
            {
                narration.text = "";
                //Load new scene
                playerMovement.enabled = true;
                SceneManager.LoadScene("Game World");
                /*SceneManager.LoadScene("CastleRoom");
                if (teleport == 1)
                {
                    teleport = 2;
                    playerMove.transform.position = new Vector4(1.492f, -5.501f, 0f);
                    playerMove.end.position = playerMove.transform.position;
                    playerMove.start.position = playerMove.transform.position;

                    //door.transform.position;
                }*/
            }
        }
        else if (battleOverPlayerDefeated)
        {
            if (playerProgress.completedFirstBernieGangEncounter == true && playerProgress.completedDogEncounter == false)
            {
                playerProgress.recentlyWon = true;
                battleOver = true;
            }
            else if (userInteractionEnabled && Input.GetKeyDown(KeyCode.Z))
            {
                narration.text = "";
                DestroyAllGameObjects();
                //Load new scene
                SceneManager.LoadScene("Start Menu");
                /*SceneManager.LoadScene("CastleRoom");
                if (teleport == 1)
                {
                    teleport = 2;
                    playerMove.transform.position = new Vector4(1.492f, -5.501f, 0f);
                    playerMove.end.position = playerMove.transform.position;
                    playerMove.start.position = playerMove.transform.position;

                    //door.transform.position;
                }*/
            }
        }

        //ENEMY TURN
        else if (enemyTurn)
        {
            if (advanceEnemyText == true)
            {
                if (userInteractionEnabled && Input.GetKeyDown(KeyCode.Z))
                {
                    enemyTurn = false;
                    inMainMenu = true;
                    firstTimeInMenu = true;
                    narration.text = "";
                    advanceEnemyText = false;
                    selectorImage.rectTransform.localPosition = new Vector3(-60.0f, 5.5f, 0);
                    selectorIndex = 0;
                }
            }
            else
            {
                if (userInteractionEnabled && Input.GetKeyDown(KeyCode.Z))
                {
                    //Enemy's turn
                    BattleEnemy battleEnemy = FindObjectOfType<BattleEnemy>();
                    narration.text = battleEnemy.nameText.text + " attacked you!";
                    enemyAttack();
                    //AudioClip audioClip = (AudioClip)AssetDatabase.LoadAssetAtPath("Assets/Sounds/" + battleEnemy.nameText.text + ".mp3", typeof(AudioClip));
                    AudioClip audioClip = (AudioClip)Resources.Load("Sounds/" + battleEnemy.nameText.text, typeof(AudioClip));

                    audioSrc.clip = audioClip;
                    if (!audioSrc.isPlaying)
                    {
                        audioSrc.Play();
                    }

                    advanceEnemyText = true;
                }
            }
        }

        //PLAYER TURN
        else
        {
            //Load the proper menu
            if (inMainMenu)
            {
                mainMenu();
            }
            else if (inFightMenu)
            {
                fightMenu();
            }
            else if (inBackpackMenu)
            {
                backpackMenu(menuLevel, false);
            }
            else if (inTalkMenu)
            {
                talkMenu();
            }
            else if (inRunMenu)
            {
                runMenu();
            }
        }


    }

    public void cleanUp()
    {
        if (textObjectList != null)
        {
            for (int i = 0; i < textObjectList.Count; i++)
            {
                Destroy(textObjectList[i]);
            }
        }
    }

    public void mainMenu()
    {
        if (selectorImage != null)
        {
            selectorImage.enabled = true;
        }
        if (firstTimeInMenu)
        {
            cleanUp();
            textObjectList = new List<GameObject>();
            showItem("Fight", 0);
            showItem("Backpack", 1);
            showItem("Talk", 2);
            showItem("Run", 3);
            firstTimeInMenu = false;
        }

        if (userInteractionEnabled && Input.GetKeyDown(KeyCode.Z))
        {
            switch (selectorIndex)
            {
                case 0:
                    inFightMenu = true;
                    break;
                case 1:
                    inBackpackMenu = true;
                    break;
                case 2:
                    inTalkMenu = true;
                    break;
                case 3:
                    inRunMenu = true;
                    break;
            }
            selectorImage.rectTransform.localPosition = new Vector3(-60.0f, 5.5f, 0);
            selectorIndex = 0;
            inMainMenu = false;
            firstTimeInMenu = true;
        }
    }
    public void fightMenu()
    {
        if (firstTimeInMenu)
        {
            cleanUp();
            textObjectList = new List<GameObject>();
            firstTimeInMenu = false;
            if (playerInventory.backpack.isEmpty() || playerInventory.backpack.getEquippedWeapon() == null)
            {
                Debug.Log("Empty backpack");
                int rowIndex = 0;
                showItem("Fists", rowIndex);
            }
            else
            {
                Debug.Log("Non-empty backpack");
                Weapon weapon = playerInventory.backpack.getEquippedWeapon();
                for (int i = 0; i < weapon.attacks.Count; i++)
                {
                    showItem(weapon.attacks[i].name, i);
                }
            }
        }
        if (userInteractionEnabled && Input.GetKeyDown(KeyCode.Z))
        {
            string itemName = textObjectList[selectorIndex].GetComponent<Text>().text;
            Weapon weapon = playerInventory.backpack.getEquippedWeapon();
            Attack attack = new Attack();
            //Weapon from backpack
            if (weapon != null)
            {
                for (int i = 0; i < weapon.attacks.Count; i++)
                {
                    if (weapon.attacks[i].name == itemName)
                    {
                        attack = weapon.attacks[i];
                    }
                }
            }
            //Fists
            else
            {
                List<Attack> attackList = new List<Attack>();
                attack = new Attack("Fists", 10, 100);
                attackList.Add(attack);
                weapon = new Weapon("Fists", attackList);
            }

            for (int i = 0; i < textObjectList.Count; i++)
            {
                Destroy(textObjectList[i]);
            }
            narration.text = "You used your " + itemName + "!";
            selectorImage.enabled = false;
            attackWith(weapon, attack);
            inFightMenu = false;
            enemyTurn = true;
        }
        if (userInteractionEnabled && Input.GetKeyDown(KeyCode.X))
        {
            inFightMenu = false;
            inMainMenu = true;
            firstTimeInMenu = true;
        }
    }
    public void backpackMenu(bool menuLevel, bool overRide)
    {
        Item[] items = playerInventory.backpack.getAllItems();
        if (firstTimeInMenu || overRide)
        {
            cleanUp(); // clears menu
            firstTimeInMenu = false;
            textObjectList = new List<GameObject>();

            int menuLength = items.Length;  // get menu length, max should be 4

            int i = 0;

            if (!menuLevel) // if in menu level 0
            {
                i = 0;
            }

            if (menuLevel && items.Length > 4) // if in menu level 1
            {
                i = 4;
            }

            int j = 0;

            while (i<menuLength)
            {
                showItem(items[i].name, j); // prints to location
                i++;
                j++;
                if(i == 4 || i == 8) // if you have printed 4 items
                {
                    break;
                }
            }
        }
        if (userInteractionEnabled && Input.GetKeyDown(KeyCode.Z))
        {
            string itemName = textObjectList[selectorIndex].GetComponent<Text>().text;
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i].name == itemName)
                {
                    if (items[i] is Weapon)
                    {
                        for (int j = 0; j < textObjectList.Count; j++)
                        {
                            Destroy(textObjectList[j]);
                        }
                        selectorImage.enabled = false;
                        narration.text = "You equipped your " + itemName + "!";
                        playerInventory.backpack.setEquippedWeapon((Weapon)items[i]);
                        inBackpackMenu = false;
                        enemyTurn = true;
                        return;
                    }
                }
            }
        }
        if (userInteractionEnabled && Input.GetKeyDown(KeyCode.X))
        {
            inBackpackMenu = false;
            inMainMenu = true;
            firstTimeInMenu = true;
        }
    }
    public void talkMenu()
    {
        BattleEnemy battleEnemy = FindObjectOfType<BattleEnemy>();
        if (firstTimeInMenu)
        {
            cleanUp();
            numOfSentences = battleEnemy.currentBattleDialogue.sentences.Length;
            currentIndex = 0;
            narration.text = battleEnemy.currentBattleDialogue.sentences[currentIndex];
            numOfSentences -= 1;
            currentIndex += 1;

            selectorImage.enabled = false;
            firstTimeInMenu = false;
        }
        else
        {
            if (userInteractionEnabled && Input.GetKeyDown(KeyCode.Z))
            {
                if (numOfSentences > 0)
                {
                    narration.text = battleEnemy.currentBattleDialogue.sentences[currentIndex];
                    numOfSentences -= 1;
                    currentIndex += 1;
                }
                else
                {
                    narration.text = "";
                    inTalkMenu = false;
                    inMainMenu = true;
                    firstTimeInMenu = true;
                }
            }
        }
    }
    public void runMenu()
    {
        if (firstTimeInMenu)
        {
            cleanUp();
            firstTimeInMenu = false;
        }
        if (advanceRunningText == true)
        {
            if (userInteractionEnabled && Input.GetKeyDown(KeyCode.Z))
            {
                string output = "";
                output += "You got away!";
                narration.text = output;
                Debug.Log(output);
                advanceRunningText = false;
                playerProgress.recentlyRan = true;
                battleOver = true;
            }
        }
        else
        {
            selectorImage.enabled = false;
            string output = "";
            output += "Attempting to flee...";
            narration.text = output;
            Debug.Log(output);
            advanceRunningText = true;
        }
    }

    void attackWith(Weapon weapon, Attack attack)
    {
        BattleEnemy battleEnemy = FindObjectOfType<BattleEnemy>();

        float random = Random.Range(0.0f, 1.0f);
        Debug.Log("Attack accuracy: " + attack.accuracy + "     Random val: " + random);
        //Hit
        if (attack.accuracy >= random)
        {
            bool willDie = false;
            bool weakToAttack = false;
            int damage = 0;
            battleEnemy.hitBy(weapon, attack, ref willDie, ref weakToAttack, ref damage);
            StartCoroutine(disableUserInteractionForSeconds(damage * 0.05f));
            if (willDie == true)
            {
                string output = "";
                output += battleEnemy.name;
                output += " has been defeated!";
                narration.text = output;
                Debug.Log(output);
                playerProgress.recentlyWon = true;
                battleOver = true;
            }
            else if (weakToAttack)
            {
                string output = "";
                output += "A " + weapon.name + "! His weakness!";
                narration.text = output;
                Debug.Log(output);
            }
        }
        //Miss
        else
        {
            string output = "";
            output += "Your attack missed!";
            narration.text = output;
            Debug.Log(output);
        }
        AudioClip audioClip = (AudioClip)Resources.Load("Sounds/" + weapon.name, typeof(AudioClip));
        audioSrc.clip = audioClip;
        if (!audioSrc.isPlaying)
        {
            audioSrc.Play();
        }
        StartCoroutine(playAnimation(weapon.name));
    }

    void enemyAttack()
    {
        BattleEnemy battleEnemy = FindObjectOfType<BattleEnemy>();

        float random = Random.Range(0.0f, 1.0f);
        //Hit
        if (battleEnemy.accuracy >= random)
        {
            PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
            bool willDie = playerHealth.hitBy(battleEnemy.power);
            StartCoroutine(disableUserInteractionForSeconds(battleEnemy.power * 0.05f));
            if (willDie == true)
            {
                string output = "";
                output += "You have been defeated!";
                narration.text = output;
                Debug.Log(output);
                battleOverPlayerDefeated = true;
            }
        }
        //Miss
        else
        {
            string output = "";
            output += "The enemy missed!";
            narration.text = output;
            Debug.Log(output);
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

    public void selectorControls()
    {
        Vector3 newPosition = new Vector3(0,0);
        if (selectorImage != null)
        {
            newPosition = selectorImage.rectTransform.localPosition;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (selectorIndex % 2 == 0) // if on left
            {
                newPosition.x += 64;    // move to the right
                selectorIndex += 1;
            }
            else // if on right
            {
                newPosition.x -= 64;    // move to left
                selectorIndex -= 1;
            }
            if (selectorImage != null) // certain screens the selector has to be hidden
            {
                selectorImage.rectTransform.localPosition = newPosition;
            }
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                //At top
                if (selectorIndex == 0 || selectorIndex == 1) // if at top
                {
                    if (inBackpackMenu) // if at the top and in the battle menu
                    {
                        menuLevel = !menuLevel;
                        backpackMenu(menuLevel,true);
                    }
                    newPosition.y -= 10;    // move to bottom
                    selectorIndex += 2;
                }
                else// if at bottom
                {
                    newPosition.y += 10;    // move to top
                    selectorIndex -= 2;
                }
                if (selectorImage != null)
                {
                    selectorImage.rectTransform.localPosition = newPosition;
                }
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                //At bottom
                if (selectorIndex == 2 || selectorIndex == 3)
                {
                    if (inBackpackMenu) // if at the bottom and in the battle menu
                    {
                        menuLevel = !menuLevel;
                        backpackMenu(menuLevel, true);
                    }

                    newPosition.y += 10;
                    selectorIndex -= 2;
                }
                else
                {
                    newPosition.y -= 10;
                    selectorIndex += 2;
                }
                if (selectorImage != null)
                {
                    selectorImage.rectTransform.localPosition = newPosition;
                }
            }
        }
    }

    public void initializeSomePlayerStuff()
    {
        playerInventory.backpack = new Backpack();
        List<Attack> attacks;
        Attack attack1, attack2, attack3, attack4;
        attacks = new List<Attack>();
        attack1 = new Attack("Swing", 20, 80);
        attack2 = new Attack("Bunt", 5, 100);
        attack3 = new Attack("Smash", 30, 50);
        attacks.Add(attack1);
        attacks.Add(attack2);
        attacks.Add(attack3);
        Weapon weapon = new Weapon("Bat", attacks);
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

    IEnumerator disableUserInteractionForSeconds(float seconds)
    {
        while(true)
        {
            Debug.Log(seconds);
            if (seconds > 0)
            {
                userInteractionEnabled = false;
                seconds -= 1;
                yield return new WaitForSeconds(1.0f);
            }
            else
            {
                userInteractionEnabled = true;
                yield break;
            }
        }
    }

    public void DestroyAllGameObjects()
    {
        GameObject[] GameObjects = (FindObjectsOfType<GameObject>() as GameObject[]);

        for (int i = 0; i < GameObjects.Length; i++)
        {
            Destroy(GameObjects[i]);
        }
    }

}

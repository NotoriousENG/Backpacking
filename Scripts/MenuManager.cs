using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{

    public Animator animator;
    public Image selectorImage;
    public bool isOpen;
    private bool inMain;
    private bool inBackpack;
    private bool inPhone;
    private bool inItemDetails;
    private bool inObjectiveDetails;
    private PlayerMovement playerMovement;
    private PlayerObjectives playerObjectives;
    private PlayerInventory playerInventory;
    private List<GameObject> textObjectList;
    private talkWithButton talkwithButton;
    private bool atLeft;
    Item selectedItem;
    Objective selectedObjective;

    // Use this for initialization
    void Start()
    {
        selectorImage.rectTransform.localPosition = new Vector3(-80.0f, 24.0f);
        atLeft = true;

        animator.SetBool("IsOpen", false);
        isOpen = false;
        inMain = false;
        inBackpack = false;
        inPhone = false;
        inItemDetails = false;
        inObjectiveDetails = false;
        
        playerMovement = FindObjectOfType<PlayerMovement>();
        playerInventory = FindObjectOfType<PlayerInventory>();
        playerObjectives = FindObjectOfType<PlayerObjectives>();
        textObjectList = new List<GameObject>();
        talkwithButton = FindObjectOfType<talkWithButton>();
        atLeft = true;
    }

    private void Update()
    {
        if (isOpen)
        {
            playerMovement.enabled = false;
            talkwithButton.enabled = false;
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            showItem("Backpack", 0);
            if (playerInventory.backpack.hasItem("Phone"))
            {
                showItem("Phone", 1);
            }
            animator.SetBool("IsOpen", true);
            isOpen = true;
            inMain = true;
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (inItemDetails)
            {
                goToBackpack();
            }
            else if (inBackpack)
            {
                goToMain();
            }
            else if (inObjectiveDetails)
            {
                goToPhone();
            }
            else if (inPhone)
            {
                goToMain();
            }
            else if (inMain)
            {
                animator.SetBool("IsOpen", false);
                isOpen = false;
                playerMovement.enabled = true;
                talkwithButton.enabled = true;
                cleanUp();
                textObjectList = new List<GameObject>();
            }
        }
        if (Input.GetKeyDown(KeyCode.Z) && isOpen == true)
        {
            if (inItemDetails)
            {

            }
            else if (inBackpack)
            {
                goToItemDetails();
            }
            else if (inObjectiveDetails)
            {

            }
            else if (inPhone)
            {
                goToObjectiveDetails();
            }
            else if (inMain)
            {
                Vector3 pos = selectorImage.rectTransform.localPosition;
                if (pos.x == 0 && pos.y == 24)
                {
                    goToPhone();
                }
                else
                {
                    goToBackpack();
                }

            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)) {
            if (inMain || inBackpack || inPhone)
            {
                Vector3 newPosition = selectorImage.rectTransform.localPosition;
                int lowerYBound = 24;
                int numOfMenuOptions = textObjectList.Count;
                if (numOfMenuOptions > 0)
                {
                    //Even number of menu options
                    if (numOfMenuOptions % 2 == 0)
                    {
                        lowerYBound = 24 - 16 * (textObjectList.Count / 2 - 1);
                    }
                    //Odd number of menu options
                    else
                    {
                        Debug.Log("TextObjListcount / 2" + textObjectList.Count / 2);
                        lowerYBound = 24 - 16 * (textObjectList.Count / 2);
                    }
                }
                //Bottom row is where left/right matters
                if (newPosition.y == lowerYBound)
                {
                    //Even
                    if (numOfMenuOptions % 2 == 0)
                    {
                        if (atLeft == true)
                        {
                            newPosition.x += 80;
                            atLeft = !atLeft;
                        }
                        else
                        {
                            newPosition.x -= 80;
                            atLeft = !atLeft;
                        }
                    }
                    //Odd
                    else
                    {

                    }
                }
                else
                {
                    if (atLeft == true)
                    {
                        newPosition.x += 80;
                        atLeft = !atLeft;
                    }
                    else
                    {
                        newPosition.x -= 80;
                        atLeft = !atLeft;
                    }
                }
                selectorImage.rectTransform.localPosition = newPosition;
            }
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (inMain || inBackpack || inPhone)
            {
                Vector3 newPosition = selectorImage.rectTransform.localPosition;
                int lowerYBound = 24;
                int numOfMenuOptions = textObjectList.Count;
                if (numOfMenuOptions > 0)
                {
                    //Even number of menu options
                    if (numOfMenuOptions % 2 == 0)
                    {
                        lowerYBound = 24 - 16 * (textObjectList.Count / 2 - 1);
                    }
                    //Odd number of menu options
                    else
                    {
                        lowerYBound = 24 - 16 * (textObjectList.Count / 2);
                    }
                }
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    newPosition.y += 16;
                    if (newPosition.y > 24)
                    {
                        newPosition.y = lowerYBound;
                    }
                }
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    newPosition.y -= 16;
                    if (newPosition.y < lowerYBound)
                    {
                        newPosition.y = 24;
                    }
                }
                selectorImage.rectTransform.localPosition = newPosition;
            }
        }
    }

    private void goToItemDetails()
    {
        inItemDetails = true;
        inBackpack = false;
        inPhone = false;
        inMain = false;
        getSelectedItem();
        cleanUp();
        textObjectList = new List<GameObject>();
        selectorImage.enabled = false;
        showItemWithDescriptionAndImage(selectedItem.name, selectedItem.description, selectedItem.sprite);
        if (selectedItem is Weapon)
        {
            showAttackDescriptions(((Weapon)selectedItem).attacks);
        }
        selectorImage.rectTransform.localPosition = new Vector3(-80.0f, 24.0f);
        atLeft = true;
    }
    private void goToBackpack()
    {
        inItemDetails = false;
        inBackpack = true;
        inPhone = false;
        inMain = false;
        cleanUp();
        textObjectList = new List<GameObject>();
        if (playerInventory.backpack.isEmpty())
        {
            Debug.Log("Empty bag");
            selectorImage.enabled = false;
        }
        else
        {
            Debug.Log("Non-empty bag");
            selectorImage.enabled = true;
        }
        Item[] items = playerInventory.backpack.getAllItems();
        for (int i = 0; i < items.Length; i++)
        {
            Debug.Log("Inventory: " + i);
            showItemWithImage(items[i].name, items[i].sprite, i);
        }
        selectorImage.rectTransform.localPosition = new Vector3(-80.0f, 24.0f);
        atLeft = true;
    }

    private void goToObjectiveDetails()
    {
        inObjectiveDetails = true;
        inBackpack = false;
        inPhone = false;
        inMain = false;
        getSelectedObjective();
        cleanUp();
        textObjectList = new List<GameObject>();
        selectorImage.enabled = false;
        showObjectiveWithDescription(selectedObjective.name, selectedObjective.description);
        selectorImage.rectTransform.localPosition = new Vector3(-80.0f, 24.0f);
        atLeft = true;
    }

    private void goToPhone()
    {
        inObjectiveDetails = false;
        inPhone = true;
        inBackpack = false;
        inMain = false;
        cleanUp();
        textObjectList = new List<GameObject>();
        if (playerObjectives.phone.isEmpty())
        {
            Debug.Log("Empty objectives");
            selectorImage.enabled = false;
        }
        else
        {
            Debug.Log("Non-empty objectives");
            selectorImage.enabled = true;
        }
        Objective[] objectives = playerObjectives.phone.getAllObjectives();
        for (int i = 0; i < objectives.Length; i++)
        {
            Debug.Log("Inventory: " + i);
            showItem(objectives[i].name, i);
        }
        selectorImage.rectTransform.localPosition = new Vector3(-80.0f, 24.0f);
        atLeft = true;

    }

    private void goToMain()
    {
        inBackpack = false;
        inPhone = false;
        inMain = true;
        cleanUp();
        textObjectList = new List<GameObject>();
        showItem("Backpack", 0);
        if (playerInventory.backpack.hasItem("Phone"))
        {
            showItem("Phone", 1);
        }
        selectorImage.rectTransform.localPosition = new Vector3(-80.0f, 24.0f);
        atLeft = true;
        selectorImage.enabled = true;
    }

    public void showItem(string text, int number)
    {

        GameObject newTextObject = new GameObject(text.Replace(" ", "-"), typeof(RectTransform));
        var newText = newTextObject.AddComponent<Text>();

        newText.text = text;
        newText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        newText.color = new Color(32.0f/255.0f, 32.0f/255.0f, 32.0f/255.0f);
        newText.alignment = TextAnchor.UpperLeft;
        newText.fontSize = 14;
        newText.resizeTextForBestFit = true;
        newText.resizeTextMinSize = 1;
        newText.resizeTextMaxSize = 300;

        newTextObject.transform.SetParent(transform);
        newTextObject.GetComponent<RectTransform>().sizeDelta = new Vector2(60.0f, 12.0f);
        if (number % 2 == 0)
        {
            float yVal = 24 - 8 * number;
            newTextObject.transform.localPosition = new Vector3(-40.0f, yVal, 0.0f);
        }
        else
        {
            float yVal = 24 - 8 * (number - 1);
            newTextObject.transform.localPosition = new Vector3(40.0f, yVal, 0.0f);
        }
        newTextObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        textObjectList.Add(newTextObject);
    }

    public void showItemWithImage(string text, Sprite sprite, int number)
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
        newTextObject.GetComponent<RectTransform>().sizeDelta = new Vector2(60.0f, 12.0f);

        GameObject newImageObject = new GameObject(text.Replace(" ", "-"), typeof(RectTransform));
        var newImage = newImageObject.AddComponent<Image>();

        newImage.sprite = sprite;
        newImageObject.transform.SetParent(transform);
        if (number % 2 == 0)
        {
            float yVal = 24 - 8 * number;
            newTextObject.transform.localPosition = new Vector3(-40.0f, yVal, 0.0f);
            newImageObject.transform.localPosition = new Vector3(-0.0f, yVal, 0.0f);
        }
        else
        {
            float yVal = 24 - 8 * (number - 1);
            newTextObject.transform.localPosition = new Vector3(40.0f, yVal, 0.0f);
            newImageObject.transform.localPosition = new Vector3(72.0f, yVal, 0.0f);
        }
        newTextObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        newImageObject.transform.localScale = new Vector3(0.25f, 0.25f, 1.0f);

        textObjectList.Add(newTextObject);
        textObjectList.Add(newImageObject);
    }

    public void showItemWithDescriptionAndImage(string text, string description, Sprite sprite)
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
        newTextObject.GetComponent<RectTransform>().sizeDelta = new Vector2(60.0f, 12.0f);
        newTextObject.transform.localPosition = new Vector3(-40.0f, -32.0f, 0.0f);
        newTextObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        GameObject newDescriptionObject = new GameObject(text.Replace(" ", "-"), typeof(RectTransform));
        var newDescription = newDescriptionObject.AddComponent<Text>();

        newDescription.text = description;
        newDescription.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        newDescription.color = new Color(32.0f / 255.0f, 32.0f / 255.0f, 32.0f / 255.0f);
        newDescription.alignment = TextAnchor.UpperLeft;
        newDescription.fontSize = 14;
        newDescription.resizeTextForBestFit = true;
        newDescription.resizeTextMinSize = 1;
        newDescription.resizeTextMaxSize = 300;

        newDescriptionObject.transform.SetParent(transform);
        newDescriptionObject.GetComponent<RectTransform>().sizeDelta = new Vector2(60.0f, 12.0f);
        newDescriptionObject.transform.localPosition = new Vector3(40.0f, -32.0f, 0.0f);
        newDescriptionObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        GameObject newImageObject = new GameObject(text.Replace(" ", "-"), typeof(RectTransform));
        var newImage = newImageObject.AddComponent<Image>();

        newImage.sprite = sprite;
        newImageObject.transform.SetParent(transform);
        newImageObject.transform.localPosition = new Vector3(-52.0f, 0.0f, 0.0f);
        newImageObject.transform.localScale = new Vector3(0.5f, 0.5f, 1.0f);


        textObjectList.Add(newTextObject);
        textObjectList.Add(newDescriptionObject);
        textObjectList.Add(newImageObject);
    }

    public void showAttackDescriptions(List<Attack> attacks)
    {

        GameObject newTextObject = new GameObject("Name".Replace(" ", "-"), typeof(RectTransform));
        var newText = newTextObject.AddComponent<Text>();

        newText.text = "Name";
        newText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        newText.color = new Color(32.0f / 255.0f, 32.0f / 255.0f, 32.0f / 255.0f);
        newText.alignment = TextAnchor.UpperLeft;
        newText.fontSize = 6;
        newText.resizeTextForBestFit = true;
        newText.resizeTextMinSize = 1;
        newText.resizeTextMaxSize = 6;

        newTextObject.transform.SetParent(transform);
        newTextObject.GetComponent<RectTransform>().sizeDelta = new Vector2(60.0f, 12.0f);
        newTextObject.transform.localPosition = new Vector3(40.0f, 30.0f, 0.0f);
        newTextObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        GameObject newPowerTextObject = new GameObject("Power".Replace(" ", "-"), typeof(RectTransform));
        var newPowerText = newPowerTextObject.AddComponent<Text>();

        newPowerText.text = "Power";
        newPowerText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        newPowerText.color = new Color(32.0f / 255.0f, 32.0f / 255.0f, 32.0f / 255.0f);
        newPowerText.alignment = TextAnchor.UpperLeft;
        newPowerText.fontSize = 6;
        newPowerText.resizeTextForBestFit = true;
        newPowerText.resizeTextMinSize = 1;
        newPowerText.resizeTextMaxSize = 6;

        newPowerTextObject.transform.SetParent(transform);
        newPowerTextObject.GetComponent<RectTransform>().sizeDelta = new Vector2(60.0f, 12.0f);
        newPowerTextObject.transform.localPosition = new Vector3(70.0f, 30.0f, 0.0f);
        newPowerTextObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        GameObject newAccuracyTextObject = new GameObject("Accuracy".Replace(" ", "-"), typeof(RectTransform));
        var newAccuracyText = newAccuracyTextObject.AddComponent<Text>();

        newAccuracyText.text = "Accuracy";
        newAccuracyText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        newAccuracyText.color = new Color(32.0f / 255.0f, 32.0f / 255.0f, 32.0f / 255.0f);
        newAccuracyText.alignment = TextAnchor.UpperLeft;
        newAccuracyText.fontSize = 6;
        newAccuracyText.resizeTextForBestFit = true;
        newAccuracyText.resizeTextMinSize = 1;
        newAccuracyText.resizeTextMaxSize = 6;

        newAccuracyTextObject.transform.SetParent(transform);
        newAccuracyTextObject.GetComponent<RectTransform>().sizeDelta = new Vector2(60.0f, 12.0f);
        newAccuracyTextObject.transform.localPosition = new Vector3(90.0f, 30.0f, 0.0f);
        newAccuracyTextObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);


        textObjectList.Add(newTextObject);
        textObjectList.Add(newPowerTextObject);
        textObjectList.Add(newAccuracyTextObject);

        float yVal = 18.0f;

        for (int i = 0; i < attacks.Count; i++)
        {
            newTextObject = new GameObject(attacks[i].name.Replace(" ", "-"), typeof(RectTransform));
            newText = newTextObject.AddComponent<Text>();

            newText.text = attacks[i].name;
            newText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            newText.color = new Color(32.0f / 255.0f, 32.0f / 255.0f, 32.0f / 255.0f);
            newText.alignment = TextAnchor.UpperLeft;
            newText.fontSize = 6;
            newText.resizeTextForBestFit = true;
            newText.resizeTextMinSize = 1;
            newText.resizeTextMaxSize = 6;

            newTextObject.transform.SetParent(transform);
            newTextObject.GetComponent<RectTransform>().sizeDelta = new Vector2(60.0f, 12.0f);
            newTextObject.transform.localPosition = new Vector3(40.0f, yVal, 0.0f);
            newTextObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

            newPowerTextObject = new GameObject(attacks[i].name.Replace(" ", "-"), typeof(RectTransform));
            newPowerText = newPowerTextObject.AddComponent<Text>();

            string temp = "";
            temp += attacks[i].strength;
            newPowerText.text = temp;
            newPowerText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            newPowerText.color = new Color(32.0f / 255.0f, 32.0f / 255.0f, 32.0f / 255.0f);
            newPowerText.alignment = TextAnchor.UpperLeft;
            newPowerText.fontSize = 6;
            newPowerText.resizeTextForBestFit = true;
            newPowerText.resizeTextMinSize = 1;
            newPowerText.resizeTextMaxSize = 6;

            newPowerTextObject.transform.SetParent(transform);
            newPowerTextObject.GetComponent<RectTransform>().sizeDelta = new Vector2(60.0f, 12.0f);
            newPowerTextObject.transform.localPosition = new Vector3(70.0f, yVal, 0.0f);
            newPowerTextObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

            newAccuracyTextObject = new GameObject(attacks[i].name.Replace(" ", "-"), typeof(RectTransform));
            newAccuracyText = newAccuracyTextObject.AddComponent<Text>();

            string temp2 = "";
            int acc = (int)(attacks[i].accuracy * 100);
            temp2 += acc;
            temp2 += "%";
            newAccuracyText.text = temp2;
            newAccuracyText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            newAccuracyText.color = new Color(32.0f / 255.0f, 32.0f / 255.0f, 32.0f / 255.0f);
            newAccuracyText.alignment = TextAnchor.UpperLeft;
            newAccuracyText.fontSize = 6;
            newAccuracyText.resizeTextForBestFit = true;
            newAccuracyText.resizeTextMinSize = 1;
            newAccuracyText.resizeTextMaxSize = 6;

            newAccuracyTextObject.transform.SetParent(transform);
            newAccuracyTextObject.GetComponent<RectTransform>().sizeDelta = new Vector2(60.0f, 12.0f);
            newAccuracyTextObject.transform.localPosition = new Vector3(90.0f, yVal, 0.0f);
            newAccuracyTextObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);


            textObjectList.Add(newTextObject);
            textObjectList.Add(newPowerTextObject);
            textObjectList.Add(newAccuracyTextObject);

            yVal -= 12.0f;
        }
    }

    public void showObjectiveWithDescription(string text, string description)
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
        newTextObject.GetComponent<RectTransform>().sizeDelta = new Vector2(60.0f, 12.0f);
        newTextObject.transform.localPosition = new Vector3(-40.0f, -16.0f, 0.0f);
        newTextObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        GameObject newDescriptionObject = new GameObject(text.Replace(" ", "-"), typeof(RectTransform));
        var newDescription = newDescriptionObject.AddComponent<Text>();

        newDescription.text = description;
        newDescription.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        newDescription.color = new Color(32.0f / 255.0f, 32.0f / 255.0f, 32.0f / 255.0f);
        newDescription.alignment = TextAnchor.UpperLeft;
        newDescription.fontSize = 14;
        newDescription.resizeTextForBestFit = true;
        newDescription.resizeTextMinSize = 1;
        newDescription.resizeTextMaxSize = 300;

        newDescriptionObject.transform.SetParent(transform);
        newDescriptionObject.GetComponent<RectTransform>().sizeDelta = new Vector2(60.0f, 12.0f);
        newDescriptionObject.transform.localPosition = new Vector3(40.0f, -16.0f, 0.0f);
        newDescriptionObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        textObjectList.Add(newTextObject);
        textObjectList.Add(newDescriptionObject);
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

    public void getSelectedItem()
    {
        float xVal = selectorImage.rectTransform.localPosition.x;
        float yVal = selectorImage.rectTransform.localPosition.y;
        Item[] items = playerInventory.backpack.getAllItems();
        if (xVal == -80 && yVal == 24)
        {
            selectedItem = items[0];
        }
        if (xVal == 0 && yVal == 24)
        {
            selectedItem = items[1];
        }
        if (xVal == -80 && yVal == 8)
        {
            selectedItem = items[2];
        }
        if (xVal == 0 && yVal == 8)
        {
            selectedItem = items[3];
        }
        if (xVal == -80 && yVal == -8)
        {
            selectedItem = items[4];
        }
        if (xVal == 0 && yVal == -8)
        {
            selectedItem = items[5];
        }
        if (xVal == -80 && yVal == -24)
        {
            selectedItem = items[6];
        }
        if (xVal == 0 && yVal == -24)
        {
            selectedItem = items[7];
        }
    }

    public void getSelectedObjective()
    {
        float xVal = selectorImage.rectTransform.localPosition.x;
        float yVal = selectorImage.rectTransform.localPosition.y;
        Objective[] objectives = playerObjectives.phone.getAllObjectives();
        if (xVal == -80 && yVal == 24)
        {
            selectedObjective = objectives[0];
        }
        if (xVal == 0 && yVal == 24)
        {
            selectedObjective = objectives[1];
        }
    }

}
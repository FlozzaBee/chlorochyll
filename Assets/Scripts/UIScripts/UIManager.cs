using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//!!Work in progress!!
//Builds and populates the seed selection menu
public class UIManager : MonoBehaviour
{
    public string[] names;
    public GameObject[] seeds;
    public GameObject containerPrefab;
    public int collectedSeeds;

    //Internal Variables
    private GameObject[] menuItem;
    private Camera mainCamera;
    Vector3 offsetVector;

    //debug
    [Header("Testing")]
    public bool buildMenu = false;

    private void Update()
    {
        if(buildMenu)
        {
            buildMenu = false;
            if (menuItem != null)
            {
                foreach (GameObject obj in menuItem)
                {
                    Destroy(obj);
                }
            }
            InstantiateMenu();
        }
    }
    private void InstantiateMenu()
    {
        menuItem = new GameObject[collectedSeeds];
        int toBeInstantiated = collectedSeeds;

        int rows = Mathf.FloorToInt(collectedSeeds / 3); //calculates how many rows of 3 collected seeds we have
        Debug.Log("rows " + rows);

        int remainder = collectedSeeds - rows * 3; //calculates seeds how many are left over
        Debug.Log("remainder " + remainder);

        int rowIndex = 0;

        mainCamera = Camera.main;
        //calculate horizontal distance between menu items, so we can use that for the vertical offset between rows and get a nice grid of items
        float yOffset = Vector3.Magnitude(mainCamera.ViewportToWorldPoint(new Vector3(0.33333f, 0, 3)) - mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 3))); //uses a magnatude so might be worth moving to start/awake to front load intensive calculations
        //convert that yoffset to a vector for easier vector maths
        offsetVector = new Vector3(0, yOffset, 0);
        Debug.Log(offsetVector);

        for (int i = 0; i < rows; i++) //for each row, instantiate a row of 3 menu items
        {
            Instantiate3(rowIndex);
            
            rowIndex++;
        }

        if(remainder == 2)
        {
            Instantiate2(rowIndex);
            
        }
        if(remainder == 1)
        {
            Instantiate1(rowIndex);
        }


    }

    //instantiates a row of 3 equally spaced menu items
    private void Instantiate3(int rowIndex)  //instantiates row with 3 menu items and assigns them to the prefabs array
    {
        for (int i = 0; i < 3; i++)
        {
            menuItem[(rowIndex * 3) + i] = Instantiate(containerPrefab, transform);
        }
        //move each of these items so they form a row of 3 menu items
        menuItem[(rowIndex * 3)].transform.position = mainCamera.ViewportToWorldPoint(new Vector3(0.16666f, 1, 3f));
        menuItem[(rowIndex * 3) + 1].transform.position = mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 1, 3f));
        menuItem[(rowIndex * 3) + 2].transform.position = mainCamera.ViewportToWorldPoint(new Vector3(0.83333f, 1, 3f));
        //for each of the 3 instantiated menu items per row, move them down by the y offset * row number, exept the first row, which is moved by half the offset as to line up with the top of the screen
        for (int ii = 0; ii < 3; ii++)
        {
            menuItem[(rowIndex * 3) + ii].transform.localPosition -= offsetVector * (rowIndex + 1);
        }
        //Debug.Log("Three menu items built");
    }

    private void Instantiate2(int rowIndex)  //instantiate row with 2 menu items
    {
        for (int i = 0; i < 2; i++)
        {
            menuItem[(rowIndex * 3) + i] = Instantiate(containerPrefab, transform);
        }
        //center the 2 menu items
        menuItem[(rowIndex * 3)].transform.position = mainCamera.ViewportToWorldPoint(new Vector3(0.33333f, 1f, 3f));
        menuItem[(rowIndex * 3) + 1].transform.position = mainCamera.ViewportToWorldPoint(new Vector3(0.66666f, 1f, 3f));
        //apply the vertical offset to the 2 menu items
        for (int i = 0; i < 2; i++)
        {
            menuItem[(rowIndex * 3) + i].transform.localPosition -= offsetVector * (rowIndex + 1);
        }
        Debug.Log("two menu items built & positioned");
    }

    private void Instantiate1(int rowIndex)  //instantiates row with 1 menu item
    {
        menuItem[(rowIndex * 3)] = Instantiate(containerPrefab, transform);
        menuItem[(rowIndex * 3)].transform.position = mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 1, 3f));
        menuItem[(rowIndex * 3)].transform.localPosition -= offsetVector * (rowIndex + 1);
        Debug.Log("one menu items built");
    }
}

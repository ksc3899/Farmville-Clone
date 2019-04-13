using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[System.Serializable]
public class SavedProfile
{
    public float savedWood;
    public float savedStones;
    public float savedFood;
    public List<BuildingInformation> buildingsSavedData = new List<BuildingInformation>();
}

public class Save : MonoBehaviour
{
    public SavedProfile savedProfile;

    private Buildings buildings;
    private Resources resources;
    private Build build;

    private void Awake()
    {
        build = FindObjectOfType<Build>();
        resources = FindObjectOfType<Resources>();
        buildings = FindObjectOfType<Buildings>();

        LoadGame();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            SaveGame();
            Debug.Log("Game Saved!");
        }
    }

    private void SaveGame()
    {
        if(savedProfile == null)
        {
            savedProfile = new SavedProfile();
        }

        savedProfile.savedWood = resources.wood;
        savedProfile.savedStones = resources.stones;
        savedProfile.savedFood = resources.food;
        
        foreach(GameObject g in buildings.builtObjects)
        {
            savedProfile.buildingsSavedData.Add(g.GetComponent<Building>().information);
        }

        BinaryFormatter bF = new BinaryFormatter();
        string path = Application.persistentDataPath + "/save.dat";

        if(File.Exists(path))
        {
            File.Delete(path);
        }

        FileStream fs = File.Open(path, FileMode.OpenOrCreate);
        bF.Serialize(fs, savedProfile);
        fs.Close();
    }

    private void LoadGame()
    {
        string pathToLoad = Application.persistentDataPath + "/save.dat";
        if(!File.Exists(pathToLoad))
        {
            Debug.Log("No saved profile found! Have you saved yet?");
            return;
        }

        BinaryFormatter bF = new BinaryFormatter();
        FileStream fS = File.Open(pathToLoad, FileMode.Open);
        SavedProfile loadedProfile = bF.Deserialize(fS) as SavedProfile;
        fS.Close();

        resources.wood = loadedProfile.savedWood;
        resources.stones = loadedProfile.savedStones;
        resources.food = loadedProfile.savedFood;

        //RELOAD BUILDINGS AND REBUILD THEM.
    }
}

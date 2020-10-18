using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameSaveManager : MonoBehaviour{

    public static GameSaveManager instance;

    void Awake() {
        if(instance == null)
            instance = this;
        else if(instance != this)
            Destroy(this.gameObject);
        DontDestroyOnLoad(this);
    }

    public bool IsSaveFile(){
        return Directory.Exists(Application.persistentDataPath + "game_save");
    }

    public void SaveGame(){
        if(!IsSaveFile())
            Directory.CreateDirectory(Application.persistentDataPath + "/game_save");
        if(!Directory.Exists(Application.persistentDataPath + "/game_save/user_data"))
            Directory.CreateDirectory(Application.persistentDataPath + "/game_save/user_data");
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/game_save/user_data/user_save.txt");
        var json = JsonUtility.ToJson(UserDataManager.instance.playerData);
        //**************************************************************************************
        DatabaseManager.writeNewPlayerData(UserDataManager.instance.playerData.userDataPieces[0].userName, UserDataManager.instance.playerData.userDataPieces);
        //**************************************************************************************
        bf.Serialize(file,json);
        file.Close();
    }

    public void LoadGame(){
        if(!Directory.Exists(Application.persistentDataPath + "/game_save/user_data"))
            Directory.CreateDirectory(Application.persistentDataPath + "/game_save/user_data");
        BinaryFormatter bf = new BinaryFormatter();
        if(File.Exists(Application.persistentDataPath + "/game_save/user_data/user_save.txt")){
            FileStream file = File.Open(Application.persistentDataPath + "/game_save/user_data/user_save.txt", FileMode.Open);
            JsonUtility.FromJsonOverwrite((string) bf.Deserialize(file), UserDataManager.instance.playerData);
            file.Close();
        }
    }
}

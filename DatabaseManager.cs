using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class DatabaseManager : MonoBehaviour{
    DatabaseReference reference;

    void Start(){
      // Set this before calling into the realtime database.
      FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("**********URL PROYECTO**************");
      // Get the root reference location of the database.
      reference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    void Update(){

    }

    private void writeNewPlayerData(string playerID, List<UserDataPiece> userDataPieces) {
        PlayerData_SO playerData = new PlayerData_SO(userDataPieces);
        playerData.dashPlayerData();
        string json = JsonUtility.ToJson(playerData);

        reference.Child("Players").Child(playerID).SetRawJsonValueAsync(json);
    }

}

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
      FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://inklu-database.firebaseio.com/");
      // Get the root reference location of the database.
      reference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    void Update(){

    }

    //Esta función se tiene que llamar cuando se quiera subir la info del jugador
    private void writeNewPlayerData(string playerID, List<UserDataPiece> userDataPieces) {
        PlayerData_SO playerData = new PlayerData_SO(userDataPieces);
        playerData.dashPlayerData();
        string json = JsonUtility.ToJson(playerData);

        reference.Child("Players").Child(playerID).SetRawJsonValueAsync(json);
    }

}

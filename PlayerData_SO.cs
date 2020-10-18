using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData")]
public class PlayerData_SO : ScriptableObject {

    public List<UserDataPiece> userDataPieces;
    public float RTV;
    public float RT;
    public float PCRT;
    public int EO;
    public float PPC;

    public void AddDataPiece(float time, string currentLevel, string buttonPressed, bool correct, int inputsCount) {
        UserDataPiece dataPiece = new UserDataPiece();
        dataPiece.inputTime = time;
        dataPiece.level = currentLevel;
        dataPiece.buttonPressed = buttonPressed;
        dataPiece.date = System.DateTime.Now.ToString();
        dataPiece.correct = (correct ? 1 : 0);
        dataPiece.n = inputsCount;
        dataPiece.userName = GameManager.instance.userName;
        dataPiece.CompleteInfo();
        dataPiece.SetEpisode(GameManager.instance.episode);
        userDataPieces.Add(dataPiece);
        GameSaveManager.instance.SaveGame();
        CSVManager.AppendToReport(ConvertDataPieceToStringArray(dataPiece));
    }

    public void dashPlayerData(){
      RTV = 0;
      RT = 0;
      PCRT = 0;
      EO = 0;
      PPC = 0;

      int corrR = 0;
      int incorrR = 0;
      float meanDifIncorrectRTime = 0;
      float meanCorrectRTime = 0;

      //RTV = S(Response Times - Mean Correct Resp Time)^2 / #Correct Responses
      for(int i=0; i<userDataPieces.Count; i++){
        meanCorrectRTime = meanCorrectRTime + userDataPieces[i].correctRespTime;
        if(userDataPieces[i].correct == 1)
          corrR++;
        else
          incorrR++;
      }
      meanCorrectRTime = meanCorrectRTime/corrR;

      for(int i=0; i<userDataPieces.Count; i++)
          RTV = RTV + pow(userDataPieces[i].correctRespTime - meanCorrectRTime, 2)
      RTV = RTV / corrR;

      //RT = S(Correct RT)/#Targets
      for(int i=0; i<userDataPieces.Count; i++)
          RT = RT + userDataPieces[i].correctRespTime;
      RT = RT / corrR;

      //Error of O = #Ommissions / #Targets - #Target Anticipatory Resp x100
      for(int i=0; i<userDataPieces.Count; i++)
        EO = EO + userDataPieces[i].errorOfOmi;

      //PCRT = S(PCRT)/#PCR
      for(int i=0; i<userDataPieces.Count; i++)
        PCRT = PCRT + userDataPieces[i].incorrectRespTime;
      PCRT = PCRT/incorrR;

      //PPC = S(Response Times - Mean Incorrect Resp Time)^2 / #Incorrect Responses
      for(int i=1; i<userDataPieces.Count; i++){
        if(userDataPieces[i-1].correct == 0 && userDataPieces[i].correct == 0)
          userDataPieces[i].difIncorrectRespTime = userDataPieces[i].incorrectRespTime - userDataPieces[i-1].incorrectRespTime;
      }
      for(int i=0; i<userDataPieces.Count; i++)
        meanDifIncorrectRTime = meanDifIncorrectRTime + userDataPieces[i].difIncorrectRespTime;
      meanDifIncorrectRTime = meanDifIncorrectRTime/incorrR;
      for(int i=0; i<userDataPieces.Count; i++)
          PPC = PPC + pow(userDataPieces[i].difIncorrectRespTime - meanDifIncorrectRTime, 2)
      PPC = PPC / incorrR;

    }

    public string[] ConvertDataPieceToStringArray(UserDataPiece dataPiece){
        string[] auxArray = new string[8];
        auxArray[0] = dataPiece.userName;
        auxArray[1] = dataPiece.inputTime.ToString();
        auxArray[2] = dataPiece.correct.ToString();
        auxArray[3] = dataPiece.level;
        auxArray[4] = dataPiece.n.ToString();
        auxArray[5] = dataPiece.buttonPressed;
        auxArray[6] = dataPiece.episode;
        auxArray[7] = dataPiece.date;
        return auxArray;
    }
}

[System.Serializable]
public struct UserDataPiece{

    public string date;
    public string userName;//usuario
    public float inputTime;//tiempo de interacción
    public string level;//mini juego
    public string buttonPressed;//color presionado
    public string episode;//dificultad
    public int correct; //informacion sobre si la pulsacion fue o no correcta
    public int n; //numero de interaccion

    public float correctRespTime;
    public float incorrectRespTime;
    public float difIncorrectRespTime;
    public int errorOfOmi;

    public void CompleteInfo(){
      if(correct == 1){
        errorOfOmi = 0;
        correctRespTime = inputTime;
        incorrectRespTime = 0;
      }else if(correct == 0){
        errorOfOmi = 1;
        incorrectRespTime = inputTime;
        correctRespTime = 0;
      }
    }

    public void SetEpisode(int value){
        switch(value){
            case 0:
                episode = "facil";
            break;
            case 1:
                episode = "medio";
            break;
            case 2:
                episode = "dificil";
            break;
        }
    }
}

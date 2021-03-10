using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


public class SaveData<T> 
    where T : class
{
    static string dataPath;
    

    public static void DataSave(T data, string path)
    {
        dataPath = Application.persistentDataPath + path;

        Debug.Log(dataPath);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(dataPath);


        bf.Serialize(file, data);
        file.Close();
    }


    public static T DataLoad(string path)
    {
        dataPath = Application.persistentDataPath + path;

        if (File.Exists(dataPath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(dataPath, FileMode.Open);
            T data = (T)bf.Deserialize(file);
            file.Close();
            return data;
        }

        Debug.Log("Load Fail");
        return null;
    }
}

using UnityEngine;
using System;
using System.IO;

public static class FileDataHandler
{

    public static GameData Load(string dir)
    {
        // use Path.Combine to account for different OS's having different path separators
        //string fullPath = Path.Combine(dataDirPath, dataFileName + teamID + dataExtention);
        string fullPath = Path.Combine(dir);
        GameData loadedData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                // load the serialized data from the file
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                // deserialize the data from Json back into the C# object
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("Error occured when trying to load data from file: " + fullPath + "\n" + e);
            }
        }
        return loadedData;
    }

    public static void Save(GameData data, string fullPath)
    {
        // use Path.Combine to account for different OS's having different path separators
        //string fullPath = Path.Combine(dataDirPath, dataFileName + teamID + dataExtention);
        try
        {
            // create the directory the file will be written to if it doesn't already exist
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            // serialize the C# game data object into Json
            string dataToStore = JsonUtility.ToJson(data, true);

            // write the serialized data to the file
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                    Debug.Log("Se a guardado exitosamente la data en: " + fullPath);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error occured when trying to save data to file: " + fullPath + "\n" + e);
        }
    }


}

using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

public static class Serialization
{
    public static string SaveLocation(string worldName)
    {
        string saveLocation = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Unity-Voxel\" + worldName + @"\chunks\";
        if (!Directory.Exists(saveLocation))
            Directory.CreateDirectory(saveLocation);

        return saveLocation;
    }

    public static void SaveChunk(Chunk chunk)
    {
        Save save = new Save(chunk);
        if (save.Blocks.Count == 0)
            return;

        string saveFile = GetChunkFileName(chunk);

        IFormatter formatter = new BinaryFormatter();
        using (Stream stream = new FileStream(saveFile, FileMode.Create, FileAccess.Write, FileShare.None))
        {
            formatter.Serialize(stream, save);
            stream.Close();
        }
    }

    public static bool Load(Chunk chunk)
    {
        string saveFile = GetChunkFileName(chunk);

        if (!File.Exists(saveFile))
            return false;

        IFormatter formatter = new BinaryFormatter();
        using (FileStream stream = new FileStream(saveFile, FileMode.Open))
        {

            Save save = (Save)formatter.Deserialize(stream);

            foreach (var block in save.Blocks)
            {
                chunk.Blocks[block.Key.x, block.Key.y, block.Key.z] = block.Value;
            }
            stream.Close();
        }

        foreach (Block b in chunk.Blocks)
            b.Chunk = chunk;
        return true;
    }

    private static string GetChunkFileName(Chunk chunk)
    {
        string saveFile = SaveLocation(chunk.World.worldName);
        saveFile += chunk.FileName();
        
        return saveFile;
    }
}

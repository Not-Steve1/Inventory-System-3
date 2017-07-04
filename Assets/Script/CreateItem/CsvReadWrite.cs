using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class CsvReadWrite : MonoBehaviour {

    public static string[][] LoadTextFile(TextAsset textFile)
    {
        if (textFile != null)
        {
            string[] row = textFile.text.Split('\n');
            string[][] grid = new string[row.Length][];
            for (int i = 0; i < row.Length; i++)
            {
                grid[i] = row[i].Split(',');
            }
            return grid.ToArray();
        }
        else
        {
            Debug.Log("File does not exist.");
            return null;
        }
    }

    public static void WriteToFile(string[][] strList, TextAsset textFile)
    {
        if (textFile != null)
        {
            string[] lines = new string[strList.Length];
            for (int i = 0; i < strList.Length; i++)
            {
                for (int j = 0; j < strList[i].Length   ; j++)
                {
                    lines[i] += strList[i][j];
                }
            }
            File.WriteAllLines(AssetDatabase.GetAssetPath(textFile), lines);
        }
        else
        {
            Debug.Log("File does not exist.");
        }

    }

}

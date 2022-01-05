using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Trolling : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    public void Run()
    {
        var dir = new DirectoryInfo(@"C:\Temp");
        var file = new FileInfo(Path.Combine(dir.FullName, "tempFrogs.txt"));
        if (!file.Exists)
        {
            using (Stream stream = file.OpenWrite())
            using (StreamWriter writer = new StreamWriter(stream))
            {
                writer.Write("tebya zatrollili");
            }

            audioSource.Play();
        }
    }
}

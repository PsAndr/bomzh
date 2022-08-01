using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace Engine
{
    public class DebugEngine
    {
        private static readonly string path = Application.persistentDataPath + "/debugLogs.txt";

        public static void Log(string message)
        {
            if (!File.Exists(path))
            {
                File.Create(path);
            }
            Debug.Log(message);
            string text = message + '\n' + File.ReadAllText(path);
            File.WriteAllText(path, text);
        }

        public static void LogWarning(string message)
        {
            if (!File.Exists(path))
            {
                File.Create(path);
            }
            Debug.LogWarning(message);
            string text = "WARNING!!!: " + message + '\n' + File.ReadAllText(path);
            File.WriteAllText(path, text);
        }

        public static void LogException(System.Exception message)
        {
            if (!File.Exists(path))
            {
                File.Create(path);
            }
            Debug.LogException(message);
            string text = "EXCEPTION!!!: " + message.Message + '\n' + File.ReadAllText(path);
            File.WriteAllText(path, text);
        }

        public static void Clear()
        {
            if (File.Exists(path))
            {
                File.WriteAllText(path, "");
            }
            else
            {
                File.Create(path);
            }
        }
    }
}

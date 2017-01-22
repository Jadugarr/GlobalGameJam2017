using UnityEditor;
using UnityEngine;

public class DeletePlayerPrefItem
{
    [MenuItem("Tools/Clear PlayerPrefs")]
    private static void NewMenuOption()
    {
        PlayerPrefs.DeleteAll();
    }
}

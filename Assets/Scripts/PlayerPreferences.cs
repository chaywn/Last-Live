using UnityEngine;

public class PlayerPreferences : MonoBehaviour
{
    public void SavePrefs(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
        PlayerPrefs.Save();
    }

    public int LoadPrefs(string key)
    {
        return PlayerPrefs.GetInt(key, 0);
    }
}

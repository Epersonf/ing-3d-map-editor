using UnityEngine;

public class SceneSingleton<T> where T : MonoBehaviour
{
  private static T _instance;
  public static T Instance
  {
    get
    {
      if (_instance == null)
      {
        _instance = Object.FindFirstObjectByType<T>();
        return _instance;
      }
      return _instance;
    }
  }
}

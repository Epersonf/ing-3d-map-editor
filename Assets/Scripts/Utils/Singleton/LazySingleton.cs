using UnityEngine;

public abstract class LazySingleton<T> : MonoBehaviour where T : MonoBehaviour
{
  private static T _instance;
  public static T Instance
  {
    get
    {
      if (_instance != null) return _instance;

      // tenta achar na cena
      _instance = FindFirstObjectByType<T>();
      if (_instance != null) return _instance;

      // tenta carregar de Resources (usa o nome da classe como padrão)
      var resource = Resources.Load<T>(typeof(T).Name);
      if (resource != null)
      {
        _instance = Instantiate(resource);
        DontDestroyOnLoad(_instance);
        return _instance;
      }

      Debug.LogError($"Singleton<{typeof(T).Name}>: Não encontrado em cena nem em Resources.");
      return null;
    }
  }
}

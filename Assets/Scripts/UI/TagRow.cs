using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TagRow : MonoBehaviour
{
  [SerializeField] TMP_Text keyLabel;
  [SerializeField] TMP_Text valueLabel;
  [SerializeField] Button deleteButton;

  string key;
  TransformableObject target;

  public void Init(TransformableObject t, string key, string value)
  {
    this.target = t;
    this.key = key;

    keyLabel.text = key;
    valueLabel.text = value;

    deleteButton.onClick.AddListener(Delete);
  }

  void Delete()
  {
    if (target != null)
      target.RemoveTag(key);

    Destroy(gameObject);
  }
}

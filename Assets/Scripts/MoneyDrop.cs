using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyDrop : MonoBehaviour
{
    public SFXManager sfxManager;
    public GameObject moneyPrefab;
    public RectTransform imageTransform;
    private List<GameObject> moneyList = new List<GameObject>();
    private bool isFollowingImage = false;
    public float moveSpeed = 10f;
    
    void Start()
    {
        sfxManager = FindObjectOfType<SFXManager>();
    }

    void Update()
    {
        if (isFollowingImage && moneyList.Count > 0)
        {
            Vector3 imageWorldPosition = Camera.main.ScreenToWorldPoint(imageTransform.position);
            imageWorldPosition.z = 0;

            for (int i = moneyList.Count - 1; i >= 0; i--)
            {
                GameObject money = moneyList[i];
                if (money != null)
                {
                    Vector3 targetPosition = Vector3.MoveTowards(money.transform.position, imageWorldPosition, moveSpeed * Time.deltaTime);
                    Vector3 clampedPosition = ClampToCameraBounds(targetPosition);
                    money.transform.position = clampedPosition;

                    if (Vector3.Distance(clampedPosition, imageWorldPosition) <= 0.5f)
                    {
                        Destroy(money);
                        moneyList.RemoveAt(i);
                        sfxManager.PlayEnvironmentSFX(0);
                    }
                }
            }

            if (moneyList.Count == 0)
            {
                isFollowingImage = false;
            }
        }
    }

    private Vector3 ClampToCameraBounds(Vector3 position)
    {
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(position);
        viewportPosition.x = Mathf.Clamp(viewportPosition.x, 0.05f, 0.95f);
        viewportPosition.y = Mathf.Clamp(viewportPosition.y, 0.05f, 0.95f);
        return Camera.main.ViewportToWorldPoint(viewportPosition);
    }

    public void DropMoney(Vector3 dropPosition)
    {
        GameObject money = Instantiate(moneyPrefab, dropPosition, Quaternion.identity);
        moneyList.Add(money);
        isFollowingImage = true;
    }
}

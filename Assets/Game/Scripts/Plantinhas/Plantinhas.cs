using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plantinhas : MonoBehaviour
{
    [System.Serializable]
    public class Plant
    {
        public GameObject plantObject; 
        public Sprite healthySprite;  
        public Sprite wiltedSprite;
        [HideInInspector] public bool isWilted = false;
    }
    public Plant[] plants;
    private void Start()
    {
        foreach (var plant in plants)
        {
            SetPlantHealthy(plant);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null)
            {
                foreach (var plant in plants)
                {
                    if (hit.collider.gameObject == plant.plantObject)
                    {
                        TogglePlantState(plant);
                        break;
                    }
                }
            }
        }
    }

    private void SetPlantHealthy(Plant plant)
    {
        plant.isWilted = false;
        plant.plantObject.GetComponent<SpriteRenderer>().sprite = plant.healthySprite;
    }

    private void SetPlantWilted(Plant plant)
    {
        plant.isWilted = true;
        plant.plantObject.GetComponent<SpriteRenderer>().sprite = plant.wiltedSprite;
    }

    private void TogglePlantState(Plant plant)
    {
        if (plant.isWilted)
        {
            SetPlantHealthy(plant);
        }
        else
        {
            SetPlantWilted(plant);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingCreatorManager : MonoBehaviour
{
    public static BuildingCreatorManager instance;
    public Grid grid;
    public GameObject buildingCreatorHolder;
    private SpriteRenderer buildingCreatorSpriteRenderer;

    public Color validPlacementColor = Color.green;
    public Color invalidPlacementColor = Color.red;
    private bool isValidPlacement = true;
    private bool isPlacingBuilding = false;
    private Building actualBuilding;
    private void Awake()
    {
        if (BuildingCreatorManager.instance) Destroy(gameObject);
        else BuildingCreatorManager.instance = this;

        buildingCreatorSpriteRenderer = buildingCreatorHolder.GetComponent<SpriteRenderer>();
        buildingCreatorHolder.SetActive(false);
    }

    #region PlacementSystem
    void Update()
    {
        if (!actualBuilding) return;
        // Actualiza el color de acuerdo a si es una posición de construcción válida
        UpdatePlacementColor();

        // Mover el objeto con el mouse
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        buildingCreatorHolder.transform.position = GetGridPosition(mousePos);

        // Colocar el objeto al presionar el botón izquierdo del ratón
        if (!isPlacingBuilding) return;
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (isValidPlacement)
            {
                ConstructBuilding();
            }
            else
            {
                Debug.Log("No se puede colocar en esta posición.");
            }
        }
    }

    // Obtiene la posición de la grid basada en la posición del mouse
    Vector3 GetGridPosition(Vector3 position)
    {
        Vector3Int cellPosition = grid.WorldToCell(position);
        Vector3 result = grid.CellToWorld(cellPosition);
        result.z = 0f;
        return result;
    }

    // Actualiza el color del objeto según la validez de la posición de la grid
    void UpdatePlacementColor()
    {
        Vector3Int cellPosition = grid.WorldToCell(transform.position);
        isValidPlacement = !IsOverlapping();

        // Cambiar el color según la validez
        buildingCreatorSpriteRenderer.color = isValidPlacement ? BuildingCreatorManager.instance.validPlacementColor : BuildingCreatorManager.instance.invalidPlacementColor;
    }

    // Verifica si hay algún objeto en la posición de la grid
    bool IsOverlapping()
    {
        List<Collider2D> colliders = new List<Collider2D>();
        foreach (var coll in actualBuilding.buildingSize)
        {
            colliders.AddRange(Physics2D.OverlapBoxAll(ColliderPosition(coll.position), new Vector2(coll.sizeX - 0.1f, coll.sizeY - 0.1f), 0));
        }

        foreach (var collider in colliders)
        {
            if (collider.gameObject != gameObject) // Ignora este objeto
            {
                return true;
            }
        }
        return false;
    }

    Vector2 ColliderPosition(Vector2 collPosition)
    {
        Vector2 myPos = buildingCreatorHolder.transform.position;
        return myPos + collPosition;
    }

    // Dibuja un gizmo visual para la posición y tamaño en la grid (solo en el editor)
    void OnDrawGizmosSelected()
    {
        if (!actualBuilding) return;
        Gizmos.color = Color.cyan;

        foreach (var coll in actualBuilding.buildingSize)
        {
            Gizmos.DrawWireCube(ColliderPosition(coll.position), new Vector3(coll.sizeX, coll.sizeY, 1));
        }
    }
    #endregion

    #region BuildingSystem
    public void InitCreationOfBuilding(Building building)
    {
        buildingCreatorHolder.gameObject.SetActive(true);
        buildingCreatorSpriteRenderer.sprite = building.Sprite;
        actualBuilding = building;
        isPlacingBuilding = true;
    }

    public void ConstructBuilding()
    {
        isPlacingBuilding = false;
        buildingCreatorHolder.gameObject.SetActive(false);
        Instantiate(actualBuilding.gameObject, buildingCreatorHolder.transform.position, quaternion.identity);
    }
    #endregion
}
[System.Serializable]
public class BuildingSize
{
    public Vector2 position;
    public float sizeX, sizeY;
}
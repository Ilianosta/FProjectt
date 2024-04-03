using System.Collections.Generic;
using UnityEngine;
public class Building : MonoBehaviour
{
    public BuildingSize[] buildginColls; // Tamaño en la grid (en unidades de grid)
    public Color validPlacementColor = Color.green;
    public Color invalidPlacementColor = Color.red;

    [SerializeField] private Grid grid;
    private SpriteRenderer spriteRenderer;
    private bool isValidPlacement = true;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdatePlacementColor();
    }

    void Update()
    {
        // Actualiza el color de acuerdo a si es una posición de construcción válida
        UpdatePlacementColor();

        // Mover el objeto con el mouse
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        transform.position = GetGridPosition(mousePos);

        // Colocar el objeto al presionar el botón izquierdo del ratón
        if (Input.GetMouseButtonDown(0))
        {
            if (isValidPlacement)
            {
                // Aquí puedes colocar el objeto en la posición de la grid
                // Por ejemplo, puedes instanciar otro objeto o realizar otra acción
                Debug.Log("Objeto colocado en posición válida!");
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
        spriteRenderer.color = isValidPlacement ? validPlacementColor : invalidPlacementColor;
    }

    // Verifica si hay algún objeto en la posición de la grid
    bool IsOverlapping()
    {
        List<Collider2D> colliders = new List<Collider2D>();
        foreach (var coll in buildginColls)
        {
            colliders.AddRange(Physics2D.OverlapBoxAll(ColliderPosition(coll.position), new Vector2(coll.sizeX, coll.sizeY), 0));
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
        Vector2 myPos = transform.position;
        return myPos + collPosition;
    }

    // Dibuja un gizmo visual para la posición y tamaño en la grid (solo en el editor)
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        if (grid)
        {
            foreach (var coll in buildginColls)
            {
                Gizmos.DrawWireCube(ColliderPosition(coll.position), new Vector3(coll.sizeX, coll.sizeY, 1));
            }
        }
    }
}
[System.Serializable]
public class BuildingSize
{
    public Vector2 position;
    public float sizeX, sizeY;
}


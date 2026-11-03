using UnityEngine;

//builds a row of destructible shields between the ship and the fleet
public class Shields : MonoBehaviour
{
    [Header("Inscribed")]
    public Material blockMaterial;
    public int count = 4;
    public float spacing = 8f;
    public float blockSize = 0.45f;

    //each shield's shape, top row first ('X' = block)
    static readonly string[] shape =
    {
        " XXXXXX ",
        "XXXXXXXX",
        "XXXXXXXX",
        "XXX  XXX",
    };

    void Start()
    {
        for (int i = 0; i < count; i++)
        {
            float centreX = (i - (count - 1) / 2f) * spacing;
            BuildShield(new Vector3(centreX, transform.position.y, 0));
        }
    }

    void BuildShield(Vector3 centre)
    {
        GameObject shield = new GameObject("Shield");
        shield.transform.SetParent(transform);
        int rows = shape.Length, cols = shape[0].Length;
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                if (shape[row][col] != 'X')
                {
                    continue;
                }
                GameObject block = GameObject.CreatePrimitive(PrimitiveType.Cube);
                block.name = "ShieldBlock";
                block.transform.SetParent(shield.transform);
                block.transform.localScale = Vector3.one * blockSize;
                block.transform.position = centre + new Vector3((col - (cols - 1) / 2f) * blockSize, (rows - 1 - row) * blockSize, 0);
                block.GetComponent<Collider>().isTrigger = true;
                block.GetComponent<Renderer>().sharedMaterial = blockMaterial;
                block.AddComponent<ShieldBlock>();
            }
        }
    }
}

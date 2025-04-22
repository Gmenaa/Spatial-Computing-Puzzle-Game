using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    private Renderer objRenderer;
    private bool isTouched = false;  // track if object has been touched
    private int xIndex, yIndex;      // track position in the grid

    public AudioSource source;
    public AudioClip beepAudioClip;

    public AudioSource winsource;
    public AudioClip winAudioClip;

    public GameObject pokableButton; // final button to initiate end game sequence

    private static Color[,] gridColors = new Color[3, 3] {
        {Color.red, Color.red, Color.red},
        {Color.red, Color.red, Color.red},
        {Color.red, Color.red, Color.red}
    };

    // initialize each object in grid
    public void Initialize(int x, int y)
    {
        xIndex = x;
        yIndex = y;
        objRenderer = GetComponent<Renderer>();
        objRenderer.material.color = gridColors[x, y]; // set initial color 
    }

    // toggle color for row and column
    private void ToggleRowAndColumnColors()
    {
        // toggle colors in the row of touched object
        for (int x = 0; x < 3; x++)
        {
            gridColors[x, yIndex] = gridColors[x, yIndex] == Color.red ? Color.green : Color.red;
        }

        // toggle colors in the column of touched object
        for (int y = 0; y < 3; y++)
        {
            gridColors[xIndex, y] = gridColors[xIndex, y] == Color.red ? Color.green : Color.red;
        }

        // toggle color of touched object itself
        gridColors[xIndex, yIndex] = gridColors[xIndex, yIndex] == Color.red ? Color.green : Color.red;


        AudioSource.PlayClipAtPoint(beepAudioClip, transform.position);

        // update colors of all game objects based on grid
        UpdateAllObjects();

        // check if all objects are green
        CheckForWin();
    }

    // update all object's colors based on grid
    private void UpdateAllObjects()
    {
        // find all objects
        ColorChanger[] allObjects = FindObjectsOfType<ColorChanger>();
        foreach (ColorChanger obj in allObjects)
        {
            obj.objRenderer.material.color = gridColors[obj.xIndex, obj.yIndex];
        }
    }

    // touch interaction
    void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("LeftHand") || other.name.Contains("Controller"))
        {
            Debug.Log($"Touched object at position: ({xIndex},{yIndex})");

            // toggle colors in the row and column
            ToggleRowAndColumnColors();
        }
    }

    private void CheckForWin()
    {
        for (int y = 0; y < 3; y++)
        {
            for (int x = 0; x < 3; x++)
            {
                if (gridColors[x, y] != Color.green)
                    return; // exit if any are not green
            }
        }

        Debug.LogError("All squares are green!");
        // AudioSource.PlayClipAtPoint(winAudioClip, transform.position);
        GameLoop gameLoop = FindObjectOfType<GameLoop>();
        if (gameLoop != null)
        {
            gameLoop.TriggerWin();
        }
        else
        {
            Debug.LogError("GameLoop not found in scene!");
        }

        if (pokableButton != null)
        {
            pokableButton.SetActive(true); 
            Debug.LogError("poke it babyyyyy");
        }

    }
}

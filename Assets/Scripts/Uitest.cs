using UnityEngine;
using UnityEngine.UIElements;

public class Uitest : MonoBehaviour
{
    public UIDocument uiDocument; // Assign UI Document in Inspector
    public GameObject targetObject; // Assign the Cylinder in Inspector
    public ParticleSystem particleSystem; // Assign the Particle System in Inspector

    private VisualElement root;
    private Button turnOnButton;
    private Button turnOffButton;
    private Renderer targetRenderer;

    void Start()
    {
        if (uiDocument == null || targetObject == null)
        {
            Debug.LogError("UI Document or Target Object is missing!");
            return;
        }



              Vector3 objectPos = targetObject.transform.position;
              Debug.Log(objectPos.x);
              Debug.Log(objectPos.z);
              Debug.Log(uiDocument.rootVisualElement.style.left);
              Debug.Log(uiDocument.rootVisualElement.style.top);
    
  uiDocument.rootVisualElement.style.left = objectPos.x ;
    uiDocument.rootVisualElement.style.top =  -objectPos.z ; // Adjust Y-axis
        // Get the renderer of the Cylinder
        targetRenderer = targetObject.GetComponent<Renderer>();

        // Get the root of the UI Document
        root = uiDocument.rootVisualElement;

        // Get buttons from UI
        turnOnButton = root.Q<Button>("on");
        turnOffButton = root.Q<Button>("off");

        // Assign button actions
        turnOnButton.clicked += TurnOnCylinder;
        turnOffButton.clicked += TurnOffCylinder;

        // Initialize state
        TurnOffCylinder();
    }

    void Update()
    {

    }

    void TurnOnCylinder()
    {
        targetObject.SetActive(true);
        if (particleSystem != null)
            particleSystem.Play(); // Enable Particle System
        targetRenderer.material.color = Color.green; // Change existing material color to green
        Debug.Log("Cylinder Turned ON (Green)");
    }

    void TurnOffCylinder()
    {
        targetObject.SetActive(true);
           if (particleSystem != null)
            particleSystem.Stop(); // Disable Particle System
        targetRenderer.material.color = Color.red; // Change existing material color to red
        Debug.Log("Cylinder Turned OFF (Red)");
    }
}

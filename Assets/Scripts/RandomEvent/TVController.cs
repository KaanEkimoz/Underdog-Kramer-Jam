using UnityEngine;

public class TVController : MonoBehaviour
{
    public Renderer tvRenderer;
    public int materialIndex;
    public Material openMaterial;
    public Material closedMaterial;
    

    public AudioSource tvAudio;

    public float tvOpenDuration = 10f;

    [SerializeField]private float timer;
    private bool isOpen = false;

    private void Start()
    {
        timer = tvOpenDuration;
        
        CloseTV();
        
    }

    private void Update()
    {
        if (isOpen)
        {
            timer -= Time.deltaTime;
            if(timer <= 0f)
            {
                CloseTV();  
            }
        }
    }

    public void CloseTV()
    {
        tvAudio.Stop();
        Material[] tvMaterials = tvRenderer.materials;
        tvMaterials[materialIndex] = closedMaterial;
        tvRenderer.materials = tvMaterials;
        isOpen = false;
        Debug.Log("tv kapali");
    }

    public void OpenTV()
    {
        tvAudio.Play();
        Material[] tvMaterials = tvRenderer.materials;
        tvMaterials[materialIndex] = openMaterial;
        tvRenderer.materials = tvMaterials;
        Debug.Log("tv acik");
        isOpen = true;
    }

    public bool IsOpen()
    {
        return isOpen;
    }
}

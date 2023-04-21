using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateLevel : MonoBehaviour
{
    public GameObject[] section; // an array of section prefabs to generate
    public int zPos = 25; // starting z position for the first section
    public bool creatingSection = false; // flag to indicate if a section is currently being generated
    public int secNum; // the index of the section to generate

    private List<GameObject> spawnedSections = new List<GameObject>(); // a list to store references to spawned sections

    // Start is called before the first frame update
    void Start()
    {
        // Generate the first three sections
        for (int i = 0; i < 3; i++)
        {
            GenerateNewSection();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check if a section is currently being generated
        if (creatingSection == false)
        {
            creatingSection = true;
            StartCoroutine(GenerateSection());
        }
    }

    void GenerateNewSection()
    {
        // Choose a random section prefab from the array
        secNum = Random.Range(0, 7);

        // Instantiate the chosen prefab at the current z position
        GameObject newSection = Instantiate(section[secNum], new Vector3(0, 0, zPos), Quaternion.identity);

        // Add a reference to the spawned section to the list
        spawnedSections.Add(newSection);

        // Increase the z position for the next section
        zPos += 25;
    }

    IEnumerator GenerateSection()
    {
        // Generate a new section
        GenerateNewSection();

        // Wait for a second before destroying clones
        yield return new WaitForSeconds(1);
        creatingSection = false;

        // Destroy clones one at a time more quickly
        float delayTime = 5f;
        
        // decrease the delay time
        for (int i = 0; i < spawnedSections.Count; i++)
        {
            // Wait for a shorter time before destroying the next clone
            yield return new WaitForSeconds(delayTime);

            // Destroy the i-th section in the list
            Destroy(spawnedSections[i]);
        }
    }
}

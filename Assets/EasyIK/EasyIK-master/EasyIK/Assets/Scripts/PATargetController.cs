using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class PATargetController : MonoBehaviour
{
    // Start is called before the first frame update
    private AIPath aiPath;
    private AIDestinationSetter aiDestinationSetter;

    void Awake()
    {
        aiPath = GetComponent<AIPath>();
        aiDestinationSetter = GetComponent<AIDestinationSetter>();
    }

    public void DisableAIPathFind()
    {
        aiPath.enabled = false;
        aiDestinationSetter.enabled = false;
    }

    public void EnableAIPathFind()
    {
        aiPath.enabled = true;
        aiDestinationSetter.enabled = true;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

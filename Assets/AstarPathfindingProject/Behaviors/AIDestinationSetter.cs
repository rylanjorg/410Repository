using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
    /// <summary>
    /// Sets the destination of an AI to the position of a specified object.
    /// This component should be attached to a GameObject together with a movement script such as AIPath, RichAI or AILerp.
    /// This component will then make the AI move towards the <see cref="target"/> set on this component.
    ///
    /// See: <see cref="Pathfinding.IAstarAI.destination"/>
    ///
    /// [Open online documentation to see images]
    /// </summary>
    [UniqueComponent(tag = "ai.destination")]
    [HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_a_i_destination_setter.php")]
    public class AIDestinationSetter : VersionedMonoBehaviour
    {
        /// <summary>The object that the AI should move to</summary>
        public Transform target;
        AIPath ai;

        public float agroRange = 5;
        public float extendedAgroRange = 8;
        public bool hasAgroed = false;
        public bool atStart = true;
        public Vector3 startPos;
        

        void OnEnable()
        {
            ai = GetComponent<AIPath>();
            // Update the destination right before searching for a path as well.
            // This is enough in theory, but this script will also update the destination every
            // frame as the destination is used for debugging and may be used for other things by other
            // scripts as well. So it makes sense that it is up to date every frame.
            if (ai != null) ai.onSearchPath += Update;
        }

        void OnDisable()
        {
            if (ai != null) ai.onSearchPath -= Update;
        }

        void Start()
        {
            // Sets target to Player
            //target = GameObject.FindGameObjectWithTag("Player").transform;
            startPos = transform.position;
        }

        public void SetTarget(Transform newTarget)
        {
            target = newTarget;
        }

        /// <summary>Updates the AI's destination every frame</summary>
        void Update()
        {
            // Sets toPlayer to difference between target and current position
            Vector3 toPlayer = target.position - transform.position;
            // if player is withing agroRange, sets hasAgroed and at start to true and sets destination as target's position
            if (toPlayer.magnitude <= agroRange)
            {
                hasAgroed = true;
                atStart = false;
                if (target != null && ai != null) ai.destination = target.position;
            }
            // otherwise if player is not in agroRange, sets hasAgroed to false, sets destination as startPos
            else if (toPlayer.magnitude > extendedAgroRange)
            {
                hasAgroed = false;
                if (target != null && ai != null) ai.destination = startPos;
                if(ai.reachedEndOfPath)
                {
                    atStart = true;
                }
            }
        }
    }
}

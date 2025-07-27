using UnityEngine;
using UnityEngine.UIElements;

namespace Pathfinding {
	/// <summary>
	/// Blocks single nodes in a graph.
	///
	/// This is useful in turn based games where you want
	/// units to avoid all other units while pathfinding
	/// but not be blocked by itself.
	///
	/// Note: To use this with a movement script, you have to assign the BlockManager's traversal provider to either <see cref="Seeker.traversalProvider"/> or <see cref="FollowerEntity.pathfindingSettings.traversalProvider"/>.
	///
	/// See: TurnBasedAI for example usage
	///
	/// See: BlockManager
	/// See: turnbased (view in online documentation for working links)
	/// See: traversal_provider (view in online documentation for working links)
	/// </summary>
	[HelpURL("https://arongranberg.com/astar/documentation/stable/singlenodeblocker.html")]
	public class SingleNodeBlocker : VersionedMonoBehaviour {
		public GraphNode lastBlocked { get; private set; }
		public BlockManager manager;

        protected override void Awake()
        {
            base.Awake();
        }

        /// <summary>
        /// Block node closest to the position of this object.
        ///
        /// Will unblock the last node that was reserved (if any)
        /// </summary>
        public void BlockAtCurrentPosition () {
			BlockAt(transform.position);
		}

		/// <summary>
		/// Block node closest to the specified position.
		///
		/// Will unblock the last node that was reserved (if any)
		/// </summary>
		public void BlockAt (Vector3 position) {
			Unblock();
			var node = AstarPath.active.GetNearest(position, NNConstraint.None).node;
			if (node != null) 
			{
				Debug.Log("Blocking Node at" + position);
				Block(node);
            }
            else
            {
				Debug.Log("Blocking Node at failed" + position);
			}
		}

		/// <summary>
		/// Block specified node.
		///
		/// Will unblock the last node that was reserved (if any)
		/// </summary>
		public void Block (GraphNode node) {
			if (node == null)
				throw new System.ArgumentNullException("node");

            Debug.Log("Blocking Node at" + (Vector3)node.position);


            manager.InternalBlock(node, this);
			lastBlocked = node;
		}

		/// <summary>Unblock the last node that was blocked (if any)</summary>
		public void Unblock () {
			if (lastBlocked == null || lastBlocked.Destroyed) {
				lastBlocked = null;
				return;
			}

            Debug.Log("Unblocking Node at" + (Vector3)lastBlocked.position);

            manager.InternalUnblock(lastBlocked, this);
			lastBlocked = null;
		}	
		
		public void UnblockAt (GraphNode node) {
            manager.InternalUnblock(node, this);
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Tools
{
    public static void test()
    {
        Debug.Log("tools test");
    }

	public static Get_NODE_BFS BFS = new Get_NODE_BFS();

    

}



public class Get_NODE_BFS
{
	public ObjectBase targetBase;

	public Node get_next_node(ObjectBase objectBase, ObjectBase targetBase)
	{
		this.targetBase = targetBase;

		return get_next_node_BFS(objectBase.node_now, this.targetBase.node_now);
	}

	private Node get_next_node_BFS(Node start_node, Node target_node)
	{

		int min_score = 100;
		Dictionary<Node, int> node_score = new Dictionary<Node, int>();
		Queue<Node> queue_node = new Queue<Node>();

		node_score.Add(start_node, 0);

		Node next_node = start_node;
		List<int> candidate = new List<int>();

		for (int i = 0; i < 4; i++)
		{
			candidate.Add(i);
		}

		for (int n = 4; n > 0; n--)
		{
			int i = candidate[Random.Range(0, n)];
			candidate.Remove(i);

			if (cant_move_node(start_node.UDLR_node[i])) continue;

			if (node_score.ContainsKey(start_node.UDLR_node[i])) node_score[start_node.UDLR_node[i]] = 1;
			else node_score.Add(start_node.UDLR_node[i], 1);

			queue_node.Clear();
			queue_node.Enqueue(start_node.UDLR_node[i]);


			int score = get_path_BFS(target_node, queue_node, node_score);

			if (min_score > score)
			{
				min_score = score;
				next_node = start_node.UDLR_node[i];
			}
		}

		return next_node;
	}

	private int get_path_BFS(Node target_node, Queue<Node> queue_node, Dictionary<Node, int> node_score)
	{

		if (queue_node.Count == 0) return 100;

		Node currnet_node = queue_node.Dequeue();
		int score = node_score[currnet_node];

		if (currnet_node == target_node) return score + 1;

		for (int i = 0; i < 4; i++)
		{
			Node next_node = currnet_node.UDLR_node[i];
			if (cant_move_node(next_node)) continue;

			if (!node_score.ContainsKey(next_node))
			{
				node_score.Add(next_node, score + 1);
				queue_node.Enqueue(next_node);

			}
			else if (node_score[next_node] > score + 1)
			{
				node_score[next_node] = score + 1;
				queue_node.Enqueue(next_node);
			}
		}
		return get_path_BFS(target_node, queue_node, node_score);
	}

	private bool cant_move_node(Node next_node)
	{
		if (next_node == null) return true;

		foreach (var objectbase in next_node.object_here_list)
		{
			if (objectbase == targetBase) return false;
			if (objectbase.is_collide) return true;
		}

		return false;
	}
}





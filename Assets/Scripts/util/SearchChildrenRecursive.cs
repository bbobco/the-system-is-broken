using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SearchChildrenRecursive
{
    public static Transform BFS( this Transform parent, string name) {
        Queue<Transform> queue = new Queue<Transform>();
         queue.Enqueue(parent);
         while (queue.Count > 0)
         {
             var c = queue.Dequeue();
             if (c.name == name)
                 return c;
             foreach(Transform t in c)
                 queue.Enqueue(t);
         }
         return null;
    }
}

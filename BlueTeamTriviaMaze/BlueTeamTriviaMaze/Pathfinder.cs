//Author: Blue Team (Elise Peterson, Cord Rehn, Zak Steele)
//Class: Spring 2014 CSCD 350-01
//Description: Handles checking whether or not a maze is complete-able from
//  an arbitrary room to the exit room

using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace BlueTeamTriviaMaze
{
    class Pathfinder
    {
        public bool PathExists { get; private set; }


	    private ArrayList closed;
	    private SimpleSortedList open;
	
	    private Node[,] nodes;
	

        public Pathfinder(Room start, Room end)
        {
            closed = new ArrayList();
            open = new SimpleSortedList();

		    nodes = new Node[MazeWindow.GetInstance().GetMaze().Rows, MazeWindow.GetInstance().GetMaze().Columns];
            for (int y = 0; y < nodes.GetLength(0); y++)
                for (int x = 0; x < nodes.GetLength(1); x++)
                    nodes[y, x] = new Node(MazeWindow.GetInstance().GetMaze().GetRoom(x, y));


            PathExists = DoesPathExist(start.X, start.Y, end.X, end.Y);
        }



        // Use A* path finding to determine if a path exists from (sx,sy) to (tx,ty) 
        private bool DoesPathExist(int sx, int sy, int tx, int ty)
        {
            closed.Clear();
            open.Clear();

            // add the starting node to the open list
		    open.Add(nodes[sy, sx]);

            nodes[sy, sx].cost = 0;
		    nodes[ty, tx].parent = null;
		

		    while (open.Size() != 0)
            {
			    // take the first node from open list
                Node current = (Node) open.First();

                // if the node is the target node, a path exists!
                if (current == nodes[ty, tx])
                    return true;
			    
			
			    open.Remove(current);
			    closed.Add(current);
			

			    // search through all the neighbours of the current node evaluating them as next steps
			    for (int x = -1; x < 2; x++) 
				    for (int y = -1; y < 2; y++)
                    {
					    // not a neighbour, its the current tile
					    if (x == 0 && y == 0) 
						    continue;
				

					    // no diagonal movement, only one of x or y can be set
						if (x != 0 && y != 0) 
							continue;
					

                        // can't move through locked doors!!
                        if (x > 0) {// east
                            if (current.room.EastDoor != null)
                                if (current.room.EastDoor.GetState() == Door.State.Locked)
                                    continue;
                        
                        } else if (x < 0) { // west
                            if (current.room.WestDoor != null)
                                if (current.room.WestDoor.GetState() == Door.State.Locked)
                                    continue;
                        }
                        else if (y < 0) { // north
                            if (current.room.NorthDoor != null)
                                if (current.room.NorthDoor.GetState() == Door.State.Locked)
                                    continue;
                        }
                        else if (y > 0) {  // south
                            if (current.room.SouthDoor != null)
                                if (current.room.SouthDoor.GetState() == Door.State.Locked)
                                    continue;
                        }



					    // if a neighbor room exists... evaluate!
					    int xp = x + current.x;
					    int yp = y + current.y;
					
					    if (MazeWindow.GetInstance().GetMaze().GetRoom(xp, yp) != null)
                        {  
						    // so the cost to get to this node is the current node's cost plus 1 (the cost to get to this node...)
                            float nextStepCost = current.cost + 1;
						    Node neighbour = nodes[yp, xp];


						    // determine if there might be a better path to get to this node, if so then it needs to be re-evaluated
						    if (nextStepCost < neighbour.cost) {
							    if (open.Contains(neighbour)) 
								    open.Remove(neighbour);
							    
							    if (closed.Contains(neighbour)) 
								    closed.Remove(neighbour);
						    }
						
						    // if the node hasn't already been processed...
						    if (!open.Contains(neighbour) && !closed.Contains(neighbour)) {
							    neighbour.cost = nextStepCost;

                                float dx = tx - xp;
                                float dy = ty - yp;  // use the distance formula for the heuristic 
                                neighbour.heuristic = (float)(Math.Sqrt((dx * dx) + (dy * dy)));

							    open.Add(neighbour);
						    }
					    }
				    }
		    }

            return false;
	    }




	    private class SimpleSortedList {
		    private ArrayList list = new ArrayList();
		
		    public Object First() {
                return list[0];
		    }
		
		    public void Clear() {
                list.Clear();
		    }
		
		    public void Add(Object o) {
                list.Add(o);
                list.Sort(); // sort on add = a sorted list
		    }
		
		    public void Remove(Object o) {
			    list.Remove(o);
		    }
	
		    public int Size() {
			    return list.Count;
		    }
		
		    public bool Contains(Object o) {
			    return list.Contains(o);
		    }
	    }
	


	    private class Node : IComparable, IEquatable<Node> {
		    public int x;
            public int y;
            public float cost;
            public float heuristic;
            public Room room;
            public Node parent;
		
		    public Node(Room room) {
                this.room = room;
			    this.x = room.X;
			    this.y = room.Y;
		    }
		
		    public int CompareTo(Object other)
            {
			    Node o = (Node)other;
			
			    float f = heuristic + cost;
			    float f_other = o.heuristic + o.cost; // essentially- compare based on shortest distance

                if (f < f_other) 
				    return -1;
                if (f > f_other) 
				    return 1;
				
                return 0;
		    }

            public override bool Equals(object obj)
            {
                if (obj == null)
                    return false;

                Node objAsNode = obj as Node;
                if (objAsNode == null)
                    return false;
                
                return Equals(objAsNode);
            }

            public bool Equals(Node other)
            {
                if (other == null)
                    return false;

                return this.x == other.x && this.y == other.y;
            }
	    }









    }
}

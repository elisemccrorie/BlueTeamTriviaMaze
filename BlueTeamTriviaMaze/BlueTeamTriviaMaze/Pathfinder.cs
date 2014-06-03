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
	    private SortedList open;
	
	    private Node[,] nodes;
	

        public Pathfinder(Room start, Room end)
        {
            closed = new ArrayList();
            open = new SortedList();

		    nodes = new Node[MazeWindow.GetInstance().GetMaze().Columns, MazeWindow.GetInstance().GetMaze().Rows];
		    for (int x=0; x < nodes.GetLength(0); x++)
                for (int y = 0; y < nodes.GetLength(1); y++)
                    nodes[x, y] = new Node(MazeWindow.GetInstance().GetMaze().GetRoom(x, y));

            FindPath(start.X, start.Y, end.X, end.Y);
        }




	    private void FindPath(int sx, int sy, int tx, int ty) {

            PathExists = false;

		
		    // initial state for A*. The closed group is empty. Only the starting
		    // tile is in the open list and it'e're already there
		    nodes[sx, sy].cost = 0;
		    closed.Clear();
		    open.Clear();
		    open.Add(nodes[sx, sy]);
		
		    nodes[tx, ty].parent = null;
		

		    while (open.Size() != 0)
            {
			    // pull out the first node in our open list, this is determined to 
			    // be the most likely to be the next step based on our heuristic
			    Node current = GetFirstInOpen();
                if (current == nodes[tx, ty])
                {
                    PathExists = true;
                    break;
                }
			    
			
			    RemoveFromOpen(current);
			    AddToClosed(current);
			

			    // search through all the neighbours of the current node evaluating
			    // them as next steps
			    for (int x = -1; x < 2; x++) 
				    for (int y = -1; y < 2; y++)
                    {
					    // not a neighbour, its the current tile
					    if ((x == 0) && (y == 0)) 
						    continue;
				

					    // no diagonal movement, only one of x or y can be set
						if ((x != 0) && (y != 0)) 
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
                        else if (y > 0) { // north
                            if (current.room.NorthDoor != null)
                                if (current.room.NorthDoor.GetState() == Door.State.Locked)
                                    continue;
                        }
                        else if (y < 0) {  // south
                            if (current.room.SouthDoor != null)
                                if (current.room.SouthDoor.GetState() == Door.State.Locked)
                                    continue;
                        }



					    // determine the location of the neighbour and evaluate it
					    int xp = x + current.x;
					    int yp = y + current.y;
					
					    if (MazeWindow.GetInstance().GetMaze().GetRoom(xp, yp) != null) { // room exists
						    // the cost to get to this node is cost the current plus the movement
						    // cost to reach this node. Note that the heursitic value is only used
						    // in the sorted open list

                            float nextStepCost = current.cost + 1;
						    Node neighbour = nodes[xp,yp];
						
						    // if the new cost we've determined for this node is lower than 
						    // it has been previously makes sure the node hasn'e've
						    // determined that there might have been a better path to get to
						    // this node so it needs to be re-evaluated

						    if (nextStepCost < neighbour.cost) {
							    if (InOpenList(neighbour)) {
								    RemoveFromOpen(neighbour);
							    }
							    if (InClosedList(neighbour)) {
								    RemoveFromClosed(neighbour);
							    }
						    }
						
						    // if the node hasn't already been processed and discarded then
						    // reset it's cost to our current cost and add it as a next possible
						    // step (i.e. to the open list)

						    if (!InOpenList(neighbour) && !InClosedList(neighbour)) {
							    neighbour.cost = nextStepCost;
							    neighbour.heuristic = GetHeuristicCost(xp, yp, tx, ty);
							    AddToOpen(neighbour);
						    }
					    }
				    }
			    
		    }
	    }

        private Node GetFirstInOpen()
        {
		    return (Node) open.First();
	    }

        private void AddToOpen(Node node)
        {
		    open.Add(node);
	    }

        private bool InOpenList(Node node)
        {
		    return open.Contains(node);
	    }

        private void RemoveFromOpen(Node node)
        {
		    open.Remove(node);
	    }

        private void AddToClosed(Node node)
        {
		    closed.Add(node);
	    }

        private bool InClosedList(Node node)
        {
		    return closed.Contains(node);
	    }

        private void RemoveFromClosed(Node node)
        {
		    closed.Remove(node);
	    }

        private float GetHeuristicCost(int x, int y, int tx, int ty)
        {
            float dx = tx - x;
            float dy = ty - y;
            return (float)(Math.Sqrt((dx * dx) + (dy * dy)));
	    }
	


	    private class SortedList {
		    private ArrayList list = new ArrayList();
		
		    public Object First() {
                return list[0];
		    }
		
		    public void Clear() {
                list.Clear();
		    }
		
		    public void Add(Object o) {
                list.Add(o);
                list.Sort();
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
            public Node parent;
            public float heuristic;
            public Room room;
		
		    public Node(Room room) {
                this.room = room;
			    this.x = room.X;
			    this.y = room.Y;
		    }
		
		    public void setParent(Node parent) {
			    this.parent = parent;
		    }
		
		    public int CompareTo(Object other) {
			    Node o = (Node)other;
			
			    float f = heuristic + cost;
			    float of = o.heuristic + o.cost;
			
			    if (f < of) {
				    return -1;
			    } else if (f > of) {
				    return 1;
			    } else {
				    return 0;
			    }
		    }

            public override bool Equals(object obj)
            {
                if (obj == null) return false;
                Node objAsNode = obj as Node;
                if (objAsNode == null) return false;
                else return Equals(objAsNode);
            }

            public bool Equals(Node other)
            {
                if (other == null) return false;
                return this.x == other.x && this.y == other.y;
            }
	    }









    }
}

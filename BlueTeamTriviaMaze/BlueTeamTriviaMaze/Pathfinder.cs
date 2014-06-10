//Author: Blue Team (Elise Peterson, Cord Rehn, Zak Steele)
//Class: Spring 2014 CSCD 350-01
//Description: Handles checking whether or not a maze is complete-able from
//  an arbitrary room to the exit room

using System;
using System.Collections;

namespace BlueTeamTriviaMaze
{
    class Pathfinder
    {
        private ArrayList _closed;
        private SimpleSortedList _open;
        private Node[,] _nodes;

        public bool PathExists { get; private set; }

        public Pathfinder(Room start, Room end)
        {
            _closed = new ArrayList();
            _open = new SimpleSortedList();

		    _nodes = new Node[MazeWindow.GetInstance().GetMaze().Rows, MazeWindow.GetInstance().GetMaze().Columns];
            for (int y = 0; y < _nodes.GetLength(0); y++)
                for (int x = 0; x < _nodes.GetLength(1); x++)
                    _nodes[y, x] = new Node(MazeWindow.GetInstance().GetMaze().GetRoom(x, y));

            if (MazeWindow.GetInstance().GetMaze().GetPlayer().Keys > 0)
                PathExists = true;
            else
                PathExists = DoesPathExist(start.X, start.Y, end.X, end.Y);
        }

        // Use A* path finding to determine if a path exists from (sx,sy) to (tx,ty) 
        private bool DoesPathExist(int sx, int sy, int tx, int ty)
        {
            _closed.Clear();
            _open.Clear();

            // add the starting node to the open list
		    _open.Add(_nodes[sy, sx]);

            _nodes[sy, sx]._cost = 0;
		    _nodes[ty, tx]._parent = null;
		

		    while (_open.Size() != 0)
            {
			    // take the first node from open list
                Node current = (Node) _open.First();

                // if the node is the target node, a path exists!
                if (current == _nodes[ty, tx])
                    return true;
			    
			
			    _open.Remove(current);
			    _closed.Add(current);
			

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
                        if (x > 0) 
                        {// east
                            if (current._room.EastDoor != null)
                                if (current._room.EastDoor.GetState() == Door.State.Locked)
                                    continue;
                        
                        } 
                        else if (x < 0) 
                        { // west
                            if (current._room.WestDoor != null)
                                if (current._room.WestDoor.GetState() == Door.State.Locked)
                                    continue;
                        }
                        else if (y < 0) 
                        { // north
                            if (current._room.NorthDoor != null)
                                if (current._room.NorthDoor.GetState() == Door.State.Locked)
                                    continue;
                        }
                        else if (y > 0) 
                        {  // south
                            if (current._room.SouthDoor != null)
                                if (current._room.SouthDoor.GetState() == Door.State.Locked)
                                    continue;
                        }



					    // if a neighbor room exists... evaluate!
					    int xp = x + current._x;
					    int yp = y + current._y;
					
					    if (MazeWindow.GetInstance().GetMaze().GetRoom(xp, yp) != null)
                        {  
						    // so the cost to get to this node is the current node's cost plus 1 (the cost to get to this node...)
                            float nextStepCost = current._cost + 1;
						    Node neighbour = _nodes[yp, xp];


						    // determine if there might be a better path to get to this node, if so then it needs to be re-evaluated
						    if (nextStepCost < neighbour._cost) 
                            {
							    if (_open.Contains(neighbour)) 
								    _open.Remove(neighbour);
							    
							    if (_closed.Contains(neighbour)) 
								    _closed.Remove(neighbour);
						    }
						
						    // if the node hasn't already been processed...
						    if (!_open.Contains(neighbour) && !_closed.Contains(neighbour)) 
                            {
							    neighbour._cost = nextStepCost;

                                float dx = tx - xp;
                                float dy = ty - yp;  // use the distance formula for the heuristic 
                                neighbour._heuristic = (float)(Math.Sqrt((dx * dx) + (dy * dy)));

							    _open.Add(neighbour);
						    }
					    }
				    }
		    }

            return false;
	    }//end DoesPathExist(int, int, int, int)

        //nested class
	    private class SimpleSortedList 
        {
		    private ArrayList _list = new ArrayList();
		
		    public Object First() 
            {
                return _list[0];
		    }
		
		    public void Clear() 
            {
                _list.Clear();
		    }
		
		    public void Add(Object o) 
            {
                _list.Add(o);
                _list.Sort(); // sort on add = a sorted list
		    }
		
		    public void Remove(Object o) 
            {
			    _list.Remove(o);
		    }
	
		    public int Size() 
            {
			    return _list.Count;
		    }
		
		    public bool Contains(Object o) 
            {
			    return _list.Contains(o);
		    }
	    }//end SimpleSortedList
	

        //nested class
	    private class Node : IComparable, IEquatable<Node> 
        {
		    public int _x;
            public int _y;
            public float _cost;
            public float _heuristic;
            public Room _room;
            public Node _parent;
		
		    public Node(Room room) 
            {
                this._room = room;
			    this._x = room.X;
			    this._y = room.Y;
		    }
		
		    public int CompareTo(Object other)
            {
			    Node o = (Node)other;
			
			    float f = _heuristic + _cost;
			    float f_other = o._heuristic + o._cost; // essentially- compare based on shortest distance

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

                return this._x == other._x && this._y == other._y;
            }
	    }
    }
}

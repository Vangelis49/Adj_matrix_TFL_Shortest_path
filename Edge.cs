using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using static System.Collections.Specialized.BitVector32;

namespace TFLShortestPathFinder
{
    internal class Edge
    {
        //A private Station field representing the parent station of the edge
        private Station _parent;
        public Station parent { get { return _parent; } set { _parent = value; } }

        //A private Station field representing the child station of the edge.
        private Station _child;
        public Station child { get { return _child; } set { _child = value; } }

        //A private int field representing the weight (walking time) of the edge
        private int _weight;
        public int weight { get { return _weight; } set { _weight = value; } }

        //new stuff
        public bool isPossible;

        public Edge(Station child, int weight)
        {
            this.child = child;
            this.weight = weight;
            this.isPossible = true;
        }
    }
}

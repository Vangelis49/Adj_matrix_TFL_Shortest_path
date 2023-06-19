
using System.Collections.Generic;
using System.Xml.Linq;
using TFLShortestPathFinder.Enum;

namespace TFLShortestPathFinder
{
    internal class Station
    {
        //name: A string representing the name of the station
        //stationAccess: An enumeration of type StationAccess (defined in the
        //TFLShortestPathFinder.Enum namespace), which represents the access type of station (e.g., stairs, escalator, etc.)
        //isActive: A boolean flag indicating whether the station is currently active (open) or not (closed)
        //edges: A custom list of type MyList<Edge> that represents the edges (connections) between this station and its neighboring stations

        private string _name;
        public string name { get { return _name; } set { _name = value; } }

        private StationAccess _stationAccess;
        public StationAccess stationAccess { get { return _stationAccess; } set { _stationAccess = value; } }

        private bool _isActive;
        public bool isActive { get { return _isActive; } set { _isActive = value; } }

        //this is for add or remove delay
        public int delay { get; set; }

        private MyList<Edge> _edges = new MyList<Edge>();
        public MyList<Edge> edges { get { return _edges; } set { _edges = value; } }


        //Constructor: The Station class has a constructor that takes three parameters: name, stationAccess, and status.
        //It initializes the corresponding properties with the provided values.
        public Station(string name, StationAccess stationAccess, bool status)
        {
            this.name = name;
            this.stationAccess = stationAccess;
            this.isActive = status;
        }

        

        //new for case 2
        public void AddTwoWayEdge(Station station, int w, bool isPossible = true)
        {
            Edge edge1 = new Edge(station, w) { isPossible = isPossible };
            edges.Add(edge1);

            Edge edge2 = new Edge(this, w) { isPossible = isPossible };
            station.edges.Add(edge2);
        }



        //new addition
        public string GetLineName()
        {
            int startParenthesis = name.IndexOf("(");
            int endParenthesis = name.IndexOf(")");

            if (startParenthesis != -1 && endParenthesis != -1)
            {
                return name.Substring(startParenthesis + 1, endParenthesis - startParenthesis - 1);
            }

            return "";
        }

        public static Station FindStationByName(string stationName, MyList<Station> stations)
        {
            return stations.FirstOrDefault(station => station.name.Equals(stationName, StringComparison.OrdinalIgnoreCase));
        }

        //ToString method: This method overrides the default ToString method and returns a string representation of the Station object.
        //It displays the station's name, access type, and status (open or closed).
        public override string ToString()
        {
            string resp = isActive ? "Open" : "Closed";

            //return $"Station Name: {name} \n" +
            //    $"Station Access: {stationAccess} \n" +
            //    $"Station Status: {resp}";
            return $"\nStation Name: {name} \n" +
                $"Station Access: {stationAccess} \n" +
                $"Travel Zone: 1 \n" +
                $"Station Status: {resp}\n";
        }
    }
}



using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using TFLShortestPathFinder.Enum;
using static System.Collections.Specialized.BitVector32;

namespace TFLShortestPathFinder
{
    internal class Graph
    {

        //_stations: A private MyList<Station> field to store the stations in the graph
        //stations: A public property to get and set the _stations field value.
        private MyList<Station> _stations = new MyList<Station>();
        public MyList<Station> stations { get { return _stations; } set { _stations = value; } }

        //Creates a new Station object with the given name, station access, and status, adds it to the stations list, and returns the new Station.
        public Station CreateStation(string name, StationAccess stationAccess, bool status)
        {
            Station newStation = new Station(name, stationAccess, status);
            stations.Add(newStation);
            return newStation;
        }

        //Creates an adjacency matrix representing the graph. The matrix is a 2D integer array, where adjMatrix[i, j] represents the weight (walking time) between station i and station j.
        //If there's no direct connection between two stations, the corresponding value in the adjacency matrix will be 0.
        public int[,] CreateAdjMatrix()
        {
            int[,] adjMatrix = new int[stations.Count(), stations.Count()];
            for (int i = 0; i < stations.Count(); i++)
            {
                Station st1 = stations[i];
                for (int j = 0; j < stations.Count(); j++)
                {
                    Station st2 = stations[j];
                    for (int k = 0; k < st1.edges.Count(); k++)
                    //foreach (Edge e in st1.edges)
                    {
                        if (st1.edges[k].child == st2)
                        {
                            adjMatrix[i, j] = st1.edges[k].weight;
                        }
                    }
                }
            }
            return adjMatrix;
        }

        //Prints the adjacency matrix to the console. This method is mainly for debugging purposes and visualizing the graph structure.
        public void Print(int[,] adjMatrix, int N)
        {
            for (int i = 0; i < N; i++)
            {
                Console.Write("[", (char)('A' + i));

                for (int j = 0; j < N; j++)
                {
                    if (i == j)
                    {
                        Console.Write(" 0 ");
                    }
                    else
                    {
                        Console.Write(" {0} ", adjMatrix[i, j]);
                    }

                }
                Console.WriteLine("]");
            }
        }

        //new stuff for case 1
        public Station GetStationByName(string stationName)
        {
            int index = 0;
            Station currentStation = null;

            try
            {
                currentStation = stations[index];
            }
            catch (Exception)
            {
                return null;
            }

            while (currentStation != null)
            {
                if (currentStation.name == stationName)
                {
                    return currentStation;
                }
                index++;

                try
                {
                    currentStation = stations[index];
                }
                catch (Exception)
                {
                    return null;
                }
            }
            return null;
        }

        public void UpdateWalkingTimeDelay(string fromStation, string toStation, int delay)
        {
            Station from = GetStationByName(fromStation);
            Station to = GetStationByName(toStation);

            if (from != null && to != null)
            {
                for (int i = 0; i < from.edges.Count(); i++)
                {
                    if (from.edges[i].child == to)
                    {
                        from.edges[i].weight += delay;
                        Console.WriteLine($"Updated delay for {fromStation} to {toStation}: {from.edges[i].weight} minutes");
                        break;
                    }
                }

                for (int i = 0; i < to.edges.Count(); i++)
                {
                    if (to.edges[i].child == from)
                    {
                        to.edges[i].weight += delay;
                        Console.WriteLine($"Updated delay for {toStation} to {fromStation}: {to.edges[i].weight} minutes");
                        break;
                    }
                }
            }
            else
            {
                Console.WriteLine("One or both of the stations were not found.");
            }
        }

        public void AddRemoveDelays()
        {
            Console.WriteLine("Enter the starting station:");
            string fromStation = Console.ReadLine();

            Console.WriteLine("Enter the ending station:");
            string toStation = Console.ReadLine();

            Console.WriteLine("Enter the delay in minutes (use a negative value to remove delay):");
            int delay = int.Parse(Console.ReadLine());

            UpdateWalkingTimeDelay(fromStation, toStation, delay);
        }

        public int GetWalkingTime(Station fromStation, Station toStation)
        {
            Edge edge = fromStation.edges.FirstOrDefault(e => e.child == toStation);
            if (edge != null)
            {
                return edge.weight;
            }
            return int.MaxValue;
        }

        //for case 2
        public void MarkRoutePossibleOrImpossible(string fromStationName, string toStationName, bool isPossible)
        {
            Station fromStation = GetStationByName(fromStationName);
            Station toStation = GetStationByName(toStationName);

            if (fromStation != null && toStation != null)
            {
                Edge edge = fromStation.edges.FirstOrDefault(e => e.child == toStation);
                if (edge != null)
                {
                    edge.isPossible = isPossible;
                    Console.WriteLine($"Route from {fromStation.name} to {toStation.name} marked as {(isPossible ? "possible" : "impossible")}");
                }
                else
                {
                    Console.WriteLine("Edge not found.");
                }

                // Update the reverse edge (from toStation to fromStation) as well
                Edge reverseEdge = toStation.edges.FirstOrDefault(e => e.child == fromStation);
                if (reverseEdge != null)
                {
                    reverseEdge.isPossible = isPossible;
                }
                else
                {
                    Console.WriteLine("Reverse edge not found.");
                }
            }
            else
            {
                Console.WriteLine("One or both stations not found.");
            }
        }


        //for case 3
        public void PrintImpossibleWalkingRoutes()
        {
            Console.WriteLine("Closed routes:");
            for (int i = 0; i < stations.Count(); i++)
            {
                Station station = stations[i];
                for (int j = 0; j < station.edges.Count(); j++)
                {
                    Edge edge = station.edges[j];
                    if (!edge.isPossible)
                    {
                        // Print the stations and the reason for the route being impossible
                        Console.WriteLine($"{station.GetLineName()}: {station.name} - {edge.child.name} : route closed");
                    }
                }
            }
        }

        //for case 4
        public void PrintDelayedWalkingRoutes(int[,] adj, int[,] adj_backup)
        {
            Console.WriteLine("Delayed Routes");

            for (int i = 0; i < adj.GetLength(0); i++)
            {
                Station station1 = stations[i];
                for (int j = 0; j < adj_backup.GetLength(0); j++)
                {
                    if (adj[i, j] != adj_backup[i, j])
                    {
                        for (int k = 0; k < station1.edges.Count(); k++)
                        {
                            Edge edge = station1.edges[k];
                            Station from = stations[i];
                            Station to = stations[j];

                            if (from.edges[k].child == to)
                            {
                                Console.WriteLine($"{station1.name} - {edge.child.name} : {adj_backup[i, j]} now {adj[i, j]}");
                                break;
                            }

                            if (to.edges[k].child == from)
                            {
                                Console.WriteLine($"{station1.name} - {edge.child.name} : {adj_backup[i, j]} now {adj[i, j]}");
                                break;
                            }
                            // Print the stations and the reason for the route being impossible
                            //Console.WriteLine($"{station1.GetLineName()}: {station1.name} - {edge.child.name} : {adj_backup[i, j]} now {adj[i, j]}");
                        }
                    }
                }
            }
        }

        public void DijkstraAlgorithm(Station source, Station target, Graph graph, int[,] adj)
        {
            int length = graph.stations.Count();
            int[] distance = new int[length];
            string[] previous = new string[length];
            string[] unvisited = new string[length];
            // keep an original copy to find later at line 267 the j 
            string[] original = new string[length];
            //initialize arrays
            for (int i = 0; i < length; i++)
            {
                distance[i] = int.MaxValue;
                unvisited[i] = graph.stations[i].name;
                original[i] = graph.stations[i].name;
            }
            distance[graph.stations.Indexof(source)] = 0;
            Station current;
            while (unvisited.Length > 0)
            {
                // node with min dist from sourse
                int min = int.MaxValue;
                int i = -1; // min index
                for (int k = 0; k < length; k++)
                {
                    // need to check only the queue Q = unvisited
                    // visit only the UNVISITED vertex with the
                    // smallest distance from the start
                    if (unvisited.Contains(original[k]) && distance[k] <= min)
                    {
                        min = distance[k];
                        i = k;
                    }
                }
                current = graph.stations[i];

                if (current == target)
                {
                    break;
                }
                // remove current from Q
                unvisited = unvisited.Where(val => val != current.name).ToArray();

                //new stuff for case 2
                for (int l = 0; l < current.edges.Count(); l++)
                {
                    if (unvisited.Contains(current.edges[l].child.name) && current.edges[l].isPossible)
                    {
                        int j = Array.IndexOf(original, current.edges[l].child.name);
                        int alt = distance[i] + adj[i, j];
                        if (alt < distance[j])
                        {
                            distance[j] = alt;
                            previous[j] = current.name;
                        }
                    }
                }


            }

            MyList<string> route = new MyList<string>();
            Console.WriteLine($"Route: {source.name} to {target.name}:");

            // Initialize the current station to the target station
            string cur = target.name;

            // Traverse the path from the target station to the source station (in reverse)
            while (cur != source.name)
            {
                route.Add(cur);
                int index = Array.IndexOf(original, cur);
                cur = previous[index];
            }

            // Add the source station to the route and reverse the order to get the correct sequence
            route.Add(source.name);
            route.Reverse();

            // Iterate through the stations in the route
            for (int i = 0; i < route.Count(); i++)
            {
                int index = Array.IndexOf(original, route[i]);
                Station currentStation = graph.stations[index];

                // If it's the first station, print the starting point
                if (i == 0)
                {
                    Console.WriteLine($"({i + 1}) Start:  {route[i]}");
                }
                else
                {
                    // Calculate the travel time between the current and previous stations
                    int travelTime = distance[index] - distance[Array.IndexOf(original, route[i - 1])];
                    Station previousStation = graph.stations[Array.IndexOf(original, route[i - 1])];

                    // If the current and previous stations are on different lines, print the change of line
                    if (currentStation.GetLineName() != previousStation.GetLineName())
                    {
                        //Console.WriteLine($"    Change: {previousStation.name} ({previousStation.GetLineName()}) to {currentStation.name}"); //({currentStation.GetLineName()})
                    }

                    // Print the travel from the previous station to the current station with the travel time
                    if(travelTime > 0)
                    {
                        Console.WriteLine($"({i + 1})         {previousStation.name} to {currentStation.name} {travelTime} min"); 
                    }
                    else
                    {
                        Console.WriteLine($"({i + 1}) Change: {previousStation.name} to {currentStation.name} {travelTime} min"); 
                    }
                }
            }

            // Print the ending point of the route
            Console.WriteLine($"({route.Count() + 1}) End:    {target.name}");

            // Print the total journey time
            Console.WriteLine($"Total Journey Time: {distance[Array.IndexOf(original, target.name)]} minutes \n");
        }
    }
}

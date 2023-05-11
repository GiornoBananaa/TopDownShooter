using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager : MonoBehaviour
{
    public Transform[] GetPatrolPaths(RoomManager currentPosition)
    {
        return currentPosition.transform.GetComponentsInChildren<Transform>();
    }

    public Transform[] FindPath(RoomManager currentPosition, RoomManager destination)
    {
        if (currentPosition == destination)
        {
            return new Transform[]{currentPosition.transform};
        }

        List<RoomManager>[] pathes = new List<RoomManager>[currentPosition.AccessibleRooms.Length];
        int minLengthIndex = 0;
        for (int i = 0; i < currentPosition.AccessibleRooms.Length; i++)
        {
            pathes[i] = FindPath(currentPosition, currentPosition.AccessibleRooms[i], destination, new List<RoomManager> { });
            if (pathes[minLengthIndex] is not null && pathes[minLengthIndex].Count > pathes[i].Count) minLengthIndex = i;
        }

        if (pathes[minLengthIndex] is null) return null;
        pathes[minLengthIndex].Add(currentPosition);

        Transform[] path = new Transform[pathes[minLengthIndex].Count];

        int count = pathes[minLengthIndex].Count;
        for (int i = 0; i < count; i++)
        {
            path[i] = pathes[minLengthIndex][count - 1 - i].transform;
        }

        return path;
    }

    private List<RoomManager> FindPath(RoomManager startPosition, RoomManager currentPosition, RoomManager destination, List<RoomManager> previousPositions)
    {
        if (currentPosition == destination)
        {
            List<RoomManager> path = new List<RoomManager>();
            path.Add(currentPosition);
            return path;
        }
        else if (currentPosition == startPosition)
        {
            return null;
        }

        if (previousPositions.Contains(currentPosition)) return null;
        previousPositions.Add(currentPosition);

        List<RoomManager>[] pathes = new List<RoomManager>[currentPosition.AccessibleRooms.Length];
        int minLengthIndex = 0;
        for(int i = 0; i < currentPosition.AccessibleRooms.Length; i++)
        {
            pathes[i] = FindPath(startPosition, currentPosition.AccessibleRooms[i], destination, previousPositions);
            if(pathes[minLengthIndex] is null && pathes[i] is not null) minLengthIndex = i;
            if (pathes[i] is not null && pathes[minLengthIndex] is not null && pathes[minLengthIndex].Count > pathes[i].Count) minLengthIndex = i;
        }

        if(pathes[minLengthIndex] is not null) pathes[minLengthIndex].Add(currentPosition);

        return pathes[minLengthIndex];
    }

    public RoomManager FindPlayer()
    {
        foreach(RoomManager room in transform.GetComponentsInChildren<RoomManager>())
        {
            if (room.PlayerIsInside) return room;
        }
        return transform.GetChild(4).GetComponent<RoomManager>();
    }
}

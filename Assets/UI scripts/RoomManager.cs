using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField] private GameObject[] rooms;
    private int currentRoomIndex = 0;

    void Start()
    {
        ShowRoom(currentRoomIndex);
    }

    public void ShowNextRoom()
    {
        if (currentRoomIndex + 1 < rooms.Length)
        {
            currentRoomIndex++;
            ShowRoom(currentRoomIndex);
        }
    }

    public void ShowRoom(int index)
    {
        for (int i = 0; i < rooms.Length; i++)
        {
            rooms[i].SetActive(i == index);
        }
    }
}

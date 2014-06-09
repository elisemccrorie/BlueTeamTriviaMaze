//Author: Blue Team (Elise Peterson, Cord Rehn, Zak Steele)
//Class: Spring 2014 CSCD 350-01
//Description: An interface for databases to adhere to

namespace BlueTeamTriviaMaze
{
    public interface IDatabase
    {
        bool Connect();
        bool Disconnect();
        bool CreateTable();
    }
}

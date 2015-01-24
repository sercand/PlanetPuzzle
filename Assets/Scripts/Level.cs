// #
// # Created by Sercan Degirmenci on 2015.01.24
// #
namespace Assets.Scripts
{
    public class Level
    {
        public int LevelID;
        public int PlanetCount;
        public int[] PlanetPieceCounts;
		public float MeteorSpawnPerSec;

		public Level(int id,int planetCount,int[] planetPieceCounts, float meteorSpawnPerSec)
		{
			LevelID = id;
			PlanetCount = planetCount;
			MeteorSpawnPerSec = meteorSpawnPerSec;
			PlanetPieceCounts = planetPieceCounts;

		}



    }
}
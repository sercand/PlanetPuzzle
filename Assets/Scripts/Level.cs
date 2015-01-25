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
		public float MeteorSpawnPeriodInSeconds;

		public Level(int id,int planetCount,int[] planetPieceCounts, float meteorSpawnPeriodInSeconds)
		{
			LevelID = id;
			PlanetCount = planetCount;
			MeteorSpawnPeriodInSeconds = meteorSpawnPeriodInSeconds;
			PlanetPieceCounts = planetPieceCounts;

		}



    }
}
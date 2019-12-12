namespace UnityEngine.Playables
{
	public static class PlayableDirectorExtensions
	{
		/// <summary>
		/// Rewinds to time 0.
		/// </summary>
		/// <param name="director"></param>
		public static void Rewind(this PlayableDirector director)
		{
			director.time = 0f;
		}
		
		
		/// <summary>
		/// Stops this PlayableDirector and rewind it to 0.
		/// </summary>
		/// <param name="director"></param>
		public static void StopAndRewind(this PlayableDirector director)
		{
			director.Stop();
			director.Rewind();
		}
	}
}

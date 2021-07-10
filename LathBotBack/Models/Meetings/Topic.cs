namespace LathBotBack.Models.Meetings
{
	public class Topic
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int Meeting { get; set; }
		public string Text { get; set; }
		public int Initiator { get; set; }
	}
}

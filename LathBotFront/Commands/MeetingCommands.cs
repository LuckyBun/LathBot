using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using LathBotBack.Config;
using LathBotBack.Models.Meetings;
using LathBotBack.Repos.Meetings;
using System.Threading.Tasks;

namespace LathBotFront.Commands
{
	[Group("meeting")]
	[RequireUserPermissions(Permissions.Administrator)]
	public class MeetingCommands : BaseCommandModule
	{
		[Group("create")]
		[RequireUserPermissions(Permissions.Administrator)]
		public class Create
		{
			[GroupCommand]
			[RequireUserPermissions(Permissions.Administrator)]
			public async Task Meeting(CommandContext ctx)
			{
				MeetingRepository repo = new MeetingRepository(ReadConfig.configJson.ConnectionString);
				if (!repo.GetLastMeeting(out Meeting lastMeeting))
				{
					await ctx.RespondAsync("Error getting the last meeting, creation aborted.");
					return;
				}
				Meeting newMeeting = new Meeting() { Number = lastMeeting.Number + 1 };
				if (!repo.Create(ref newMeeting))
				{
					await ctx.RespondAsync("Error creating the meeting.");
					return;
				}
				await ctx.RespondAsync($"New meeting with id {newMeeting.Id} and number {newMeeting.Number} has been created.");
			}

			[Command("initiator")]
			[RequireUserPermissions(Permissions.Administrator)]
			public async Task Initiator(CommandContext ctx, [RemainingText]string name)
			{
				InitiatorRepository repo = new InitiatorRepository(ReadConfig.configJson.ConnectionString);
				Initiator initiator = new Initiator() { Name = name };
				if (!repo.Create(ref initiator))
				{
					await ctx.RespondAsync("Error creating the initiator.");
					return;
				}
				await ctx.RespondAsync($"New initiator with id {initiator.Id} and name {initiator.Name} has been created.");
			}

			//[Command("topic")]
			//[RequireUserPermissions(Permissions.Administrator)]
			//public async Task Topic(CommandContext ctx, int meeting, int initiator, [RemainingText]string name)
			//{

			//}
		}

		[GroupCommand]
		[RequireUserPermissions(Permissions.Administrator)]
		public async Task Show(CommandContext ctx)
		{
			
		}
	}
}

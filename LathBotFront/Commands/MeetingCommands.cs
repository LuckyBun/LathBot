using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Threading.Tasks;

namespace LathBotFront.Commands
{
	[Group("meeting")]
	[RequireUserPermissions(Permissions.Administrator)]
	public class MeetingCommands : BaseCommandModule
	{
		[Command("add")]
		public async Task Add(CommandContext ctx)
		{

		}
	}
}

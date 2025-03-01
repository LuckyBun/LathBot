﻿using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.Extensions.PlatformAbstractions;

using DSharpPlus.Entities;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

using LathBotBack.Repos;
using LathBotBack.Config;
using LathBotBack.Models;
using LathBotBack.Services;

namespace LathBotFront.Commands
{
	public class InfoCommands : BaseCommandModule
	{
		[Command("info")]
		[Description("Display some info about the bot")]
		public async Task Info(CommandContext ctx)
		{
			DiscordEmbedBuilder discordEmbed = new DiscordEmbedBuilder
			{
				Color = new DiscordColor(101, 24, 201),
				Title = "LathBot#1753",
				Description = $"LathBot is a custom bot for the server Lathland, prefix is - or {ctx.Client.CurrentUser.Mention}\n" +
					"For more info use -help, -tos, or -privacy",
				Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail { Url = ctx.Client.CurrentUser.AvatarUrl }
			};
			discordEmbed.AddField("Language", "C# using Visual Studio 2019");
			discordEmbed.AddField("Library", "DSharpPlus, Version:" + ctx.Client.VersionString);
			discordEmbed.AddField(".NET Core Version: ", PlatformServices.Default.Application.RuntimeFramework.Version.ToString(2));
			discordEmbed.AddField("Repository", "[GitHub](https://github.com/JulianusIV/LathBot)");
			TimeSpan uptime = DateTime.Now - StartupService.Instance.StartTime;
			discordEmbed.AddField("Uptime", $"Bot has been running since {uptime}");
			await ctx.Channel.SendMessageAsync(discordEmbed.Build()).ConfigureAwait(false);
		}

		[Command("tos")]
		[Description("The bot's TOS")]
		public async Task Tos(CommandContext ctx)
		{
			string part = "";
			int index = 0;
			string result;
			using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("LathBotFront.Resources.LathBotTOS.txt"))
			using (StreamReader reader = new StreamReader(stream))
			{
				result = reader.ReadToEnd();
			}
			List<string> content = new List<string>();
			foreach (char character in result)
			{
				index++;
				if (part.Length < 4096)
				{
					if (index < result.Length)
						part += character;
					else
						content.Add(part);
				}
				else
				{
					content.Add(part);
					part = character.ToString();
				}
			}
			foreach (string cut in content)
			{
				await ctx.Channel.SendMessageAsync(new DiscordEmbedBuilder { Description = cut });
			}
		}

		[Command("privacy")]
		[Description("The bot's privacy policy")]
		public async Task Privacy(CommandContext ctx)
		{
			string part = "";
			int index = 0;
			string result;
			using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("LathBotFront.Resources.LathBotPP.txt"))
			using (StreamReader reader = new StreamReader(stream))
			{
				result = reader.ReadToEnd();
			}
			List<string> content = new List<string>();
			foreach (char character in result)
			{
				index++;
				if (part.Length < 4096)
				{
					if (index < result.Length)
						part += character;
					else
						content.Add(part);
				}
				else
				{
					content.Add(part);
					part = character.ToString();
				}
			}
			foreach (string cut in content)
			{
				await ctx.Channel.SendMessageAsync(new DiscordEmbedBuilder { Description = cut });
			}
		}

		[Command("stafftime")]
		[Description("Display timezones and current time of staff members")]
		public async Task StaffTimes(CommandContext ctx)
		{
			await ctx.Channel.TriggerTypingAsync().ConfigureAwait(false);
			DiscordEmbedBuilder discordEmbed = new DiscordEmbedBuilder
			{
				Color = new DiscordColor(64, 255, 0),
				Title = "Staff timezones",
				Description = "Keep in mind that it being daytime in the users timezone does not obligate them to being available!"
			};
			DateTime thisTime = DateTime.Now;

			ModRepository repo = new ModRepository(ReadConfig.Config.ConnectionString);
			UserRepository urepo = new UserRepository(ReadConfig.Config.ConnectionString);
			bool result = repo.GetAll(out List<Mod> list);
			if (!result)
			{
				await ctx.RespondAsync("Error reading mods from database.");
				return;
			}

			foreach (var item in list)
			{
				result = urepo.Read(item.DbId, out User entity);
				if (!result)
				{
					await ctx.RespondAsync($"Error reading a user from the database.");
					continue;
				}
				DiscordMember mod = await ctx.Guild.GetMemberAsync(entity.DcID);
				try
				{
					TimeZoneInfo modTimeZone = TimeZoneInfo.FindSystemTimeZoneById(item.Timezone);
					DateTime modTime = TimeZoneInfo.ConvertTime(thisTime, TimeZoneInfo.Local, modTimeZone);
					discordEmbed.AddField($"{mod.Username}#{mod.Discriminator}",
						modTime.ToString("yyyy-mm-dd     **HH:mm**") + "     (" + (modTimeZone.IsDaylightSavingTime(modTime) ?
						modTimeZone.DaylightName.Substring(0, 6) : modTimeZone.StandardName.Substring(0, 6)) + ")");
				}
				catch
				{
					await ctx.RespondAsync($"Error creating the embed for user {mod.DisplayName}#{mod.Discriminator} ({mod.Id}).");
					continue;
				}
			}
			await ctx.Channel.SendMessageAsync(discordEmbed).ConfigureAwait(false);
		}
	}
}

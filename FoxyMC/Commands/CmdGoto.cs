namespace FoxyMC.Commands
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public class CmdGoto : Command
    {
        public override void Help(Player p)
        {
            p.SendMessage("/goto <mapname> - Teleports yourself to a different level.");
        }

        public override void Use(Player p, string message)
        {
			try
			{
				if (message == "")
				{
					#region blank
					List<string> levels = new List<string>(Server.levels.Count);
					message = Server.mainLevel.name;
					string message2 = "";
					levels.Add(Server.mainLevel.name.ToLower());
					bool Once = false;
					foreach (Level level in Server.levels)
					{
						if (level != Server.mainLevel)
						{
							if (level.permissionvisit <= p.group.Permission)
							{
								message = message + ", " + level.name;
								levels.Add(level.name.ToLower());
							}
							else if (!Once)
							{
								Once = true;
								message2 = message2 + level.name;
							}
							else
							{
								message2 = message2 + ", " + level.name;
							}
						}
					}
					p.SendMessage("Loaded: &2" + message);
					p.SendMessage("Can't Goto: &c" + message2);
					message = "";
					FileInfo[] files = new DirectoryInfo("levels/").GetFiles("*.lvl");
					Once = false;
					foreach (FileInfo info2 in files)
					{
						if (!levels.Contains(info2.Name.Replace(".lvl", "").ToLower()))
						{
							if (!Once)
							{
								Once = true;
								message = message + info2.Name.Replace(".lvl", "");
							}
							else
							{
								message = message + ", " + info2.Name.Replace(".lvl", "");
							}
						}
					}
					p.SendMessage("Unloaded: &4" + message);
					#endregion
				}
				else
				{
					Level level = null;
					bool multi = false;
					foreach (Level l in Server.levels)
					{
						if (l.name.ToLower() == message.ToLower())
						{
							level = l;
							multi = false;
							break;
						}
						if (l.name.ToLower().Contains(message.ToLower()))
						{
							if (level == null)
							{level = l;}
							else
							{multi = true;}
						}
					}

					if (multi)
					{
						p.SendMessage("Multiple levels returned, be more specific.");
						return;
					}

					if (level != null)
					{
						if (p.level == level)
						{
							p.SendMessage("You are already in \"" + level.name + "\".");
						}
						else if (p.group.Permission < level.permissionvisit)
						{
							p.SendMessage("Your not allowed to goto " + level.name + ".");
						}
						else
						{
							p.Loading = true;
							foreach (Player player in Player.players)
							{
								if ((p.level == player.level) && (p != player))
								{
									p.SendDie(player.id);
								}
							}
							foreach (PlayerBot bot in PlayerBot.playerbots)
							{
								if (p.level == bot.level)
								{
									p.SendDie(bot.id);
								}
							}
							p.ClearBlockchange();
							p.BlockAction = 0;
							p.painting = false;
							Player.GlobalDie(p, true);
							p.level = level;
							p.SendMotd();
							p.SendMap();
							ushort x = (ushort)((0.5 + level.spawnx) * 32.0);
							ushort y = (ushort)((1 + level.spawny) * 0x20);
							ushort z = (ushort)((0.5 + level.spawnz) * 32.0);
							if (!p.hidden)
							{
								Player.GlobalSpawn(p, x, y, z, level.rotx, level.roty, true);
							}
							else
							{
								p.SendPos(0xff, x, y, z, level.rotx, level.roty);
							}
							foreach (Player player2 in Player.players)
							{
								if (((player2.level == p.level) && (p != player2)) && !player2.hidden)
								{
									p.SendSpawn(player2.id, player2.color + player2.name, player2.pos[0], player2.pos[1], player2.pos[2], player2.rot[0], player2.rot[1]);
								}
							}
							foreach (PlayerBot bot2 in PlayerBot.playerbots)
							{
								if (bot2.level == p.level)
								{
									p.SendSpawn(bot2.id, bot2.color + bot2.name, bot2.pos[0], bot2.pos[1], bot2.pos[2], bot2.rot[0], bot2.rot[1]);
								}
							}
							if (!p.hidden)
							{
								Player.GlobalChat(p, p.color + "*" + p.name + "&e went to \"" + level.name + "\".", false);
							}
							p.Loading = false;
							GC.Collect();
							GC.WaitForPendingFinalizers();
						}
					}
					else
					{ p.SendMessage("There is no level \"" + message + "\" loaded."); }
				}
			}
			catch (Exception e)
			{
				Server.s.Log(e.ToString());
			}
        }

        public override string name
        {
            get
            {
                return "goto";
            }
        }

        public override LevelPermission level
        {
            get
            {
                return (Server.guestGoto) ? LevelPermission.Guest : LevelPermission.Builder;
            }
        }
    }
}


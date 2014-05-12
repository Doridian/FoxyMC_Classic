namespace FoxyMC.Commands
{
    using System;

    public class CmdBind : Command
    {
        public override void Help(Player p)
        {
            p.SendMessage("/bind <block> [type] - Replaces block with type.");
        }

        public override void Use(Player p, string message)
        {
            if (message == "")
            {
                this.Help(p);
            }
            else if (message.Split(new char[] { ' ' }).Length > 2)
            {
                this.Help(p);
            }
            else
            {
                message = message.ToLower();
                int index = message.IndexOf(' ');
                if (index == -1)
                {
                    byte type = Block.Byte(message);
                    if (type == 0xff)
                    {
                        p.SendMessage("There is no block \"" + message + "\".");
                    }
                    else if (!Block.Placable(type))
                    {
                        p.SendMessage("You can't place " + Block.Name(type) + ".");
                    }
                    else if (p.bindings[type] == type)
                    {
                        p.SendMessage(Block.Name(type) + " isn't bound.");
                    }
                    else
                    {
                        p.bindings[type] = type;
                        p.SendMessage("Unbound " + Block.Name(type) + ".");
                    }
                }
                else
                {
                    byte num2 = Block.Byte(message.Substring(0, index));
                    if (num2 == 0xff)
                    {
                        p.SendMessage("There is no block \"" + message.Substring(0, index) + "\".");
                    }
                    else if (!Block.Placable(num2))
                    {
                        p.SendMessage("You can't bind " + Block.Name(num2) + ".");
                    }
                    else
                    {
                        byte num3 = Block.Byte(message.Substring(index + 1));
                        switch (num3)
                        {
                            case 0xff:
                                p.SendMessage("There is no block \"" + message.Substring(index + 1) + "\".");
                                return;

                            case 0:
                            case 8:
                            case 10:
                                p.SendMessage("You can't bind " + Block.Name(num3) + ".");
                                return;

                            default:
                                if (Block.Placable(num3))
                                {
                                    p.SendMessage(Block.Name(num3) + " isn't a special block.");
                                    return;
                                }
                                if (p.bindings[num2] == num3)
                                {
                                    p.SendMessage(Block.Name(num2) + " is already bound to " + Block.Name(num3) + ".");
                                    return;
                                }
                                p.bindings[num2] = num3;
                                message = Block.Name(num2) + " bound to " + Block.Name(num3) + ".";
                                for (byte i = 0; i < 0x80; i = (byte) (i + 1))
                                {
                                    byte num5 = i;
                                    if (((p.bindings[i] == num3) && (i != num2)) && Block.Placable(num5))
                                    {
                                        message = message + " Unbound " + Block.Name(num5) + ".";
                                        p.bindings[i] = i;
                                        break;
                                    }
                                }
                                break;
                        }
                        p.SendMessage(message);
                    }
                }
            }
        }

        public override string name
        {
            get
            {
                return "bind";
            }
        }

        public override LevelPermission level
        {
            get
            {
                return LevelPermission.Operator;
            }
        }
    }
}


using System.Collections.Generic;
using System.Linq;
using MCGalaxy.Events.PlayerEvents;
using MCGalaxy.Events.ServerEvents;
using MCGalaxy.Network;

namespace MCGalaxy {
    public sealed partial class MCGalaxyToolgunPlugin : Plugin {
        public override string name { get { return "Toolgun"; } }
        public override void Load(bool startup) {
            Debug("Load");

            OnPlayerDisconnectEvent.Register(OnPlayerDisconnect, Priority.Low);
            OnPluginMessageReceivedEvent.Register(OnPluginMessageReceived, Priority.Low);
            OnBlockChangedEvent.Register(OnBlockChanged, Priority.Low);
        }
        public override void Unload(bool shutdown) {
            Debug("Unload");

            OnBlockChangedEvent.Unregister(OnBlockChanged);
            OnPluginMessageReceivedEvent.Unregister(OnPluginMessageReceived);
            OnPlayerDisconnectEvent.Unregister(OnPlayerDisconnect);
        }

        private static byte CHANNEL = 71;

        private static HashSet<Player> HavePlugin = new();

        public static void OnPluginMessageReceived(Player sender, byte channel, byte[] data) {
            if (channel != CHANNEL) { return; }

            Debug(
                "OnPluginMessageReceived {0}",
                sender.truename
            );
            HavePlugin.Add(sender);
        }

        public static void OnPlayerDisconnect(Player p, string reason) {
            if (HavePlugin.Contains(p)) {
                Debug(
                    "OnPlayerDisconnect {0}",
                    p.truename
                );
                HavePlugin.Remove(p);
            }
        }


        public static void OnBlockChanged(Player p, ushort x, ushort y, ushort z, ChangeResult result) {
            if (result != ChangeResult.Modified) { return; }

            Level level = p.level;
            if (level.GetBlock(x, y, z) == Block.Air) { return; }

            var players = PlayerInfo.Online.Items
                .Where((other) => other != p)
                .Where((other) => other.Supports(CpeExt.PluginMessages))
                .Where((other) => HavePlugin.Contains(other))
                .Where((other) => other.level == level)
                .ToArray();

            byte[] data = new byte[Packet.PluginMessageDataLength];

            int i = 0;
            data[i += 1] = p.id;
            NetUtils.WriteU16(x, data, i); i += 2;
            NetUtils.WriteU16(y, data, i); i += 2;
            NetUtils.WriteU16(z, data, i); i += 2;

            foreach (var other in players) {
                other.Send(Packet.PluginMessage(CHANNEL, data));
            }
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using MCGalaxy.Events.PlayerEvents;
using MCGalaxy.Events.ServerEvents;
using MCGalaxy.Network;

namespace MCGalaxy {
    public sealed partial class MCGalaxyToolgunPlugin : Plugin {
        public override string name { get { return "Toolgun"; } }
        public override void Load(bool startup) {
            Logger.Log(LogType.Debug, "Toolgun Load");

            OnPlayerDisconnectEvent.Register(OnPlayerDisconnect, Priority.Low);
            OnPluginMessageReceivedEvent.Register(OnPluginMessageReceived, Priority.Low);
            OnBlockChangedEvent.Register(OnBlockChanged, Priority.Low);
        }
        public override void Unload(bool shutdown) {
            Logger.Log(LogType.Debug, "Toolgun Unload");

            OnBlockChangedEvent.Unregister(OnBlockChanged);
            OnPluginMessageReceivedEvent.Unregister(OnPluginMessageReceived);
            OnPlayerDisconnectEvent.Unregister(OnPlayerDisconnect);
        }

        private static byte CHANNEL = 71;

        private static HashSet<Player> HavePlugin = new();

        public static void OnPluginMessageReceived(Player sender, byte channel, byte[] data) {
            if (channel != CHANNEL) { return; }
            HavePlugin.Add(sender);
        }

        public static void OnPlayerDisconnect(Player p, string reason) {
            Debug(
                "OnPlayerDisconnect {0}",
                p.truename
            );
            HavePlugin.Remove(p);
        }


        public static void OnBlockChanged(Player p, ushort x, ushort y, ushort z, ChangeResult result) {
            if (result != ChangeResult.Modified) { return; }

            Level level = p.level;

            var players = PlayerInfo.Online.Items
                .Where((p) => p.Supports(CpeExt.PluginMessages))
                .Where((p) => HavePlugin.Contains(p))
                .Where((p) => p.level == level)
                .ToArray();

            byte[] data = new byte[Packet.PluginMessageDataLength];

            int i = 0;
            data[i += 1] = p.id;
            NetUtils.WriteU16(x, data, i); i += 2;
            NetUtils.WriteU16(y, data, i); i += 2;
            NetUtils.WriteU16(z, data, i); i += 2;

            p.Send(Packet.PluginMessage(CHANNEL, data));
        }
    }
}

namespace MCGalaxy {
    public sealed partial class MCGalaxyToolgunPlugin {
        private static readonly bool debug = true;
        private static void Debug(string format, params object[] args) {
            if (!debug) { return; }
            Logger.Log(LogType.Debug, format, args);
        }
        private static void Debug(string format) {
            if (!debug) { return; }
            Logger.Log(LogType.Debug, format);
        }


        private static void Warn(string format, params object[] args) {
            Logger.Log(LogType.Warning, format, args);
        }
        private static void Warn(string format) {
            Logger.Log(LogType.Warning, format);
        }
    }
}

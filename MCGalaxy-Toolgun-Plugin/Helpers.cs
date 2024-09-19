namespace MCGalaxy {
    public sealed partial class MCGalaxyToolgunPlugin {
#if DEBUG
        private static readonly bool debug = true;
#else
        private static readonly bool debug = false;
#endif
        private static void Debug(string format, params object[] args) {
            if (!debug) { return; }
            Logger.Log(LogType.Debug, "ToolgunPlugin: " + format, args);
        }
        private static void Debug(string format) {
            if (!debug) { return; }
            Logger.Log(LogType.Debug, "ToolgunPlugin: " + format);
        }
    }
}

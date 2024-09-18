namespace MCGalaxy {
    public sealed partial class MCGalaxyToolgunPlugin : Plugin {
        public override string name { get { return "Toolgun"; } }
        public override void Load(bool startup) {
            Logger.Log(LogType.Debug, "Toolgun Load");
        }
        public override void Unload(bool shutdown) {
            Logger.Log(LogType.Debug, "Toolgun Unload");
        }
    }
}

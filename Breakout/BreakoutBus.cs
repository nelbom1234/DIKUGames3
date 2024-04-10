using DIKUArcade.Events;
namespace Breakout {
    public static class BreakoutBus {
        private static GameEventBus eventBus;
        public static GameEventBus GetBus() {
            return BreakoutBus.eventBus ?? (BreakoutBus.eventBus =
                                         new GameEventBus());
        }
        //for testing purposes as we can't initialize the bus several times
        // and it breaks otherwise if we just don't initialize
        public static void ResetBus() {
            BreakoutBus.eventBus = new GameEventBus();
        }
    }
}
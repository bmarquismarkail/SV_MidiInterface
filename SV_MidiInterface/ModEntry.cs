using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Commons.Music.Midi;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using SV_MidiInterface.Framework;
using SV_MidiInterface.Framework.Manager;

namespace SV_MidiInterface
{
    public class ModEntry : Mod
    {
        MidiManager midiManager;

        public override void Entry(IModHelper helper)
        {
            Helper.Events.GameLoop.GameLaunched += OnGameLaunched;
            Helper.Events.GameLoop.UpdateTicking += PollConnection;

        }

        private void PollConnection(object sender, UpdateTickingEventArgs e)
        {
            Task task1 = midiManager.Poll();
        }

        public override object GetApi()
        {
            return new MidiAPI();
        }


        private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
        {
            midiManager = new MidiManager(this.Monitor);

        }
    }
}

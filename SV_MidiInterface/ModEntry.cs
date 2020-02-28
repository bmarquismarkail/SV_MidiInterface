using System;
using StardewModdingAPI;
using Commons.Music.Midi;
using StardewModdingAPI.Events;
using System.Linq;

namespace SV_MidiInterface
{
    public class ModEntry : Mod
    {
        public IMidiAccess2 midi;
        public IMidiInput inDevice;
        public byte prevType, prevValue;

        public override void Entry(IModHelper helper)
        {
            Helper.Events.GameLoop.GameLaunched += OnGameLaunched;
            Helper.Events.GameLoop.UpdateTicked += PollMidi;

        }

        private void PollMidi(object sender, UpdateTickedEventArgs e)
        {
            inDevice.MessageReceived += (obj, ev) => {
                if (prevType != ev.Data[0])
                    this.Monitor.Log($"State Changed: {ev.Data[0]}", LogLevel.Debug);

                this.Monitor.Log($"Value: {ev.Data[1]}", LogLevel.Debug);
                prevType = ev.Data[0];
            };
        }

        private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
        {
            this.Monitor.Log("Initializing MIDI Framework", LogLevel.Info);
            midi = (IMidiAccess2)MidiAccessManager.Default;
            var numdecices = midi.Inputs.Count();
            this.Monitor.Log($"Midi Framework Initialized. Found {numdecices} device{ ( (numdecices ==0 || numdecices > 1)? ("s"):("") ) }", LogLevel.Info);
            inDevice = midi.OpenInputAsync(midi.Inputs.Last().Id).Result;
            this.Monitor.Log($"Using {inDevice.Details.Name}", LogLevel.Debug);
        }
    }
}

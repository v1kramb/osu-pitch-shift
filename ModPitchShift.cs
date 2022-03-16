// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Framework.Audio;
using osu.Framework.Audio.Track;
using osu.Framework.Bindables;
using osu.Game.Configuration;
using osu.Framework.Graphics.Audio;
using osu.Framework.Graphics.Sprites;

namespace osu.Game.Rulesets.Mods
{
    public abstract class ModPitchShift : Mod, IApplicableToAudio
    {
        public override string Name => "Pitch Shift";
        public override string Acronym => "PS";
        public override IconUsage? Icon => FontAwesome.Solid.SlidersH;
        public override ModType Type => ModType.Conversion;
        public override string Description => "Change the pitch of your song!";
        public override double ScoreMultiplier => 1;

        public override Type[] IncompatibleMods => new[] { typeof(ModNightcore), typeof(ModDaycore) };

        private readonly BindableNumber<double> tempoAdjust = new BindableDouble(1);
        private readonly BindableNumber<double> freqAdjust = new BindableDouble(1);

        [SettingSource("Pitch", "The pitch of the song in semitones")]
        public BindableNumber<double> PitchChange { get; } = new BindableDouble // in semitones
        {
            MinValue = -12.0,  // -12 semitones = pitch w/ 0.5x tempo
            MaxValue = 12.0,  // 12 semitones = pitch w/ 2x tempo
            Default = 0.0,
            Value = 0.0,
            Precision = 0.01,
        };
        
        public ModPitchShift()
        {
            PitchChange.BindValueChanged(val =>
            {
                freqAdjust.Value = ConvertSemitoneToRelativeBPM(val.NewValue);
                tempoAdjust.Value = 1.0 / freqAdjust.Value;
            }, true);
        }

        public virtual void ApplyToTrack(ITrack track)
        {
            track.AddAdjustment(AdjustableProperty.Frequency, freqAdjust);
            track.AddAdjustment(AdjustableProperty.Tempo, tempoAdjust);
        }

        public virtual void ApplyToSample(DrawableSample sample)
        {
            sample.AddAdjustment(AdjustableProperty.Frequency, freqAdjust);
            sample.AddAdjustment(AdjustableProperty.Tempo, tempoAdjust);
        }

        private double ConvertSemitoneToRelativeBPM(double semitone)
        {
            // tempo ratio (i.e. new BPM / base BPM) = 2 ^ (semitone / 12)
            return Math.Pow(2, semitone / 12);
        }
    }
}

// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Game.Beatmaps;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osuTK.Graphics;

namespace osu.Game.Screens.Menu
{
    public class SongTracker : FillFlowContainer
    {
        private Bindable<bool> showUnicode { get; set; }
        private IBindable<WorkingBeatmap> beatmapBindable { get; set; }

        private SpriteText artist;
        private SpriteText title;

        [BackgroundDependencyLoader]
        private void load(FrameworkConfigManager frameworkConfig, IBindable<WorkingBeatmap> beatmapBindable)
        {
            Padding = new MarginPadding()
            {
                Horizontal = 15,
                Vertical = 5,
            };

            Direction = FillDirection.Vertical;

            AddRange(new[]
            {
                title = new OsuSpriteText
                {
                    Origin = Anchor.TopRight,
                    Anchor = Anchor.TopRight,
                    Font = OsuFont.GetFont(size: 25, italics: true),
                    Colour = Color4.White,
                    Margin = new MarginPadding()
                    {
                        Bottom = 3
                    }
                },
                artist = new OsuSpriteText
                {
                    Origin = Anchor.TopRight,
                    Anchor = Anchor.TopRight,
                    Font = OsuFont.GetFont(size: 15, weight: FontWeight.Bold, italics: true),
                    Colour = Color4.White,
                },
            });

            this.beatmapBindable = beatmapBindable;
            showUnicode = frameworkConfig.GetBindable<bool>(FrameworkSetting.ShowUnicode);

            this.beatmapBindable.BindValueChanged(beatmapChanged, true);
            showUnicode.BindValueChanged((e) => updateSong(this.beatmapBindable.Value, false));
        }

        private void beatmapChanged(ValueChangedEvent<WorkingBeatmap> beatmap) => updateSong(beatmap.NewValue);

        private void updateSong(WorkingBeatmap beatmap, bool animate = true)
        {
            var metadata = beatmap.BeatmapSetInfo.Metadata;
            title.Text = showUnicode.Value && !string.IsNullOrWhiteSpace(metadata.TitleUnicode) ? metadata.TitleUnicode : metadata.Title;
            artist.Text = showUnicode.Value && !string.IsNullOrWhiteSpace(metadata.ArtistUnicode) ? metadata.ArtistUnicode : metadata.Artist;

            if (animate)
            {
                this.FadeIn(100, Easing.OutCirc);
                this.Delay(3000).FadeOut(350, Easing.InSine);
            }
        }
    }
}

using System;
using BannerlordTwitch;
using BannerlordTwitch.Localization;
using BannerlordTwitch.Rewards;
using JetBrains.Annotations;

namespace BLTAdoptAHero
{
    [LocDisplayName("autosummonchangeside"), UsedImplicitly]
    internal class ToggleAutoSummonSide : ActionHandlerBase
    {
        protected override Type ConfigType => typeof(object);
        protected override void ExecuteInternal(ReplyContext ctx, object _, Action<string> ok, Action<string> fail)
        {
            var h = BLTAdoptAHeroCampaignBehavior.Current.GetAdoptedHero(ctx.UserName);
            if (h == null) { fail("Герой не найден"); return; }
            var st = Settings.Load();
            var cfg = GlobalCommonConfig.Get(st);
            var k = h.Name.ToString();
            var prev = cfg.AutoSummonSide.TryGetValue(k, out var s) ? s : true;
            var next = !prev;
            cfg.AutoSummonSide[k] = next;
            Settings.Save(st);
            BLTAdoptAHeroModule.CommonConfig.AutoSummonSide = cfg.AutoSummonSide;
            ok($"Сторона для {k} теперь {(next ? "player" : "enemy")}");
        }
    }
}

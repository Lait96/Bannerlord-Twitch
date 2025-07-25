using System;
using System.Linq;
using BannerlordTwitch;
using BannerlordTwitch.Localization;
using JetBrains.Annotations;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

namespace BLTAdoptAHero
{
    [LocDisplayName("unequip"), UsedImplicitly]
    internal class UnequipItemHandler : HeroActionHandlerBase
    {
        protected override Type ConfigType => typeof(object);
        protected override void ExecuteInternal(Hero h, ReplyContext ctx, object _, Action<string> ok, Action<string> fail)
        {
            var a = ctx.Args.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (a.Length != 1 || !int.TryParse(a[0], out var si) || si < 1)
            { fail("Использование: unequip <номер_слота>"); return; }

            var slots = BLTAdoptAHeroCampaignBehavior.Current.GetEquipmentClass(h).IndexedSlots.ToList();
            if (si > slots.Count) { fail("Неверный слот"); return; }

            var idx = slots[si - 1].Item1;
            var el = h.BattleEquipment[idx];
            if (el.Item == null) { fail("В слоте нет предмета"); return; }

            h.BattleEquipment[idx] = EquipmentElement.Invalid;
            ok($"Слот {si} очищен");
        }
    }
}

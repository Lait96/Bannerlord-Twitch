using System;
using System.Linq;
using BannerlordTwitch;
using BannerlordTwitch.Localization;
using JetBrains.Annotations;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using BannerlordTwitch.Util;
using BannerlordTwitch.Helpers;

namespace BLTAdoptAHero.Actions
{
    [LocDisplayName("equip"), UsedImplicitly]
    internal class EquipItemHandler : HeroActionHandlerBase
    {
        protected override Type ConfigType => typeof(object);

        protected override void ExecuteInternal(Hero h, ReplyContext ctx, object _, Action<string> ok, Action<string> fail)
        {
            var a = ctx.Args.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (a.Length != 2 || !int.TryParse(a[0], out var si) || si < 1
                || !int.TryParse(a[1], out var ii) || ii < 1)
            {
                fail("Использование: equip <номер_слота> <номер_предмета>");
                return;
            }

            var slots = BLTAdoptAHeroCampaignBehavior.Current
                .GetEquipmentClass(h)
                .IndexedSlots
                .ToList();
            if (si > slots.Count)
            {
                fail($"Неверный слот, должно быть от 1 до {slots.Count}");
                return;
            }

            var inv = BLTAdoptAHeroCampaignBehavior.Current.GetCustomItems(h);
            if (ii > inv.Count)
            {
                fail($"Неверный предмет, должно быть от 1 до {inv.Count}");
                return;
            }

            var (idx, tp) = slots[si - 1];
            var el = inv[ii - 1];
            // проверяем, что тип предмета совпадает с типом слота
            if (!el.Item.IsEquipmentType(tp))
            {
                fail("Нельзя надеть этот предмет в этот слот");
                return;
            }

            BLTAdoptAHeroCampaignBehavior.Current.AddCustomItem(h, el);
            h.BattleEquipment[idx] = el;
            ok($"Слот {si}: {el.GetModifiedItemName()}");
        }
    }
}

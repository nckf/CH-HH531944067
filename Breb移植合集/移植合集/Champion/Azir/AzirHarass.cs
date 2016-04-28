﻿using ClipperLib;
using Color = System.Drawing.Color;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK;
using EloBuddy;
using Font = SharpDX.Direct3D9.Font;
using LeagueSharp.Common.Data;
using LeagueSharp.Common;
using SharpDX.Direct3D9;
using SharpDX;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Security.AccessControl;
using System;
using System.Speech.Synthesis;

namespace HeavenStrikeAzir
{
    public static class AzirHarass
    {
        public static AIHeroClient Player { get { return ObjectManager.Player; } }
        public static void Initialize()
        {
            Game.OnUpdate += Game_OnUpdate;
        }

        private static void Game_OnUpdate(EventArgs args)
        {
            if (!Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
                return;
            if (Program._q.IsReady() && Orbwalker.CanMove && Program.qharass && (!Program.donotqharass || (!Soldiers.enemies.Any() && !Soldiers.splashautoattackchampions.Any())))
            {
                var target = TargetSelector.GetTarget(Program._q.Range, DamageType.Magical);
                foreach (var obj in Soldiers.soldier)
                {
                    Program._q.SetSkillshot(0.0f, 65f, 1500f, false, SkillshotType.SkillshotLine, obj.Position, Player.Position);
                    Program._q.Cast(target);
                }
            }
            if (Program._w.IsReady() && Orbwalker.CanMove && !Soldiers.enemies.Any() && Program.wharass)
            {
                var target = TargetSelector.GetTarget(Program._w.Range + 300, DamageType.Magical);
                if (target.IsValidTarget() && !target.IsZombie && !Soldiers.enemies.Contains(target))
                {
                    var x = Player.Distance(target.Position) > Program._w.Range ? Player.Position.LSExtend(target.Position, Program._w.Range)
                        : target.Position;
                    Program._w.Cast(x);
                }
            }
            if (Program._w.IsReady() && Orbwalker.CanMove && !Soldiers.enemies.Any() && !Soldiers.soldier.Any() && Program.wharass && Program.Qisready())
            {
                var target = TargetSelector.GetTarget(Program._w.Range + 300, DamageType.Magical);
                if (target == null || !target.IsValidTarget() || target.IsZombie)
                {
                    var tar = HeroManager.Enemies.Where(x => x.IsValidTarget(Program._q.Range) && !x.IsZombie).OrderByDescending(x => Player.Distance(x.Position)).LastOrDefault();
                    if (tar.IsValidTarget() && !tar.IsZombie)
                    {
                        var x = Player.Distance(tar.Position) > Program._w.Range ? Player.Position.LSExtend(tar.Position, Program._w.Range)
                            : tar.Position;
                        Program._w.Cast(x);
                    }
                }
            }
        }
    }
}

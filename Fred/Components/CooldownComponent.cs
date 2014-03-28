using Artemis.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fred.Components
{
    class CooldownComponent : IComponent
    {
        public double CurrentAttackCooldown { get; set; }
        public double MaxAttackCooldown { get; set; }
        public double CurrentBuildCooldown { get; set; }
        public double MaxBuildCooldown { get; set; }
        public double CurrentBounceCooldown { get; set; }
        public double MaxBounceCooldown { get; set; }
        public double CurrentMenuSelectCooldown { get; set; }
        public double MaxMenuSelectCooldown { get; set; }
        public bool IsAttackReady
        {
            get
            {
                return this.CurrentAttackCooldown <= 0;
            }
        }
        public bool IsBuildReady
        {
            get
            {
                return this.CurrentBuildCooldown <= 0;
            }
        }
        public bool IsBounceReady
        {
            get
            {
                return this.CurrentBounceCooldown <= 0;
            }
        }
        public bool IsMenuSelectReady
        {
            get
            {
                return this.CurrentMenuSelectCooldown <= 0;
            }
        }


        public CooldownComponent()
        {
            CurrentAttackCooldown = MaxAttackCooldown = 500;
            CurrentBuildCooldown = MaxBuildCooldown = 500;
            CurrentBounceCooldown = MaxBounceCooldown = 50;
            CurrentMenuSelectCooldown = MaxMenuSelectCooldown = 150;

        }

        public CooldownComponent(double attack, double build, double bounce, double menu)
        {
            CurrentAttackCooldown = CurrentAttackCooldown = attack;
            CurrentBuildCooldown = MaxBuildCooldown = build;
            CurrentBounceCooldown = MaxBounceCooldown = bounce;
            CurrentMenuSelectCooldown = MaxMenuSelectCooldown = menu;
        }
        public void ResetCooldowns()
        {
            CurrentAttackCooldown = MaxAttackCooldown;
            CurrentBuildCooldown = MaxBuildCooldown;
            CurrentBounceCooldown = MaxBounceCooldown;
            CurrentMenuSelectCooldown = MaxMenuSelectCooldown;
        }
        public void CancelCooldowns()
        {
            CurrentAttackCooldown = 0;
            CurrentBuildCooldown = 0;
            CurrentBounceCooldown = 0;
            CurrentMenuSelectCooldown = 0;
        }
        public void ResetAttackCooldown()
        {
            CurrentAttackCooldown = MaxAttackCooldown;
        }
        public void ResetBuildCooldown()
        {
            CurrentBuildCooldown = MaxBuildCooldown;
        }
        public void ResetBounceCooldown()
        {
            CurrentBounceCooldown = MaxBounceCooldown;
        }
        public void ResetMenuSelectCooldown()
        {
            CurrentMenuSelectCooldown = MaxMenuSelectCooldown;
        }
    }
}

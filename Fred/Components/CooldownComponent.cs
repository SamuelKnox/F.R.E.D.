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


        public CooldownComponent()
        {
            CurrentAttackCooldown = MaxAttackCooldown = 3000;
            CurrentBuildCooldown = MaxBuildCooldown = 3000;
            CurrentBounceCooldown = MaxBounceCooldown = 50;
        }

        public CooldownComponent(double attack, double build, double bounce)
        {
            CurrentAttackCooldown = CurrentAttackCooldown = attack;
            CurrentBuildCooldown = MaxBuildCooldown = build;
            CurrentBounceCooldown = MaxBounceCooldown = bounce;
        }
        public void ResetCooldowns()
        {
            CurrentAttackCooldown = MaxAttackCooldown;
            CurrentBuildCooldown = MaxBuildCooldown;
            CurrentBounceCooldown = MaxBounceCooldown;
        }
        public void CancelCooldowns()
        {
            CurrentAttackCooldown = 0;
            CurrentBuildCooldown = 0;
            CurrentBounceCooldown = 0;
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
    }
}

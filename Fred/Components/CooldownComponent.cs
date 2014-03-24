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
        }

        public CooldownComponent(double attack, double build)
        {
            CurrentAttackCooldown = CurrentAttackCooldown = attack;
            CurrentBuildCooldown = MaxBuildCooldown = build;
        }
        public void ResetCooldowns()
        {
            CurrentAttackCooldown = MaxAttackCooldown;
            CurrentBuildCooldown = MaxBuildCooldown;
        }
        public void CancelCooldowns()
        {
            CurrentAttackCooldown = 0;
            CurrentBuildCooldown = 0;
        }
        public void ResetAttackCooldown()
        {
            CurrentAttackCooldown = MaxAttackCooldown;
        }
        public void ResetBuildCooldown()
        {
            CurrentBuildCooldown = MaxBuildCooldown;
        }
    }
}

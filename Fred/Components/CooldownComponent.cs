using Artemis.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fred.Components
{
    class CooldownComponent : IComponent
    {
        public double CurrentBombCooldown { get; set; }
        public double MaxBombCooldown { get; set; }
        public double CurrentBuildCooldown { get; set; }
        public double MaxBuildCooldown { get; set; }
        public double CurrentBounceCooldown { get; set; }
        public double MaxBounceCooldown { get; set; }
        public double CurrentMenuSelectCooldown { get; set; }
        public double MaxMenuSelectCooldown { get; set; }
		public double CurrentSpriteCooldown { get; set; }
		public double MaxSpriteCooldown { get; set; }

        public bool IsBombReady
        {
            get
            {
                return this.CurrentBombCooldown <= 0;
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
			
		public bool IsSpriteReady {
			get {
				return this.CurrentSpriteCooldown <= 0;
			}
		}


		public CooldownComponent()
		{
			CurrentBombCooldown = MaxBombCooldown = 2000;
			CurrentBuildCooldown = MaxBuildCooldown = 2000;
			CurrentBounceCooldown = MaxBounceCooldown = 50;
			CurrentMenuSelectCooldown = MaxMenuSelectCooldown = 150;
			CurrentSpriteCooldown = MaxSpriteCooldown = 170;
		}

		public CooldownComponent(double bomb, double build, double bounce, double menu, double sprite)
		{
			CurrentBombCooldown = CurrentBombCooldown = bomb;
			CurrentBuildCooldown = MaxBuildCooldown = build;
			CurrentBounceCooldown = MaxBounceCooldown = bounce;
			CurrentMenuSelectCooldown = MaxMenuSelectCooldown = menu;
			CurrentSpriteCooldown = MaxSpriteCooldown = sprite;
		}
		public void ResetCooldowns()
		{
			CurrentBombCooldown = MaxBombCooldown;
			CurrentBuildCooldown = MaxBuildCooldown;
			CurrentBounceCooldown = MaxBounceCooldown;
			CurrentMenuSelectCooldown = MaxMenuSelectCooldown;
			CurrentSpriteCooldown = MaxSpriteCooldown;
		}
		public void CancelCooldowns()
		{
			CurrentBombCooldown = 0;
			CurrentBuildCooldown = 0;
			CurrentBounceCooldown = 0;
			CurrentMenuSelectCooldown = 0;
			CurrentSpriteCooldown = 0;
		}
        public void ResetBombCooldown()
        {
            CurrentBombCooldown = MaxBombCooldown;
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
		public void ResetSpriteCooldown(){
			CurrentSpriteCooldown = MaxSpriteCooldown;
		}
    }
}

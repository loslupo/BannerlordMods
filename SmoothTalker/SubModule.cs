using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.CampaignSystem.Conversation.Persuasion;
using TaleWorlds.CampaignSystem;
namespace SmoothTalker
{
    public class SubModule : MBSubModuleBase
    {
        //protected override void OnSubModuleLoad()
        //{
        //}
        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
            base.OnGameStart(game, gameStarterObject);
            AddModels(gameStarterObject as CampaignGameStarter);
        }

        private void AddModels(CampaignGameStarter gameStarter)
        {
            try
            {
                if (gameStarter != null)
                {
                    gameStarter.AddModel(new MyPersuation());
                }

            } catch
            {

            }
        }
        public class MyPersuation : TaleWorlds.CampaignSystem.SandBox.GameComponents.DefaultPersuasionModel
        {

            private string LastHero = "";
            public override void GetChances(PersuasionOptionArgs optionArgs, out float successChance, out float critSuccessChance, out float critFailChance, out float failChance, float difficultyMultiplier)
            {
                base.GetChances(optionArgs, out successChance, out critSuccessChance, out critFailChance, out failChance, difficultyMultiplier);

                int iRelation = 0;
                bool bMessage = false;
                try
                {
                    if (Hero.OneToOneConversationHero != null)
                    {
                        iRelation = CharacterRelationManager.GetHeroRelation(Hero.MainHero, Hero.OneToOneConversationHero);
                        if (LastHero != Hero.OneToOneConversationHero.StringId)
                        {
                            LastHero = Hero.OneToOneConversationHero.StringId;
                            bMessage = true;
                        }
                    }
                } catch
                {
                    InformationManager.DisplayMessage(new InformationMessage("Smoothtalk crashed on getting hero info, please report!"));
                }
                try {
                     // Get relation 
                    if (iRelation >= 10)
                    {
                        if (bMessage)
                        {
                            InformationManager.DisplayMessage(new InformationMessage("SmoothTalker activated!"));
                        }
                        if (successChance < 0.50F)
                        {
                            successChance = 0.50F;
                        }
                        if (failChance > 0.25F)
                        {
                            failChance = 0.25F;
                        }
                        critFailChance = 0F;
                        if (critSuccessChance < 0.5F)
                        {
                            critSuccessChance = 0.5F;
                        }
                    }
                    else if (bMessage)
                    {
                        InformationManager.DisplayMessage(new InformationMessage("Can't SmoothTalk enemies or with a low relation!"));
                    }
                } catch
                {

                }

            }
        }
    }
}
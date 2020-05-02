using HarmonyLib;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.CampaignSystem.Conversation.Persuasion;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.GameComponents;
using TaleWorlds.CampaignSystem.GameMenus;
using System;

namespace TheMessenger
{
    public class SubModule : MBSubModuleBase
    {
        //delegate void OnInitDelegate(MenuCallbackArgs args);
        //protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        //{
        //    base.OnGameStart(game, gameStarterObject);
        //    AddModels(gameStarterObject as CampaignGameStarter);
        //}
        public override void OnGameLoaded(Game game, object initializerObject)
        {
            base.OnGameLoaded(game, initializerObject);
            try
            {
                CampaignEvents.WarDeclared.AddNonSerializedListener(this, new Action<IFaction, IFaction>(OnDeclareWar));

            }
            catch
            {

            }

        }
        // Action method fired once two empires declare war
        Hero enemyLeader = null;
        IFaction enemyFaction;
        public void OnDeclareWar(IFaction faction1, IFaction faction2)
        {
            enemyLeader = null;
            enemyFaction = null;
            if (Hero.MainHero.IsFactionLeader)
            {
                if (faction1.Leader.Id == Hero.MainHero.Id)
                {
                    enemyLeader = faction2.Leader;
                    enemyFaction = faction2;
                }
                else if (faction2.Leader.Id == Hero.MainHero.Id)
                {
                    enemyLeader = faction1.Leader;
                    enemyFaction = faction1;
                }
                if (enemyLeader != null)
                {
                    Action StartTalk = StartConversation;
                    Action NoTalk = NoConversation;
                    InformationManager.ShowInquiry(new InquiryData($"Declaration of War by {enemyFaction.Name}", $"Start conversation with {enemyLeader.Name}?", true, false, "Yes", "No", StartTalk, NoTalk, ""), true);
                }
            }
            //InformationManager.ShowInquiry(new InquiryData("Declaration of War", display, true, false, "Ok", "Close", null, null, ""), STALibrary.Instance.STAConfiguration.PauseGameOnPopup);
            //      InformationManager.DisplayMessage(new InformationMessage("STALibrary: " + display, new Color(1.0f, 0.0f, 0.0f)));
        }
        public void NoConversation()
        {

        }
        public void StartConversation()
        {
            if (enemyLeader != null)
            {
                //ConversationHelper.HeroAddressesPlayer(enemyLeader);
                //TaleWorlds.CampaignSystem.MobileParty
                //, TaleWorlds.Core.IAgent, TaleWorlds.Core.IAgent)
                Agent AgentOther = new Agent();
                AgentOther. = enemyLeader;

                ConversationManager.SetupAndStartMapConversation(MobileParty.MainParty, Agent.Main, Agent.);

                //ConversationManager.SetupAndStartMissionConversation(Agent.Main, TaleWorlds.Core.IAgent, trie);
            }
        }
    }
}
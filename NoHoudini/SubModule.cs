using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.CampaignSystem.Conversation.Persuasion;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.GameComponents;
using TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors;
using System;
using TaleWorlds.CampaignSystem.Actions;

namespace NoHoudini
{
    public class SubModule : MBSubModuleBase
    {
        bool bReportedError = false;
        int iReportedActive = -1;
        int iWatchForNrDays = 180;
        int iMinimalGold = 10000;
        public override bool DoLoading(Game game)
        {
            Action<Hero> originalDailyHeroTick = null;
            try
            {
                if (Campaign.Current != null)
                {
                    PrisonerEscapeCampaignBehavior EscapeBehaviour = Campaign.Current.GetCampaignBehavior<PrisonerEscapeCampaignBehavior>();
                    if (EscapeBehaviour != null)
                    {
                        originalDailyHeroTick = new Action<Hero>(EscapeBehaviour.DailyHeroTick);
                        if (CampaignEvents.DailyTickHeroEvent != null)
                        {
                            CampaignEvents.DailyTickHeroEvent.ClearListeners(EscapeBehaviour);
                            CampaignEvents.DailyTickHeroEvent.AddNonSerializedListener(EscapeBehaviour, hero => CheckEscape(originalDailyHeroTick, hero));
                        }
                    }
                }
            }
            catch
            {
                if (!bReportedError)
                {
                    InformationManager.DisplayMessage(new InformationMessage("NoHoudini crashed on load!, please report!"));
                    bReportedError = true;
                }
            }
            return base.DoLoading(game);
        }
        private void CheckEscape(Action<Hero> originalDailyHeroTick, Hero prisoner)
        {
            try
            {
                if (prisoner.IsPrisoner && prisoner.PartyBelongedToAsPrisoner?.MapFaction != null)
                {
                    // For the first two seasons your prisoner, if you are at war with the prisoner, you have plenty of food, plenty of money (10.000) and if you have more health soldiers then prisoners. Your prisoners go nowhere.
                    if (prisoner.PartyBelongedToAsPrisoner.MapFaction == Hero.MainHero.PartyBelongedTo.Party.MapFaction) // Only my prisoners
                    {
                        
                        if (prisoner.CaptivityStartTime.ElapsedDaysUntilNow < iWatchForNrDays)
                        {
                            if (Hero.MainHero.Gold > iMinimalGold && prisoner.MapFaction.IsAtWarWith(Hero.MainHero.PartyBelongedTo.Party.MapFaction) && !prisoner.PartyBelongedToAsPrisoner.IsStarving && prisoner.PartyBelongedToAsPrisoner.NumberOfHealthyMembers > prisoner.PartyBelongedToAsPrisoner.NumberOfPrisoners && prisoner.PartyBelongedToAsPrisoner.NumberOfMenWithHorse > 0)
                            {
                                if (iReportedActive != CampaignTime.Now.GetDayOfYear)
                                {
                                    int iDaysLeft = iWatchForNrDays - Convert.ToInt32(prisoner.CaptivityStartTime.ElapsedDaysUntilNow);
                                    InformationManager.DisplayMessage(new InformationMessage($"Prisoner {prisoner.Name} has double guards for another {iDaysLeft} days to prevent escaping."));
                                    iReportedActive = CampaignTime.Now.GetDayOfYear;
                                }
                                return;
                            }
                        }
                    }
                }

            }
            catch
            {
                if (!bReportedError)
                {
                    InformationManager.DisplayMessage(new InformationMessage("NoHoudini crashed!, please report!"));
                    bReportedError = true;
                }
            }
            if (originalDailyHeroTick != null)
            {
                originalDailyHeroTick(prisoner);
            }
        }

    }
}
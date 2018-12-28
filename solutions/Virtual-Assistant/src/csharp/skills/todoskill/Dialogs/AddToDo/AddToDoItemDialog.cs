﻿using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Solutions.Skills;

namespace ToDoSkill
{
    public class AddToDoItemDialog : ToDoSkillDialog
    {
        public AddToDoItemDialog(
            ISkillConfiguration services,
            IStatePropertyAccessor<ToDoSkillState> toDoStateAccessor,
            IStatePropertyAccessor<ToDoSkillUserState> userStateAccessor,
            ITaskService serviceManager,
            IMailService mailService,
            IBotTelemetryClient telemetryClient)
            : base(nameof(AddToDoItemDialog), services, toDoStateAccessor, userStateAccessor, serviceManager, mailService, telemetryClient)
        {
            TelemetryClient = telemetryClient;

            var addToDoTask = new WaterfallStep[]
            {
                GetAuthToken,
                AfterGetAuthToken,
                ClearContext,
                CollectToDoTaskContent,
                CollectSwitchListTypeConfirmation,
                AddToDoTask,
            };

            var collectToDoTaskContent = new WaterfallStep[]
            {
                AskToDoTaskContent,
                AfterAskToDoTaskContent,
            };

            var collectSwitchListTypeConfirmation = new WaterfallStep[]
            {
                AskSwitchListTypeConfirmation,
                AfterAskSwitchListTypeConfirmation,
            };

            // Define the conversation flow using a waterfall model.
            AddDialog(new WaterfallDialog(Action.AddToDoTask, addToDoTask) { TelemetryClient = telemetryClient });
            AddDialog(new WaterfallDialog(Action.CollectToDoTaskContent, collectToDoTaskContent) { TelemetryClient = telemetryClient });
            AddDialog(new WaterfallDialog(Action.CollectSwitchListTypeConfirmation, collectSwitchListTypeConfirmation) { TelemetryClient = telemetryClient });

            // Set starting dialog for component
            InitialDialogId = Action.AddToDoTask;
        }
    }
}

﻿using BlazorRTEWithOpenAI.Data;
using System.Text;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Navigations;
using Syncfusion.Blazor.RichTextEditor;
using Syncfusion.Blazor.SplitButtons;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace BlazorRTEWithOpenAI.Pages
{
    public partial class Index
    {
        [Inject]
        private HttpClient httpClient { get; set; }

        private SfDropDownButton? dropDownButton;
        private SfContextMenu<ContextMenuItem>? contextMenu;
        private bool showSpinner = false;
        private string Value { get; set; } = "<p>The Olympics is a global multisport event that take place for every four year. Thousands of athletes from all over the world come together to compete in various sportss and represent their countries. The Olympics is a celebration of athleticism, sportsman ship, and international unity, and it continues to inspire people all over world.</p>";

        private List<ToolbarItemModel> Tools = new List<ToolbarItemModel>()
        {
            new ToolbarItemModel() { Command = ToolbarCommand.Bold },
            new ToolbarItemModel() { Command = ToolbarCommand.Italic },
            new ToolbarItemModel() { Command = ToolbarCommand.Underline },
            new ToolbarItemModel() { Command = ToolbarCommand.Separator },
            new ToolbarItemModel() { Command = ToolbarCommand.Formats },
            new ToolbarItemModel() { Command = ToolbarCommand.Alignments },
            new ToolbarItemModel() { Command = ToolbarCommand.Separator },
            new ToolbarItemModel() { Name = "AI", TooltipText = "Edit with AI" },
            new ToolbarItemModel() { Command = ToolbarCommand.Separator },
            new ToolbarItemModel() { Command = ToolbarCommand.SourceCode },
            new ToolbarItemModel() { Command = ToolbarCommand.Separator },
            new ToolbarItemModel() { Command = ToolbarCommand.Undo },
            new ToolbarItemModel() { Command = ToolbarCommand.Redo }
        };

        private List<ContextMenuItem> AIMenuItems = new List<ContextMenuItem>{
            new ContextMenuItem { Id ="RephraseContent", Content = "Rephrase" , Items = new List<ContextMenuItem> {
                new ContextMenuItem { Content="Standard", Prompt = Constants.STANDARD },
                new ContextMenuItem { Content="Formal", Prompt = Constants.FORMAL },
                new ContextMenuItem { Content="Expand", Prompt = Constants.EXPAND },
                new ContextMenuItem { Content="Shorten", Prompt = Constants.SHORTEN}
            }},
            new ContextMenuItem { Id ="ChangeContentTone", Content = "Change tone", Items = new List<ContextMenuItem> {
                new ContextMenuItem { Content="Professional", Prompt = Constants.PROFESSIONAL },
                new ContextMenuItem { Content="Casual", Prompt = Constants.CASUAL },
                new ContextMenuItem { Content="Straightforward", Prompt = Constants.STRAIGHTFORWARD },
                new ContextMenuItem { Content="Confident", Prompt = Constants.CONFIDENT},
                new ContextMenuItem { Content="Friendly", Prompt = Constants.FRIENDLY },
            }},
            new ContextMenuItem { Separator = true },
            new ContextMenuItem { Id ="CheckGrammar", Content = "Fix spelling & grammar", Prompt = Constants.GRAMMARCHECK},
            new ContextMenuItem { Id ="Article", Content = "Convert to article", Prompt = Constants.CONVERTTOARTICLE }
        };

        public void OnMenuCreated()
        {
            contextMenu?.Open();
        }

        private async Task ItemSelected(MenuEventArgs<ContextMenuItem> args)
        {
            if (string.IsNullOrEmpty(args.Item.Prompt))
                return;

            var prompt = args.Item.Prompt + Value;
            
            dropDownButton?.Toggle();
            this.showSpinner = true;
            Value = await createCompletion(prompt);
            this.showSpinner = false;
        }

        // Method to get the completion result from OpenAI
        private async Task<string> createCompletion(string prompt)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Constants.OPENAI_KEY);
            var openAIPrompt = new
            {
                model = Constants.OPENAI_MODEL,
                prompt,
                temperature = 0.5,
                max_tokens = 1500,
                top_p = 1,  
                frequency_penalty = 0,
                presence_penalty = 0
            };

            var content = new StringContent(JsonSerializer.Serialize(openAIPrompt), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(Constants.API_ENDPOINT, content);
            var jsonContent = await response.Content.ReadAsStringAsync();
            var choices = JsonDocument.Parse(jsonContent).RootElement.GetProperty("choices").GetRawText();
            var result = JsonDocument.Parse(Regex.Replace(choices, @"[\[\]]", string.Empty)).RootElement;
#pragma warning disable CS8603 // Possible null reference return.
            return result.GetProperty("text").GetString();
#pragma warning restore CS8603 // Possible null reference return.
        }
    }
}

using BlazorRTEWithOpenAI.Data;
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
        private string Value { get; set; } = "<h1>The Olympics</h1>\r\n<p>The Olympics is an global multi sport event that takes place for every four years. Thousands of athletes from all over the world come together to compete in various sports and represent their countries.</p>\r\n<h2>History of Olympics</h2>\r\n<p>The Olympics date back to ancient Greece, where they were held to honor the god Zeus. The modern Olympics were first held in Athens, Greece in 1896 and have since grown to become one of the most prestigious sporting events in the world.</p>\r\n<h2>Popular Olympic Sports</h2>\r\n<p>Some of the most popular sports in Olympics include:</p>\r\n<ul>\r\n<li>Track and field</li>\r\n<li>Gymnastics</li>\r\n<li>Swimming</li>\r\n<li>Basketball</li>\r\n<li>Tennis</li>\r\n<li>Soccer</li>\r\n</ul>\r\n<p>The Olympics is a celebration of athleticism, sportsmanship, and international unity, and it continues to inspire people all over the world.</p>";

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

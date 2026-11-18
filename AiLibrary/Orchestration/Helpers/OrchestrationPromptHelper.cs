using LocalAiLibrary.AiLibrary.AITools.Models;
using LocalAiLibrary.AiLibrary.Orchestration.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace LocalAiLibrary.AiLibrary.Orchestration.Helpers
{
    public class OrchestrationPromptHelper
    {
        public string GetCollectionPrompt(CollectionResponse response, string returnMessage)
        {
            response.Prompt = "Present the results as an HTML table. Make the heading gray background color gray. Do not use MarkDown";
            response.Result = returnMessage;
            return JsonSerializer.Serialize(response);
        }

        public string GetNonCollectionPrompt(CollectionResponse response, string returnMessage)
        {
            List<string> itemResult = [returnMessage];
            string serializedItemResult = JsonSerializer.Serialize(itemResult);
            response.Prompt = "Present the results as an HTML table. Make the heading gray background color gray. NO TOOL CALLS. Do not use MarkDown";
            response.Result = serializedItemResult;
            return JsonSerializer.Serialize(response);
        }

        public string GetSilentPrompt(CollectionResponse response, OrchestrationItem? itemToProcess)
        {
            string processedItem = itemToProcess.Prompt + " complete!";
            response.Prompt = "Present the result to the user. NO TOOL CALLS. Do not use MarkDown";
            response.Result = processedItem;
            return JsonSerializer.Serialize(response);
        }

        public string GetExceedRetryCountPrompt(OrchestrationPlan orchestrationPlan)
        {
            CollectionResponse response = new CollectionResponse();
            string processedItem = "Call exceeded retry count! Please reference agent " + orchestrationPlan.Id + " in your logs.";
            response.Prompt = "Present the entire contents of the result node to the user. Present nothing else but this content. NO TOOL CALLS. Do not use MarkDown.";
            response.Result = processedItem;
            return JsonSerializer.Serialize(response);
        }
    }
}

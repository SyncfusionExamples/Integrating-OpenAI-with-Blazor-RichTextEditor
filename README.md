# Integration OpenAI with Blazor RichTextEditor

## Integrated AI toolbar options in Blazor RichTextEditor

- Rephrase
  * Standard
  * Formal
  * Expand
  * Shorten

- Change tone
  * Professional
  * Casual
  * Straightforward
  * Confident
  * Friendly

- Fix spelling & grammar
- Convert to article

<b>Screenshot:</b>

![image](https://user-images.githubusercontent.com/93309591/222420943-a58fbcb2-8e6a-4ecf-a881-aa3680a587e1.png)

![image](https://user-images.githubusercontent.com/93309591/222421252-f4699b68-168b-4ac8-87c6-327980c5ff14.png)

## Steps to do before the sample run

* Open the `./src/Data/Constants.cs` file.
* Replace your OpenAI License key in the `OPENAI_KEY` const variable.

```cs
const string OPENAI_KEY = "Provide_Your_OpenAI_License_Key_Here";
```

* Now, run the sample to see the OpenAI integration in Blazor RTE component.

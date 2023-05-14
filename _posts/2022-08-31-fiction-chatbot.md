---
layout: post
title: building a chatbot trained on fiction
---

Making an NLP dialogue agent and finetuning it on various prose is fairly easy to do in 2022, so don't be like 2020-me and make it harder than it needs to be.


*(update Dec 2022) I did a workshop on this topic, slides are [here](https://rvirmoors.github.io/ccia/slides/chatbot-workshop).*

*(update May 2023) there are already tools to make this [so much easier and better](https://minimaxir.com/2023/03/new-chatgpt-overlord/) that this guide is now little more than a historical curiosity.*

There's all sorts of ways to use AI for text-based art - see [these slides](https://ghostweather.slides.com/lynncherny/deck-6b091e) by Lynn Cherny for a current overview. Our project entailed a sort of futuristic fortuneteller, trained on some speculative works, of whom one is directed to ask questions about the future.

![fortuneteller illustration, made with DALL-E](/images/DALL%C2%B7E%202022-08-31%2021.04.05%20-%20futuristic%20fortuneteller%2C%20nostradamus%2C%20plan%209%20from%20outer%20space%2C%20illustration.png?raw=true)

### How NOT (?) to do it

Looking back at my first attempts in [this 2020 repo](https://github.com/RVirmoors/fiction-chatbot/tree/master/2020), my approach to the task was to take existing dialogue response generation models (like [DialoGPT](https://github.com/microsoft/DialoGPT)) and finetune them with our training corpus. This was inspired by projects like the [Rick and Morty bot](https://chatbotslife.com/creating-a-rick-sanchez-chat-bot-with-transformers-and-chai-18139f4c375f).

Now, what's the problem with this approach? Well, you need to process your dataset into the form DialoGPT et al expect, which is... dialogues. This is easy enough to do if you have a bunch of [Rick and Morty scripts](https://www.kaggle.com/andradaolteanu/rickmorty-scripts), but if we're dealing with prose we're immediately faced with having to (1) parse out relevant conversation (not a trivial task!) and (2) throw out A LOT of potentially useful text that simply isn't in dialogue form. 

Which is what I did, over several attempts, with underwhelming results. Now, I'm not saying this approach is totally wrong, and indeed there's [a lot of research](https://www.mdpi.com/2078-2489/13/1/41) on all kinds of bespoke conversational agents, but there is a more straightforward way to accomplish what we were after.

### Prompt engineering

Turns out you can get large language models to do what you want by simply describing it to them. GPT-3 is probably [most well known for this](https://blog.andrewcantino.com/blog/2021/04/21/prompt-engineering-tips-and-tricks/), but GPT-2 (and similar) can handle a basic Q-A sequence perfectly well, too.

So, in fact, you *don't* need a bespoke dialogue model to get GPT to talk to you. Just use such a prompt:[^1]

```
"""[An oracle from the future.]

Q: Who are you?
A: My identity is of no importance.
Q: Who am I?
A: You are a human with a thirst for knowledge. Ask of me all the questions you desire.
Q: What is my future?
A: The future is where you are going to spend the rest of your life. Future events such as these will affect you in the future.
Q: 
"""
```

Without any fine-tuning, this will direct GPT to keep producing Q-A sequences, which we can truncate to just the first A for our use case. The prompt should be just long enough to clearly describe the desired behaviour. My first two Q&A pairs try to establish the participants[^2], and ascribe a soothsayery verbosity to the A's.

Of course the real fun begins when you finetune the model on a bunch of your own text. This leaves humongous models like GPT-3 out of the question, and if you want to stick to Colab your choices are even more limited -- pretty much boiling down to [GPT-2](https://huggingface.co/gpt2) or [GPT-Neo 125M](https://huggingface.co/EleutherAI/gpt-neo-125M).

The Huggingface ecosystem makes [training](https://huggingface.co/models?pipeline_tag=text-generation) and [generation](https://huggingface.co/blog/how-to-generate) as straightforward as possible. [Max Woolf's aitextgen](https://docs.aitextgen.io/) is a very nice toolset, but I made do with a notebook derived from [this repo by Peter Albert](https://github.com/Xirider/finetune-gpt2xl).

[HERE IT IS](https://colab.research.google.com/drive/1_u3wb7DOW6eisGWQpCrgX2Gegj0QqyRu?usp=sharing).

### Putting it in production

The easiest way to train on a bunch of texts is to dump them all into a .txt file. But not so fast, you still need to do some cleanup like removing page numbers, footnotes etc. [Regex](https://regex101.com/) is your friend here. Don't be shy about editing the text itself, after all nobody will ever see this. For instance, I replaced the word "chapter" with "utterance" since I didn't want the oracle talking about chapters, page numbers etc.

Once your model is trained, deploy as appropriate. In my case, we only needed to run it on a local machine, so I made a [local script](https://github.com/RVirmoors/fiction-chatbot/blob/master/chatbot.py) that runs inference on the CPU.[^3] 

It's important to test this thoroughly: one thing that isn't apparent in the Colab environment is that, for some reason, the GPT-Neo model inference duration accumulates when run in a loop... Reloading the model didn't help: for every new question, the answer kept taking ~7 seconds longer to generate! Initially I was running GPT-Neo since it's a bit newer and bigger, but switching to the GPT-2 architecture made everything run fast and smooth, as expected.

### Final thoughts

We're pretty happy about how the bot is doing. If you happen to be in Linz for Ars Electronica, [check it out](https://ars.electronica.art/planetb/en/who-are-you/) in the Campus section, next to [works](https://ars.electronica.art/planetb/en/pixels-from-a-past-future/) by our ITPMA M.A. students.

Of course I'd love access to a huge PC able to train a larger model, but so far the small-medium ones have been performing OK. Still, the training itself remains somewhat of an open question. With the default settings, even a single epoch (!) is enough to visibly steer the model in the direction of the dataset. Multiple epochs do improve the quality of the responses, but how much is enough? How good can it get? I didn't have time to adequately explore this, the error metrics didn't help, and I didn't find much actionable info on finetuning strategies out there.

[^1]: Yes I did paraphrase [Plan 9 from Outer Space](https://en.wikiquote.org/wiki/Plan_9_from_Outer_Space). This bot is basically a 21st century Criswell.

[^2]: Despite my efforts, the bot still sometimes replies to first person questions ("At what age am I going to die?") in the first person.

[^3]: We used the computer's GPU for something else.
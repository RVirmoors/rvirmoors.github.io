---
layout: post
title: adding PyTorch to Max/MSP externals
---

My current project entails working with ML models and real-time audio,
so when I discovered the [C++ frontend](https://pytorch.org/tutorials/advanced/cpp_frontend.html)
to PyTorch my first instinct was to combine it with Cycling74's [Max SDK](https://cycling74.com/downloads/sdk/).

GET: [GitHub](https://github.com/RVirmoors/simplemsptorch-)

[Installing LibTorch](https://pytorch.org/cppdocs/installing.html) is easy enough,
and compiling a test project in Visual Studio is just a matter of including the right folders
and linking the .lib files[^1]. So, you would think that applying this to a Max SDK example would be
easy enough, right? Not so fast!

First, you need to set the project to compile as C++ code (obviously). Then, and this took me
two nights to figure out, you also need to include "torch.h" **before** Max's own headers ("ext.h" et al)
and not after.

Anyway, it's now on [GitHub](https://github.com/RVirmoors/simplemsptorch-) in two flavours: one is
the standard C example from the SDK, and the other is the C++ bridge that I regularly use.[^3]

Now, should you use these for development? Probably not, unless you're some LibTorch *and* Max SDK wizard
(in which case, what are you doing reading this? I should be learning from you!). So far I've only
encountered one other instance of LibTorch being used with Max, by
[Philippe Esling &co @ IRCAM](https://github.com/acids-ircam/flow_synthesizer/)[^2]. What I intend to do
is design & train my model in good ol' Python, and then load it into a Max external. And the next step
would be doing some online training in Max, but we're already getting ahead of ourselves.

[^1]: Also see [this](https://medium.com/@boonboontongbuasirilai/building-pytorch-c-integration-libtorch-with-ms-visual-studio-2017-44281f9921ea) Medium post.

[^2]: They too are still using standard PyTorch for main R&D though.

[^3]: (edit Nov '20) I discovered the official C++ Development Kit for Max, called [Min](https://cycling74.github.io/min-devkit/). Don't see why it wouldn't work with LibTorch, too: one more for the TODO pile then.

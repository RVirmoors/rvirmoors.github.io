---
layout: post
title: Adding PyTorch to Max/MSP externals
---

GET: [GitHub](https://github.com/RVirmoors/simplemsptorch-)

My current project entails working with ML models and real-time audio,
so when I discovered the [C++ frontend](https://pytorch.org/tutorials/advanced/cpp_frontend.html)
to PyTorch my first instinct was to combine it with the [Max SDK](https://cycling74.com/downloads/sdk/).

[Installing LibTorch](https://pytorch.org/cppdocs/installing.html) is easy enough,
and compiling a test project in Visual Studio is just a matter of including the right folders
and linking the .lib files.

---
layout: post
title: rolypoly~ demo @ AIMC 2023
---

I'm back from [AIMC 2023](https://aimc2023.pubpub.org/),[^1] where I finally got people to play with [rolypoly~](https://github.com/RVirmoors/rolypoly), my interactive drum sequencer.

![person-trying-rolypoly](/images/aimc/demo2.jpg)

There is a [release available](https://github.com/RVirmoors/rolypoly/releases) if you want to check it out for yourself. My main priorities right now are:
- creating a couple of videos showing off the functionality
- building the OSX version of the Max external

I'm also adding some quality-of-life and debugging functions to the object, which should be done by the end of the month.

But first, I want to use this space to vent a little about the pains of trying to make interactive music software with deep learning. As I see it, there are 3 options available:

### 1. Python backend → OSC → Max/... frontend.

This is the solution used by most researchers. They're able to use a standard deep learning pipeline (I'm familiar with PyTorch but this goes for Tensorflow, Jax and other frameworks) to develop and optimize their models and test them in jupyter/colab notebooks. Finally, they can add interactivity by using a protocol like OSC to send data from a Python script running inference in real time, to a host such as Max, Pure Data, SuperCollider etc.

I went this way for my initial research [back in 2020](https://program.ismir2020.net/lbd_422.html). The main problem, for my case, beyond the logistical hassles (you need to run a Python script in parallel with your host), was the timing jitter in OSC. This meant I couldn't trigger drum hits directly through OSC messages and would instead need to send the timing information to a special sequencer inside Max. This sequencer would receive these timing values and output the millisecond-accurate hits.

Also, let's not forget that OSC is a symbolic protocol. I don't think Python can do jitter-free audio streaming for interactive music, which leads us to...

### 2. TorchScript backend → Max/... frontend.

[TorchScript](https://pytorch.org/tutorials/beginner/Intro_to_TorchScript_tutorial.html) is a subset of PyTorch used in several modern projects, most notably [IRCAM's nn~](https://github.com/acids-ircam/nn_tilde) (which powers [RAVE](https://github.com/acids-ircam/RAVE)), allowing deep models to process audio in real time.

It allows you to specify your architecture in a PyTorch-like manner, and then export it and run it within a C++ host (such as a Max external). This seemed ideal for my use case, so I used it for v2.0 of rolypoly~[^2], which I submitted to AIMC this Spring. The [videos in my paper](https://aimc2023.pubpub.org/pub/ud9m40jc) are created using this version.

Now, the issue here is that TorchScript *isn't PyTorch* -- in fact it's missing a bunch of features from torch. The moment you want to do something beyond defining an `nn.Module`, you're stuck having to find all sorts of weird workarounds. Case in point, a central function of my system is the ability to retrain (finetune) the model interactively. But you can't export a PyTorch training loop with TorchScript, at least not directly: this involves silly things like implementing your own optimizer etc. TorchScript simply isn't made for this. I managed to get it to work, just barely, but it wasn't a sustainable design.

### 3. LibTorch backend + Max/... frontend.

So I went back to the drawing board and finally decided to stop worrying and love the C++. As I've [discussed before](/2022/07/04/libtorch-mindevkit), I'd already started using LibTorch ([the C++ API of PyTorch](https://pytorch.org/cppdocs/installing.html)) in conjunction with Min-DevKit ([toolkit for writing Max externals in C++](https://github.com/Cycling74/min-devkit)) to write my source code, so why not port everything from Python to C++? How hard could it be?

Well, it was as tedious as you'd expect, but in the end PyTorch's C++ and Python API's are [close enough to parity](https://github.com/pytorch/pytorch/issues/25883) that I'd say for time-sensitive real-time apps, making a complete transition to C++ is worth it nowadays.

And I do mean complete. Aside from some data parsing, I've moved all my code to C++, including the initial training of the main model, which happens offline, before it's loaded into the Max object. Since the model is already defined in C++, is there still a point in maintaining a parallel Python implementation for the pretraining phase?

Well, remember the testing-diagnostics pipeline I mentioned above? The Python ecosystem is still vastly superior for research and optimization, with tools like `matplotlib` and TensorBoard being widely used. I've been getting by without them so far, focussing less on finding optimal hyperparameters for my models. Frankly I'm too old for this kind of work. The exploitation of brain power (not to mention natural resources) in the service of optimization is one of the dark sides of deep learning, and still considered funny enough to be included in [official PyTorch tutorials](https://pytorch.org/tutorials/advanced/cpp_frontend.html):

![ha-ha](/images/aimc/joke.png)

Hilarious, right? I want to avoid this as much as possible. Even so, for anyone working in this field, the temptation to keep optimizing is often irresistible. The tradeoff between model performance and our mental health is often not worth it.

On the other hand, there are bug fixes and diagnostics that become unavoidable at the current stage of my project, so I'll see if I can work these into my existing framework (e.g. using Max/Jitter to visualize layer activations) or if it's better to return to Python's warm embrace.

### System design

All this PyTorch talk, and I didn't even get to the inner workings of rolypoly~. For now I'll just leave you with this gif, which can't really show the inference and sequencing parts running in parallel:

![roly-flow](https://github.com/RVirmoors/rolypoly/raw/master/_assets/workflow.gif)

My plan is to cover every relevant aspect (including basic dev tricks like [precompiled headers](https://www.youtube.com/watch?v=eSI4wctZUto) and debugging using try-catch, applied to Min+LibTorch) in a series of videos which, who knows, may lead to a code re-write? There's a lot of unexplored territory and I'd love to share all I've learned, which still is 1% of what's really needed. Meanwhile though, school is about to start, so who knows when I'll find time for all of this.

[^1]: AIMC was pretty great overall, I'm very glad I went. Special shout out to [Behzad](https://behzadhaki.com/) and [Julian](https://github.com/JLenzy) who are working on similar things, as well as being all around cool people. However, the best part of this Brighton trip was [I got to see Deerhoof](https://www.brightonandhovenews.org/2023/09/03/deerhoof-hit-miracle-level-at-brighton-gig/)!

[^2]: I messed up a bit on the version numbering. Version 2.0 is Transformer-based, as opposed to the initial Seq2Seq-based system from 2020. Then I switched to LibTorch and made it 2.0.1, although it's different enough to warrant 2.1... But then, none of these are anyhere near production-ready, so in reality they should be 0.1, 0.2, 0.2.1... Someday I'll think this stuff through properly.
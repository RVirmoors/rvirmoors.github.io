---
layout: presentation
title: Practical Python
---

class: center, middle
.title[Creative Coding and Software Design 1] 
<br/><br/>
.subtitle[Week 12: Practical Python]
<br/><br/><br/><br/><br/><br/>
.date[Jan 2021] 
<br/><br/><br/>
.note[Created with [{Liminal}](https://github.com/jonathanlilly/liminal) using [{Remark.js}](http://remarkjs.com/) + [{Markdown}](https://github.com/adam-p/markdown-here/wiki/Markdown-Cheatsheet) +  [{KaTeX}](https://katex.org)]

???

Author: Grigore Burloiu, UNATC
    
---
name: toc
class: left
# ★ Table of Contents ★    
      
1. [Installing Python](#installing)
2. [Three ways to access Python](#three)
3. [Python language specifics](#lang)
4. [Working with libraries (modules)](#modules)
5. [Links](#links)

???

This session is intended as a practical overview of the Python infrastructure you need to know and understand in order to make sense of existing open source codebases and to start to write your own scripts.

The structure is a mix of practical, *shallow* notions 
about the Python infrastructure and ecosystem, and
a hands-on introduction to the language.

In next week's session we will be delving into some more
advanced features and some *deeper* ideas about
"Pythonic" programming.

But for now, the main takeaway I hope you'll get is that
Python is a flexible, easy-to-use language, with multiple
points of entry and applications, and which doesn't look that
much different from the p5 / p5js you've used already.
          
---
layout: true  .toc[[★](#toc)]
        
---
name: installing  
class: left
# Installing Python
Check your version:

```powershell
python --version
python
```

Python **3**.7+

64 bit

???

First, let's check if we already have Python installed on our system. Most modern code should work with version 3.7 or later.

You can find out if your Python is 32 or 64 bit by running "python". Certain libraries, e.g. all modern deep learning frameworks, require 64 bit.

If you're on a Mac, it's likely you already have Python v2 installed, which won't do. There are tools that help you manage your Python installs, like Homebrew and Pyenv. And there's Anaconda, which can also manage your [libraries](#modules) -- think of it as an equivalent of Unity Hub maybe.

So far I've been able to manage without these on my Windows PC (I do use Homebrew on the Mac), but you might find them useful.

--

[https://www.python.org/downloads/](https://www.python.org/downloads/)

???

You can follow the link to download a current version for your system.

---

name: three  
class: left
# Three ways to access Python

1. From the **command line**
2. In an **editor** / IDE
3. In Jupyter **notebooks**

---

## Command line

---

## Editor

---

## Notebooks

---

name: lang
class: left
# Python language specifics

---

name: modules
class: left
# Working with libraries (modules)

Example code:
```python
import torch		# import the whole library 
import numpy as np	# import the whole library, with an alias
from torch.utils.data import DataLoader 	# import a specific class or function 

# \[...\] 

x = torch.Tensor(\[0.\]).numpy()
x = np.add(x, \[1.\])
d = DataLoader(dataset)
```

---

## Installing modules

---

## Managing installs: virtual environments

---

## Libraries in TouchDesigner

---

name: links
class: left
# Links

https://jakevdp.github.io/WhirlwindTourOfPython/
https://ml4a.github.io/classes/itp-F18/terminal-velocity/
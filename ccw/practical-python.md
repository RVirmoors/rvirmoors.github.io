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
#Table of Contents        
      
1. [Installing Python](#installing)
2. [Three ways to access Python](#three)
3. [Python language specifics](#lang)
4. [Working with libraries (modules)](#modules)
5. [Links](#links)

???

This class is intended as a practical overview of the Python infrastructure you need to know and understand in order to make sense of existing open source codebases and to start to write your own scripts.

          
---
layout: true  .toc[[&#10023;](#toc)]
        
---
name: installing  
class: left
# Installing Python

[https://www.python.org/downloads/](https://www.python.org/downloads/)

???

The first thing we need to do is to install Python. Follow the link and download.

--

Check your installation: (should be at least version 3.7)

```powershell
python --version
```

???
OSX bla

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
import torch 		# import the whole library 
import numpy as np 	# import the whole library, with an alias
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
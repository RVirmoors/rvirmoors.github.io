---
layout: post
title: LibTorch for Min-DevKit
---

A while ago I wrote [a post](/2020/05/01/libtorch-max-external/) showing how to embed PyTorch (via LibTorch) in a Max object using the standard Max SDK.
Now, 2 years later (!), I'm finally getting around to building the [rolypoly](https://github.com/RVirmoors/rolypoly) Max object, and I've decided to start using the shiny new [Min](https://cycling74.github.io/min-devkit/) system, which provides a much smoother C++ experience for writing Max externals.

And since the [Max 8.2 SDK update](https://github.com/Cycling74/max-sdk/blob/main/README-8.2-update.md), the process is fairly streamlined, as [this video](https://youtu.be/il5WblTBUgs) also shows.[^1] Here's how you do it:

First, make sure you have [cmake](https://cmake.org/download/) and [ruby](https://rubyinstaller.org/) installed. Now, follow [the instructions](https://github.com/Cycling74/min-devkit) to install Min. Then proceed to the guide for [creating a new package](https://github.com/Cycling74/min-devkit/blob/main/HowTo-NewPackage.md). Note: I got the error documented in [this issue](https://github.com/Cycling74/min-devkit/issues/102#issuecomment-368548906) - the solution is to simply run `ruby script/create_package.rb ../yourProject`.

Then follow [the instructions](https://github.com/Cycling74/min-devkit/blob/main/HowTo-NewObject.md) for creating a new object, which involves copying one of the existing examples. But before you run CMake to generate the project files, you need to edit CMakeLists.txt to add the LibTorch includes and [libraries](https://stackoverflow.com/questions/24570916/add-external-libraries-to-cmakelist-txt-c):[^2]

```cmake
include_directories( 
	"${C74_INCLUDES}"
	${CMAKE_CURRENT_SOURCE_DIR}/../../../libtorch/include
	${CMAKE_CURRENT_SOURCE_DIR}/../../../libtorch/include/torch/csrc/api/include
)

link_directories(
	${CMAKE_CURRENT_SOURCE_DIR}/../../../libtorch/lib
)

link_libraries(
	torch.lib
	torch_cpu.lib
	c10.lib
)
```
Of course, you should also [download](https://pytorch.org/get-started/locally/) and extract LibTorch to your Max package folder. This would result in something like `Max 8/Packages/yourProject/libtorch`. Only then should you run CMake and then build your project.[^3]

A big advantage of starting with a Max Package right off the bat like this, is that you don't need to copy dll files to your Max application folder. Just copy every dll file from `yourProject/libtorch/lib` to a `yourProject/support` folder which you create and Max [knows to look for](https://docs.cycling74.com/max8/vignettes/packages).

Now, to test that it works properly, edit `source/projects/yourProject/yourProject.cpp` and add:
```c++
#include "torch\torch.h"

...

	// constructor
	yourProject() {
		torch::Tensor tensor = torch::rand({ 2, 3 });
		cout << "random tensor: " 
			<< tensor[0][0].item<float>() << " " << tensor[0][1].item<float>() << " " << tensor[0][2].item<float>() 
			<< " | " 
			<< tensor[1][0].item<float>() << " " << tensor[1][1].item<float>() << " " << tensor[1][2].item<float>()
			<< endl;
	}
```

Then open Max and, when you create an `yourProject` object you should see a random tensor in the console. Now you're ready to move forth into [Min](https://cycling74.github.io/min-devkit/)+[LibTorch](https://pytorch.org/cppdocs/frontend.html) greatness!

[^1]: As with my initial Max SDK guide, these instructions are Windows-specific. A similar process should work for Mac, but I haven't tried it yet.

[^2]: Depending on your version of LibTorch, the exact names of the libraries [may change](https://medium.com/@boonboontongbuasirilai/building-pytorch-c-integration-libtorch-with-ms-visual-studio-2017-44281f9921ea). Check your `libtorch/lib` folder contents.

[^3]: I use VS Code, whose CMake integration is very clever: make any change in CMakeLists, save it, and VS Code automatically regenerates your project files, so you just need to hit 'Build'.
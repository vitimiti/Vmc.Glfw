# Vcm.Glfw

This is a safe, managed library that imports and manages lifetimes
for the C GLFW library. Unlike the original C library, all lifetimes
are managed through builders and disposable objects.

## How to Use

Using the original example from the
[GLFW Getting Started Tutorial](https://glfw.org/docs/latest/quick.html),
we can convert the following C program into a .NET program.

This program:

```c
#include <GLFW/glfw3.h>
#include <stdio.h>
#include <stdlib.h>

static void error_callback(int error, const char* description)
{
    fprintf(stderr, "Error: %s\n", description);
}

static void key_callback(GLFWwindow* window, int key, int scancode, int action, int mods)
{
    if (key == GLFW_KEY_ESCAPE && action == GLFW_PRESS)
    {
        glfwSetWindowShouldClose(window, GLFW_TRUE);
    }
}

int main(void)
{
    glfwSetErrorCallback(error_callback);
    if (!glfwInit())
    {
        exit(EXIT_FAILURE);
    }

    glfwWindowHint(GLFW_CONTEXT_VERSION_MAJOR, 3);
    glfwWindowHint(GLFW_CONTEXT_VERSION_MINOR, 3);
    glfwWindowHint(GLFW_OPENGL_PROFILE, GLFW_OPENGL_CORE_PROFILE);

    GLFWwindow* window = glfwCreateWindow(640, 480, "OpenGL Triangle", NULL, NULL);
    if (!window)
    {
        glfwTerminate();
        exit(EXIT_FAILURE);
    }

    glfwSetKeyCallback(window, key_callback);

    glfwMakeContextCurrent(window);
    // Load OpenGL
    glfwSwapInterval(1);

    // Initialise our OpenGL code to make a triangle

    while (!glfwWindowShouldClose(window))
    {
        int width, height;
        glfwGetFrameBufferSize(window, &width, &height);
        float const ratio = width / (float)height;

        // Do our OpenGL drawing here

        glfwSwapBuffers(window);
        glfwPollEvents();
    }

    glfwDestroyWindow(window);
    glfwTerminate();
    exit(EXIT_SUCCESS);
}
```

Translates to:

```csharp
using System.Drawing;
using Vmc.Glfw;
using Vmc.Glfw.Builders;

// We don't need to set an error callback, this is done through exceptions
using var nativeContext = new NativeContextBuilder().Build();
using var window = nativeContext
    .CreateWindowBuilder()
    .WithContextVersion(new Version(3, 3))
    .WithOpenGLProfile(OpenGLProfile.Core)
    .Build(new Size(640, 480), "OpenGL Triangle");

window.SetKeyCallback(KeyCallback);

window.MakeContextCurrent();

// Initialise OpenGL here
nativeContext.SetSwapInterval(1);

// Initialise our OpenGL code to make a triangle

while (!window.ShouldClose)
{
    var frameBufferSize = window.FrameBufferSize;
    var ratio = frameBufferSize.Width / (float)frameBufferSize.Height;

    // Do our OpenGL drawing here

    window.SwapBuffers();
    nativeContext.PollEvents();
}

// The destruction of the window and the context termination
// happen here automatically through the using idiom.

return;

static void KeyCallback(Window window, Key key, int scancode, KeyAction action, KeyModifiers mods)
{
    if (key is Key.Escape && action is KeyAction.Press)
    {
        window.ShouldClose = true;
    }
}

```

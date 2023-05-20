#ifndef PCH_H
#define PCH_H

#include "targetver.h"

#ifndef WIN32_LEAN_AND_MEAN
#define WIN32_LEAN_AND_MEAN
#include <Windows.h>
#endif//WIN32_LEAN_AND_MEAN

#include "Resource.h"

#ifndef IMGUI_DEFINE_MATH_OPERATORS
#define IMGUI_DEFINE_MATH_OPERATORS
#include "Dependencies/ImGui/imgui.h"
#include "Dependencies/ImGui/imgui_impl_dx9.h"
#include "Dependencies/ImGui/imgui_impl_win32.h"
#endif//IMGUI_DEFINE_MATH_OPERATORS

#include <d3d9.h>
#pragma comment(lib, "d3d9.lib")
#include <wrl/client.h>
using namespace Microsoft::WRL;

#include <array>
#include <iterator>
#include <memory>
#include <vector>

#endif//PCH_H
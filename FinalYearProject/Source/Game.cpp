#include "Game.h"
#include "Object.h"

IMGUI_IMPL_API LRESULT ImGui_ImplWin32_WndProcHandler(
    HWND,
    UINT,
    WPARAM,
    LPARAM
);

Game::Game(
    const HINSTANCE instance,
    const int       showCmd
) :
    instance{ instance },
    showCmd{ showCmd } {
    loadTitle();
    loadClass();
    registerClass();
    createWindow();
    createDeviceD3D();
    initImGui();
}

Game::~Game() {
    cleanupImGui();
    cleanupDeviceD3D();
    DestroyWindow(windowHandle);
    UnregisterClass(windowClass.data(), instance);
}

auto Game::initImGui() const -> void {
    // Setup Dear ImGui context
    IMGUI_CHECKVERSION();
    ImGui::CreateContext();
    ImGuiIO& io = ImGui::GetIO(); (void)io;
    io.ConfigFlags |= ImGuiConfigFlags_NavEnableKeyboard;     // Enable Keyboard Controls
    io.ConfigFlags |= ImGuiConfigFlags_NavEnableGamepad;      // Enable Gamepad Controls

    // Setup Dear ImGui style
    ImGui::StyleColorsDark();
    //ImGui::StyleColorsLight();

    // Setup Platform/Renderer backends
    ImGui_ImplWin32_Init(windowHandle);
    ImGui_ImplDX9_Init(direct3DDevice9.Get());
}

auto Game::cleanupImGui() const -> void {
    ImGui_ImplDX9_Shutdown();
    ImGui_ImplWin32_Shutdown();
    ImGui::DestroyContext();
}

auto Game::loadTitle() -> void {
    const auto windowTitleLength = LoadString(
        instance,
        IDS_APP_TITLE,
        windowTitle.data(),
        static_cast<int>(windowTitle.size())
    );
    windowTitle[windowTitleLength] = '\0';
}

auto Game::loadClass() -> void {
    const auto windowClassLength = LoadString(
        instance,
        IDC_FINALYEARPROJECT,
        windowClass.data(),
        static_cast<int>(windowClass.size())
    );
    windowClass[windowClassLength] = '\0';
}

auto Game::registerClass() const -> void {
    WNDCLASSEX wcex;
    wcex.cbSize = sizeof(wcex);
    wcex.style = CS_CLASSDC;
    wcex.lpfnWndProc = &windowProc;
    wcex.cbClsExtra = 0;
    wcex.cbWndExtra = 0;
    wcex.hInstance = instance;
    wcex.hIcon = LoadIcon(instance, MAKEINTRESOURCE(IDI_FINALYEARPROJECT));
    wcex.hCursor = LoadCursor(nullptr, IDC_ARROW);
    wcex.hbrBackground = reinterpret_cast<HBRUSH>(COLOR_WINDOW + 1);
    wcex.lpszMenuName = MAKEINTRESOURCEW(IDC_FINALYEARPROJECT);
    wcex.lpszClassName = windowClass.data();
    wcex.hIconSm = LoadIcon(instance, MAKEINTRESOURCE(IDI_SMALL));
    RegisterClassEx(&wcex);
}

auto Game::createWindow() -> void {
    windowHandle = CreateWindow(
        windowClass.data(),
        windowTitle.data(),
        WS_OVERLAPPEDWINDOW,
        CW_USEDEFAULT,
        0,
        CW_USEDEFAULT,
        0,
        nullptr,
        nullptr,
        instance,
        nullptr
    );
}

auto Game::createDeviceD3D() -> void {
    direct3D9 = Direct3DCreate9(D3D_SDK_VERSION);

    direct3DParams.Windowed = TRUE;
    direct3DParams.SwapEffect = D3DSWAPEFFECT_DISCARD;
    direct3DParams.BackBufferFormat = D3DFMT_UNKNOWN;
    direct3DParams.EnableAutoDepthStencil = TRUE;
    direct3DParams.AutoDepthStencilFormat = D3DFMT_D16;
    direct3DParams.PresentationInterval = D3DPRESENT_INTERVAL_IMMEDIATE;

    direct3D9->CreateDevice(
        D3DADAPTER_DEFAULT,
        D3DDEVTYPE_HAL,
        windowHandle,
        D3DCREATE_HARDWARE_VERTEXPROCESSING,
        &direct3DParams,
        &direct3DDevice9
    );
}

auto Game::cleanupDeviceD3D() -> void {
    direct3DDevice9.Reset();
    direct3D9.Reset();
}

auto Game::resetDevice() -> void {
    ImGui_ImplDX9_InvalidateDeviceObjects();
    direct3DDevice9->Reset(&direct3DParams);
    ImGui_ImplDX9_CreateDeviceObjects();
}

auto Game::windowProc(
    const HWND      hWnd,
    const UINT      message,
    const WPARAM    wParam,
    const LPARAM    lParam
) -> LRESULT {
    if (ImGui_ImplWin32_WndProcHandler(hWnd, message, wParam, lParam))
        return TRUE;

    switch (message) {
    case WM_SIZE:
        if (wParam == SIZE_MINIMIZED) {
            return FALSE;
        }
        game->windowWidth = static_cast<decltype(game->windowWidth)>(LOWORD(lParam));
        game->windowHeight = static_cast<decltype(game->windowHeight)>(HIWORD(lParam));
        return FALSE;
    case WM_SYSCOMMAND:
        if ((wParam & 0xFFF0) == SC_KEYMENU) {
            return FALSE;
        }
        break;
    case WM_DESTROY:
        PostQuitMessage(EXIT_SUCCESS);
        return FALSE;
    }

    return DefWindowProc(hWnd, message, wParam, lParam);
}

auto Game::run() -> int {
    ShowWindow(windowHandle, showCmd);
    UpdateWindow(windowHandle);

    constexpr ImVec4 clearColor{ 0.45f, 0.55f, 0.60f, 1.00f };
    MSG msg;
    bool exit = false;
    Object::objects.emplace_back(std::make_unique<Player>(ImVec2{ 500.f, 500.f }));
    do {
        while (PeekMessage(&msg, nullptr, 0, 0, PM_REMOVE)) {
            TranslateMessage(&msg);
            DispatchMessage(&msg);
            if (msg.message == WM_QUIT) {
                exit = true;
            }
        }

        if (exit) {
            break;
        }

        if (windowWidth != 0 && windowHeight != 0) {
            direct3DParams.BackBufferWidth = windowWidth;
            direct3DParams.BackBufferHeight = windowHeight;
            resetDevice();
        }

        ImGui_ImplDX9_NewFrame();
        ImGui_ImplWin32_NewFrame();
        ImGui::NewFrame();

        Object::objects.reserve(Object::objects.size() + Object::spawnedObjects.size());
        std::move(
            std::make_move_iterator(Object::spawnedObjects.begin()),
            std::make_move_iterator(Object::spawnedObjects.end()),
            std::back_inserter(Object::objects)
        );
        Object::spawnedObjects.clear();

        std::erase_if(Object::objects,
                      [](const std::unique_ptr<Object>& object) {
                          return
                              (object->pos.x > ImGui::GetIO().DisplaySize.x || object->pos.x < .0) ||
                              (object->pos.y > ImGui::GetIO().DisplaySize.y || object->pos.y < .0);
                      });

        for (auto&& object : Object::objects) {
            object->update();
            object->draw();
        }

        ImGui::Begin("Debug");
        ImGui::Text("Objects: %d", Object::objects.size());
        ImGui::End();

        ImGui::EndFrame();
        direct3DDevice9->SetRenderState(D3DRS_ZENABLE, FALSE);
        direct3DDevice9->SetRenderState(D3DRS_ALPHABLENDENABLE, FALSE);
        direct3DDevice9->SetRenderState(D3DRS_SCISSORTESTENABLE, FALSE);
        const D3DCOLOR clearColorDx = D3DCOLOR_RGBA((int)(clearColor.x * clearColor.w * 255.0f), (int)(clearColor.y * clearColor.w * 255.0f), (int)(clearColor.z * clearColor.w * 255.0f), (int)(clearColor.w * 255.0f));
        direct3DDevice9->Clear(0, nullptr, D3DCLEAR_TARGET | D3DCLEAR_ZBUFFER, clearColorDx, 1.0f, 0);
        if (direct3DDevice9->BeginScene() >= 0) {
            ImGui::Render();
            ImGui_ImplDX9_RenderDrawData(ImGui::GetDrawData());
            direct3DDevice9->EndScene();
        }

        // Handle loss of D3D9 device
        const HRESULT result = direct3DDevice9->Present(
            nullptr,
            nullptr,
            nullptr,
            nullptr
        );
        if (result == D3DERR_DEVICELOST && direct3DDevice9->TestCooperativeLevel() == D3DERR_DEVICENOTRESET) {
            resetDevice();
        }
    } while (true);

    return static_cast<int>(msg.wParam);
}
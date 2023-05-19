#pragma once

class Game final {
public:
    Game(const HINSTANCE instance, const int showCmd);
    ~Game();

    Game(Game&&) = delete;
    Game& operator=(Game&&) = delete;

    Game(const Game&) = delete;
    Game& operator=(const Game&) = delete;

    auto run() -> int;

private:
    HINSTANCE instance = nullptr;
    int showCmd = 0;

    HWND windowHandle = nullptr;

    std::array<TCHAR, 100> windowTitle = {};
    std::array<TCHAR, 100> windowClass = {};

    UINT windowWidth = 0;
    UINT windowHeight = 0;

    ComPtr<IDirect3D9> direct3D9;
    ComPtr<IDirect3DDevice9> direct3DDevice9;
    D3DPRESENT_PARAMETERS direct3DParams = {};

    auto loadTitle() -> void;
    auto loadClass() -> void;

    auto registerClass() const -> void;
    auto createWindow() -> void;

    auto createDeviceD3D() -> void;
    auto cleanupDeviceD3D() -> void;
    auto resetDevice() -> void;

    auto initImGui() const -> void;
    auto cleanupImGui() const -> void;

    static auto CALLBACK windowProc(
        const HWND      hWnd,
        const UINT      message,
        const WPARAM    wParam,
        const LPARAM    lParam
    )->LRESULT;
};

inline std::unique_ptr<Game> game;
#pragma once

class Game final {
public:
    Game(const HINSTANCE instance, const int showCmd);

    auto update() -> int;

private:
    HINSTANCE instance = nullptr;

    std::array<TCHAR, 100> windowTitle = {};
    std::array<TCHAR, 100> windowClass = {};

    HACCEL accelTable = nullptr;

    static auto CALLBACK WindowProc(
        const HWND      hWnd,
        const UINT      message,
        const WPARAM    wParam,
        const LPARAM    lParam
    )->LRESULT;

    static auto CALLBACK About(
        const HWND      hDlg,
        const UINT      message,
        const WPARAM    wParam,
        const LPARAM    lParam
    )->INT_PTR;
};

inline std::unique_ptr<Game> game;
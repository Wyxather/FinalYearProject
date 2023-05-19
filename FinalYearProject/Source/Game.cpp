#include "Game.h"

Game::Game(
    const HINSTANCE instance,
    const int       showCmd
) :
    instance{ instance } {
    const auto windowTitleLength = LoadString(
        instance,
        IDS_APP_TITLE,
        windowTitle.data(),
        windowTitle.size()
    );
    windowTitle[windowTitleLength] = '\0';

    const auto windowClassLength = LoadString(
        instance,
        IDC_FINALYEARPROJECT,
        windowClass.data(),
        windowClass.size()
    );
    windowClass[windowClassLength] = '\0';

    WNDCLASSEX wcex;
    wcex.cbSize = sizeof(wcex);
    wcex.style = CS_HREDRAW | CS_VREDRAW;
    wcex.lpfnWndProc = &WindowProc;
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

    const auto windowHandle = CreateWindow(
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

    ShowWindow(windowHandle, showCmd);
    UpdateWindow(windowHandle);

    accelTable = LoadAccelerators(instance, MAKEINTRESOURCE(IDC_FINALYEARPROJECT));
}

auto Game::WindowProc(
    const HWND      hWnd,
    const UINT      message,
    const WPARAM    wParam,
    const LPARAM    lParam
) -> LRESULT {
    switch (message) {
    case WM_COMMAND:
    {
        const auto wmId = LOWORD(wParam);
        // Parse the menu selections:
        switch (wmId) {
        case IDM_ABOUT:
            DialogBox(game->instance, MAKEINTRESOURCE(IDD_ABOUTBOX), hWnd, &About);
            break;
        case IDM_EXIT:
            DestroyWindow(hWnd);
            break;
        default:
            return DefWindowProc(hWnd, message, wParam, lParam);
        }
    }
    break;
    case WM_PAINT:
    {
        PAINTSTRUCT ps;
        const auto hdc = BeginPaint(hWnd, &ps);
        // TODO: Add any drawing code that uses hdc here...
        EndPaint(hWnd, &ps);
    }
    break;
    case WM_DESTROY:
        PostQuitMessage(0);
        break;
    default:
        return DefWindowProc(hWnd, message, wParam, lParam);
    }
    return 0;
}

auto Game::About(
    const HWND      hDlg,
    const UINT      message,
    const WPARAM    wParam,
    const LPARAM    lParam
) -> INT_PTR {
    UNREFERENCED_PARAMETER(lParam);

    switch (message) {
    case WM_INITDIALOG:
        return TRUE;

    case WM_COMMAND:
        if (LOWORD(wParam) == IDOK || LOWORD(wParam) == IDCANCEL) {
            EndDialog(hDlg, LOWORD(wParam));
            return TRUE;
        }
        break;
    }
    return FALSE;
}

auto Game::update() -> int {
    MSG message;
    while (GetMessage(&message, nullptr, 0, 0)) {
        if (!TranslateAccelerator(message.hwnd, accelTable, &message)) {
            TranslateMessage(&message);
            DispatchMessage(&message);
        }
    }
    return static_cast<int>(message.wParam);
}